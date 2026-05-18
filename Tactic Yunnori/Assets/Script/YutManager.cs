using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class YutManager : NetworkBehaviour
{
    public static YutManager Instance;

    [Header("수집된 데이터")]
    public List<bool> playerChoices = new List<bool>();

    [Header("결과창 UI")]
    public YutResultUI resultUI;

    private void Awake()
    {
        Instance = this;
    }

    public void SubmitChoice(bool isFront)
    {
        SubmitChoiceServerRpc(isFront);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void SubmitChoiceServerRpc(bool isFront)
    {
        if (playerChoices.Count >= 4) return;

        playerChoices.Add(isFront);

        Debug.Log($"플레이어 {playerChoices.Count} 선택 완료. (현재 {playerChoices.Count}/4)");

        if (playerChoices.Count == 4)
        {
            bool[] finalResults = playerChoices.ToArray();

            int frontCount = finalResults.Count(c => c == true);
            int moveCount = 0;

            switch (frontCount)
            {
                case 4: moveCount = 5; break;
                case 3: moveCount = 1; break;
                case 2: moveCount = 2; break;
                case 1: moveCount = 3; break;
                case 0: moveCount = 4; break;
            }

            ShowResultClientRpc(finalResults, moveCount);

            playerChoices.Clear();
        }
    }

    [ClientRpc]
    private void ShowResultClientRpc(bool[] finalResults, int moveCount)
    {
        if (resultUI != null)
        {
            resultUI.ShowResult(finalResults);
        }

        YutPiece piece = FindFirstObjectByType<YutPiece>();
        if (piece != null)
        {
            StartCoroutine(piece.MoveRoutine(moveCount));
        }
    }
}
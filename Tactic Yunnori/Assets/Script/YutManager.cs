using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class YutManager : MonoBehaviour
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
        if (playerChoices.Count >= 4) return;

        playerChoices.Add(isFront);
        Debug.Log($"플레이어 {playerChoices.Count} 선택 완료. (현재 {playerChoices.Count}/4)");

        if (playerChoices.Count == 4)
        {
            ProcessResult();
        }
    }

    private void ProcessResult()
    {
        int frontCount = playerChoices.Count(c => c == true);
        string resultName = "";

        switch (frontCount)
        {
            case 4: resultName = "모 (앞4)"; break;
            case 3: resultName = "빽도 (앞3, 뒤1)"; break;
            case 2: resultName = "개 (앞2, 뒤2)"; break;
            case 1: resultName = "걸 (앞1, 뒤3)"; break;
            case 0: resultName = "윷 (뒤4)"; break;
        }

        Debug.Log($"<color=yellow>전체 결과 발표: {resultName}!</color>");

        if (resultUI != null)
        {
            resultUI.ShowResult(playerChoices.ToArray());
        }
    }
}

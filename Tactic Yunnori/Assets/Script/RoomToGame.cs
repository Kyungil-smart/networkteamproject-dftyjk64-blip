using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class RoomToGame : NetworkBehaviour
{
    public Button redButton;
    public Button blueButton;
    public Button noneButton;
    public Button startButton;

    void Start()
    {
        noneButton.onClick.AddListener(() => SelectTeam(0));
        redButton.onClick.AddListener(() => SelectTeam(1));
        blueButton.onClick.AddListener(() => SelectTeam(2));

        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void SelectTeam(int teamNumber)
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.LocalClient != null)
        {
            var myPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
           
            if (myPlayer != null)
            {
                myPlayer.GetComponent<PlayerTeamSelector>().SelectTeam(teamNumber);
            }
        }
    }

    void OnStartButtonClick()
    {
        if (!IsServer) return;

        int redCount = 0;
        int blueCount = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject != null)
            {
                var selector = client.PlayerObject.GetComponent<PlayerTeamSelector>();
                if (selector != null)
                {
                    if (selector.TeamIndex.Value == 1) redCount++;
                    else if (selector.TeamIndex.Value == 2) blueCount++;
                }
            }
        }

        if (redCount == 2 && blueCount == 2)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning($"인원이 부족합니다. 현재 Red: {redCount}/2, Blue: {blueCount}/2 (4명이 모두 차야 시작됩니다.)");
        }
    }
}
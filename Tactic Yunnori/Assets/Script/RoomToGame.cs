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
        var myPlayer = NetworkManager.Singleton.LocalClient.PlayerObject;
        if (myPlayer != null)
        {
            myPlayer.GetComponent<PlayerTeamSelector>().SelectTeam(teamNumber);
        }
    }

    void OnStartButtonClick()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
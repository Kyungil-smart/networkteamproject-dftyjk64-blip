using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeamSelector : NetworkBehaviour
{
    public NetworkVariable<int> TeamIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private TextMeshProUGUI redText;
    private TextMeshProUGUI blueText;
    private TextMeshProUGUI noneText;
    private Button startButton;

    public override void OnNetworkSpawn()
    {
        FindUiComponents();

        TeamIndex.OnValueChanged += (oldValue, newValue) =>
        {
            FindUiComponents();
            UpdateTeamCountUI();
        };

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        UpdateTeamCountUI();
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId) => UpdateTeamCountUI();
    private void OnClientDisconnected(ulong clientId) => UpdateTeamCountUI();

    private void FindUiComponents()
    {
        if (redText == null) redText = GameObject.Find("RedCountText")?.GetComponent<TextMeshProUGUI>();
        if (blueText == null) blueText = GameObject.Find("BlueCountText")?.GetComponent<TextMeshProUGUI>();
        if (noneText == null) noneText = GameObject.Find("NoneCountText")?.GetComponent<TextMeshProUGUI>();
        if (startButton == null) startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
    }

    private void UpdateTeamCountUI()
    {
        FindUiComponents();

        int redCount = 0;
        int blueCount = 0;
        int noneCount = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject != null)
            {
                var selector = client.PlayerObject.GetComponent<PlayerTeamSelector>();
                if (selector != null)
                {
                    if (selector.TeamIndex.Value == 1) redCount++;
                    else if (selector.TeamIndex.Value == 2) blueCount++;
                    else noneCount++;
                }
            }
        }

        if (redText != null) redText.text = $"{redCount}/2";
        if (blueText != null) blueText.text = $"{blueCount}/2";
        if (noneText != null) noneText.text = $"{noneCount}/4";

        if (IsServer && startButton != null)
        {
            startButton.interactable = (redCount == 2 && blueCount == 2);
        }
    }

    public void SelectTeam(int team)
    {
        if (IsOwner)
        {
            RequestSelectTeamServerRpc(team);
        }
    }

    [ServerRpc]
    private void RequestSelectTeamServerRpc(int team)
    {
        int targetTeamCount = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject != null)
            {
                var selector = client.PlayerObject.GetComponent<PlayerTeamSelector>();
                if (selector != null && selector.TeamIndex.Value == team)
                {
                    targetTeamCount++;
                }
            }
        }

        if (team == 1 && targetTeamCount >= 2) return;
        if (team == 2 && targetTeamCount >= 2) return;
        if (team == 0 && targetTeamCount >= 4) return;

        TeamIndex.Value = team;
    }
}
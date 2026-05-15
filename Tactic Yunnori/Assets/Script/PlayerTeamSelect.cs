using Unity.Netcode;
using UnityEngine;

public class PlayerTeamSelector : NetworkBehaviour
{
    public NetworkVariable<int> TeamIndex =
        new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
        int count = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var selector = client.PlayerObject.GetComponent<PlayerTeamSelector>();
            if (selector.TeamIndex.Value == team)
            {
                count++; 
            }
        }

        if (count < 2)
        {
            TeamIndex.Value = team;
        }
    }
}
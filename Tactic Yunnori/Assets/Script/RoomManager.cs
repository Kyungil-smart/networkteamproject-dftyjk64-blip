using Unity.Netcode;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClients.Count >= 4)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("RoomScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        };
    }
}
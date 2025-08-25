using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master. Joining/creating room...");
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room. Spawning player...");
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Make sure prefab name matches exactly and prefab is in Assets/Resources/
        PhotonNetwork.Instantiate("PlayerJo", Vector3.zero, Quaternion.identity);
    }
}

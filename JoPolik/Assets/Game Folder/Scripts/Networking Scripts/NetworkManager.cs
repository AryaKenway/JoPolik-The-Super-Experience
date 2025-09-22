using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Vector3[] spawnPositions; 

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
        Vector3 spawnPos = Vector3.zero;

        if (spawnPositions != null && spawnPositions.Length > 0)
        {
            int index = Random.Range(0, spawnPositions.Length);
            spawnPos = spawnPositions[index];
        }

        PhotonNetwork.Instantiate("PlayerJo", spawnPos, Quaternion.identity);
    }
}

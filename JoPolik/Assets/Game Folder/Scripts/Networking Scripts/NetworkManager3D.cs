using Photon.Pun;
using UnityEngine;

public class NetworkManager3D : MonoBehaviourPunCallbacks
{
    public string playerPrefabName = "BA_First Person Player"; 

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
        else
        {
            Debug.LogError("Not in a Photon room! Player cannot spawn yet.");
        }
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Vector3 spawnPos = new Vector3(1.39f, 5.05f, -6.14f);
        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnPos, Quaternion.identity);

        if (player != null)
        {
            Debug.Log("Player spawned successfully in 3D scene via Photon: " + player.name);
        }
        else
        {
            Debug.LogError("Player failed to spawn via Photon!");
        }
    }
}

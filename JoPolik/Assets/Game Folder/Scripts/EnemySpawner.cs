//using UnityEngine;
//using Photon.Pun;

//public class EnemySpawner : MonoBehaviourPun
//{
//    public string enemyPrefabName = "KiloTheUnjust"; // prefab must be in Resources folder
//    public Vector3 spawnPosition = new Vector3(0, 0, 0);

//    void Awake()
//    {
//        if (PhotonNetwork.IsMasterClient) // only master client spawns
//        {
//            GameObject enemy =PhotonNetwork.Instantiate(enemyPrefabName, spawnPosition, Quaternion.identity);
//            Debug.Log("Spawned enemy: " + enemy.name);

//        }
//    }
//}

//using UnityEngine;
//using Photon.Pun;

//public class EnemySpawner : MonoBehaviourPun
//{
//    void Awake()
//    {
//        Debug.Log("EnemySpawner Awake called!"); // should ALWAYS show if script is attached and running

//        if (PhotonNetwork.IsMasterClient)
//        {
//            GameObject enemy = PhotonNetwork.Instantiate("KiloTheUnjust", Vector3.zero, Quaternion.identity);
//            Debug.Log("Spawned enemy: " + enemy.name);
//        }
//    }
//}


using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    public string enemyPrefabName = "KiloTheUnjust"; // prefab must be in Resources folder
    public Vector3 spawnPosition = new Vector3(0, 0, 0);

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room! Spawning check…");

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject enemy = PhotonNetwork.Instantiate(enemyPrefabName, spawnPosition, Quaternion.identity);
            Debug.Log("Spawned enemy: " + enemy.name);
        }
    }
}


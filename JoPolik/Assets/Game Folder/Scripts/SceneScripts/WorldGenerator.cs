using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] groundTiles;  
    public GameObject[] props;        

    public int worldSize = 20;        
    public float tileSize = 5f;       

    [Header("No Spawn Zone (Building Footprint)")]
    public Vector3 buildingCenter = Vector3.zero;
    public Vector2 buildingSize = new Vector2(40f, 30f); 

    [Header("World Seed")]
    public int seed = 0;  

    void Start()
    {
        if (seed == 0)
        {
            seed = System.DateTime.Now.GetHashCode();
        }

        Random.InitState(seed);
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = -worldSize; x <= worldSize; x++)
        {
            for (int z = -worldSize; z <= worldSize; z++)
            {
                Vector3 spawnPos = new Vector3(x * tileSize, 0, z * tileSize);

                if (InsideNoSpawn(spawnPos))
                    continue;

                GameObject tilePrefab = groundTiles[Random.Range(0, groundTiles.Length)];
                GameObject tile= Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                if (Random.value < 0.2f) // 20% chance
                {
                    GameObject propPrefab = props[Random.Range(0, props.Length)];

                    Vector3 propPos = new Vector3(spawnPos.x, tile.transform.position.y, spawnPos.z);

                    Instantiate(propPrefab, propPos, Quaternion.identity, transform);
                }
            }
        }
    }

    bool InsideNoSpawn(Vector3 pos)
    {
        return Mathf.Abs(pos.x - buildingCenter.x) < buildingSize.x / 2f &&
               Mathf.Abs(pos.z - buildingCenter.z) < buildingSize.y / 2f;
    }
    
    public float GetGroundYAt(Vector3 pos)
    {
        RaycastHit hit;
        Vector3 origin = pos + Vector3.up * 10f;
        if (Physics.Raycast(origin, Vector3.down, out hit, 50f))
        {
            return hit.point.y;
        }
        return 0f;
    }

}

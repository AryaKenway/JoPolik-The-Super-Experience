using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] groundTiles;  // grass, dirt, road prefabs
    public GameObject[] props;        // trees, rocks, etc.

    public int worldSize = 20;        // number of tiles outwards from center
    public float tileSize = 5f;       // spacing between tiles

    [Header("No Spawn Zone (Building Footprint)")]
    public Vector3 buildingCenter = Vector3.zero;
    public Vector2 buildingSize = new Vector2(40f, 30f); // width (X) & depth (Z)

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = -worldSize; x <= worldSize; x++)
        {
            for (int z = -worldSize; z <= worldSize; z++)
            {
                Vector3 spawnPos = new Vector3(x * tileSize, 0, z * tileSize);

                // Skip inside building footprint
                if (InsideNoSpawn(spawnPos))
                    continue;

                // Spawn a ground tile
                GameObject tilePrefab = groundTiles[Random.Range(0, groundTiles.Length)];
                Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                // Randomly spawn a prop
                if (Random.value < 0.2f) // 20% chance
                {
                    GameObject propPrefab = props[Random.Range(0, props.Length)];
                    Instantiate(propPrefab, spawnPos + Vector3.up * 0.5f, Quaternion.identity, transform);
                }
            }
        }
    }

    bool InsideNoSpawn(Vector3 pos)
    {
        return Mathf.Abs(pos.x - buildingCenter.x) < buildingSize.x / 2f &&
               Mathf.Abs(pos.z - buildingCenter.z) < buildingSize.y / 2f;
    }
}

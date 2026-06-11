using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;        // assign JumpBox + SlideBar
    [Range(0f, 1f)] public float spawnChance = 0.6f;
    public float[] lanes = { -1.5f, 0f, 1.5f };
    public float tileLength = 20f;

    public void PopulateTile(Transform tile)
    {
        // Clear obstacles from this tile's previous life
        for (int i = tile.childCount - 1; i >= 0; i--)
        {
            Transform child = tile.GetChild(i);
            if (child.CompareTag("Obstacle")) Destroy(child.gameObject);
        }

        if (obstaclePrefabs.Length == 0 || Random.value > spawnChance) return;

        int idx = Random.Range(0, obstaclePrefabs.Length);
        float laneX = lanes[Random.Range(0, lanes.Length)];
        float zJitter = Random.Range(-tileLength * 0.3f, tileLength * 0.3f);

        // worldPositionStays = false → prefab's transform is treated as local to the tile
        GameObject obstacle = Instantiate(obstaclePrefabs[idx], tile, false);
        obstacle.transform.localPosition = new Vector3(
            laneX,
            obstacle.transform.localPosition.y,   // keep the prefab's height (0.5 or 1.7)
            zJitter);
    }
}
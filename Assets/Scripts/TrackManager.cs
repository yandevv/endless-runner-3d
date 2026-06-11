using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform player;
    public ObstacleSpawner obstacleSpawner;
    public int tileCount = 6;
    public float tileLength = 20f;

    private Queue<GameObject> tiles = new Queue<GameObject>();
    private float nextSpawnZ = 0f;
    private int tilesSpawned = 0;

    void Start()
    {
        for (int i = 0; i < tileCount; i++) SpawnTile();
    }

    void Update()
    {
        // Recycle every tile that's fully behind the player
        while (tiles.Peek().transform.position.z < player.position.z - tileLength)
            RecycleTile();
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefab, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
        if (tilesSpawned >= 2)                       // first two tiles stay clear
            obstacleSpawner.PopulateTile(tile.transform);
        tiles.Enqueue(tile);
        nextSpawnZ += tileLength;
        tilesSpawned++;
    }

    void RecycleTile()
    {
        GameObject tile = tiles.Dequeue();
        tile.transform.position = new Vector3(0, 0, nextSpawnZ);
        obstacleSpawner.PopulateTile(tile.transform);   // repopulates + clears old ones
        tiles.Enqueue(tile);
        nextSpawnZ += tileLength;
    }
}
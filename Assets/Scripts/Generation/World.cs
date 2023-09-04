using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class World : MonoBehaviour
{
    [SerializeField]
    public Dictionary<Vector2Int, ChunkData> ChunkDatas = new Dictionary<Vector2Int, ChunkData>();
    public Dictionary<Vector2Int, ChunkData> ChunkDatasWater = new Dictionary<Vector2Int, ChunkData>();
    public ChunkRenderer ChunkPrefab;
    public ChunkRenderer ChunkWaterPrefab;

    public TerrainGenerator hightMap;

    public bool already = true;

    private void Start()
    {
        //if (already)
        //{
            hightMap = gameObject.GetComponent<TerrainGenerator>();
            hightMap.GenerateMap();
            GenerateWorld();
            already = true;
        //}
    }

    private void Update()
    {
        hightMap = gameObject.GetComponent<TerrainGenerator>();
        if (Input.GetKeyDown(KeyCode.H))
        {
            var a = GameObject.FindGameObjectsWithTag("Respawn");
            foreach (var e in a)
            {
                Destroy(e);
            }
            a = null;
            ChunkDatas.Clear();
            ChunkDatasWater.Clear();
            //ChunkDatasStone.Clear();

            hightMap.GenerateMap();

            GenerateWorld();
        }
    }

    private void GenerateWorld()
    {
        for (int x = 0; x < hightMap.count; x++)
        {
            for (int y = 0; y < hightMap.count; y++)
            {
                float xPos = x * hightMap.chunkWidth * hightMap.blockScale;
                float zPos = y * hightMap.chunkWidth * hightMap.blockScale;

                ChunkData chunkData = new ChunkData();
                chunkData.ChunkPosition = new Vector2Int(x, y);
                var a = gameObject.GetComponent<TerrainGenerator>();
                chunkData.Blocks = a.GenerateTerrain1(x, y, "grass");
                ChunkDatas.Add(new Vector2Int(x, y), chunkData);

                var chunk = Instantiate(ChunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
                chunk.ChunkData = chunkData;
                chunk.ParentWorld = this;
                chunk.type = "grass";
            }
        }

        for (int x = 0; x < hightMap.count; x++)
        {
            for (int y = 0; y < hightMap.count; y++)
            {
                float xPos = x * hightMap.chunkWidth * hightMap.blockScale;
                float zPos = y * hightMap.chunkWidth * hightMap.blockScale;

                ChunkData chunkData2 = new ChunkData();
                chunkData2.ChunkPosition = new Vector2Int(x, y);
                var a = gameObject.GetComponent<TerrainGenerator>();
                chunkData2.Blocks = a.GenerateTerrain1(x, y, "water");
                ChunkDatasWater.Add(new Vector2Int(x, y), chunkData2);

                var chunk = Instantiate(ChunkWaterPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
                chunk.ChunkData = chunkData2;
                chunk.ParentWorld = this;
                chunk.type = "water";
            }
        }
    }
}
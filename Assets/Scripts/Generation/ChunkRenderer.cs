using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class ChunkRenderer : MonoBehaviour
{
    public string type;

    private int ChunkWidth = 120;
    private int ChunkHeight = 256;
    private float BlockScale = .75f;

    public ChunkData ChunkData;
    public World ParentWorld;

    public List<Vector3> verticies = new List<Vector3>();
    public List<int> triangles = new List<int>();

    public bool already = true;

    private void Start()
    {
        //if (!already)
        //{
            MainGeneration();
            already = true;
        //}
    }

    private void MainGeneration()
    {
        var a = GameObject.FindGameObjectWithTag("World").GetComponent<TerrainGenerator>();
        ChunkWidth = a.chunkWidth;
        ChunkHeight = a.chunkHeight;
        BlockScale = a.blockScale;

        Vector3[] uvs = new Vector3[(ChunkWidth + 1) * (ChunkWidth + 1)];

        Mesh chunkMesh = new Mesh();

        for (int y = 0; y < ChunkHeight; y++)
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    GenerateBlock(x, y, z);
                }
            }
        }

        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        //GetComponent<MeshRenderer>().material = 
        GetComponent<MeshCollider>().sharedMesh = chunkMesh;

        verticies.Clear();
        triangles.Clear();
    }

    private void GenerateBlock(int x, int y, int z)
    {
        var blockPosition = new Vector3Int(x, y, z);

        if (GetBlockAtPosition(blockPosition) == 0) return;

        if (type != "water") if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0) GeneratingRightSide(blockPosition);
        if (type != "water") if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0) GeneratingLeftSide(blockPosition);
        if (type != "water") if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0) GeneratingFrontSide(blockPosition);
        if (type != "water") if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0) GeneratingBackSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0) GeneratingTopSide(blockPosition);
        //if (type != "water") if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0) GeneratingBottomSide(blockPosition);

    }

    private BlockType GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
        {
            return ChunkData.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            if (blockPosition.y < 0 || blockPosition.y >= ChunkHeight) return BlockType.Air;

            Vector2Int adjChunkPosition = ChunkData.ChunkPosition;
            if (blockPosition.x < 0)
            {
                adjChunkPosition.x--;
                blockPosition.x += ChunkWidth;
            }
            else if (blockPosition.x >= ChunkWidth)
            {
                adjChunkPosition.x++;
                blockPosition.x -= ChunkWidth;
            }

            if (blockPosition.z < 0)
            {
                adjChunkPosition.y--;
                blockPosition.z += ChunkWidth;
            }
            else if (blockPosition.z >= ChunkWidth)
            {
                adjChunkPosition.y++;
                blockPosition.z -= ChunkWidth;
            }

            if (ParentWorld.ChunkDatas.TryGetValue(adjChunkPosition, out ChunkData adjChunk))
            {
                return adjChunk.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
            }
            else
            {
                return BlockType.Air;
            }
        }
    }

    private void GeneratingRightSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVertexSquare();
    }

    private void GeneratingLeftSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);

        AddLastVertexSquare();
    }

    private void GeneratingFrontSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVertexSquare();
    }

    private void GeneratingBackSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);

        AddLastVertexSquare();
    }

    private void GeneratingTopSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVertexSquare();
    }

    private void GeneratingBottomSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);

        AddLastVertexSquare();
    }

    private void AddLastVertexSquare()
    {
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
    }
}
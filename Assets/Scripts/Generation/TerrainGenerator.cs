using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    public int chunkWidth = 120;
    public int chunkHeight = 256;
    public float blockScale = .75f;

    public float[,] heghtMap;
    public float[,] heightMapSecond;
    public int height = 50;
    public int mult = 10;
    public int count = 10;
    public float scale = 10f;
    public float scale2 = 10f;
    public int waterHeight = 2;

    private Vector2 NoiseOffset = Vector2.zero;
    private Vector2 NoiselScale = Vector2.one;

    private void Start()
    {
        heghtMap = new float[(chunkWidth * count) + count,
            (chunkWidth * count) + count];
        heightMapSecond = new float[(chunkWidth * count) + count,
            (chunkWidth * count) + count];
        GenerateMap();
    }

    public void GenerateMap()
    {
        heghtMap = new float[(chunkWidth * count) + count,
            (chunkWidth * count) + count];
        heightMapSecond = new float[(chunkWidth * count) + count,
            (chunkWidth * count) + count];

        for (int i = 0; i < ((chunkWidth * count) + count); i++)
        {
            for (int j = 0; j < ((chunkWidth * count) + count); j++)
            {
                float PerlinNoiseX = NoiseOffset.x + (i * scale / 10) / chunkWidth * NoiselScale.x;
                float PerlinNoiseY = NoiseOffset.y + (j * scale / 10) / chunkWidth * NoiselScale.y;
                float noise = Mathf.PerlinNoise(PerlinNoiseX, PerlinNoiseY);

                heghtMap[i, j] = noise;
            }
        }

        for (int i = 0; i < ((chunkWidth * count) + count); i++)
        {
            for (int j = 0; j < ((chunkWidth * count) + count); j++)
            {
                float PerlinNoiseX = NoiseOffset.x + (i * scale2 / 10) / chunkWidth * 2 * NoiselScale.x;
                float PerlinNoiseY = NoiseOffset.y + (j * scale2 / 10) / chunkWidth * 2 * NoiselScale.y;
                float noise = Mathf.PerlinNoise(PerlinNoiseX, PerlinNoiseY);

                heightMapSecond[i, j] = noise;
            }
        }

        for (int i = 0; i < ((chunkWidth * count) + count); i++)
        {
            for (int j = 0; j < ((chunkWidth * count) + count); j++)
            {
                heghtMap[i, j] = (Mathf.Abs(heghtMap[i, j] - heightMapSecond[i, j])) * height + mult;
            }
        }
    }


    public BlockType[,,] GenerateTerrain1(int xOffset, int zOffset, string type)
    {
        var result = new BlockType[chunkWidth, chunkHeight, chunkWidth];

        for (int x = 0; x < chunkWidth; x++)
        {
            for (int z = 0; z < chunkWidth; z++)
            {
                int height = (int)heghtMap[(x + (xOffset * chunkWidth)),
                    (z + (zOffset * chunkWidth))] + 1;


                if (type == "grass")
                {
                    for (int y = 0; y < height; y++)
                    {
                        result[x, y, z] = BlockType.Grass;
                    }
                }
                else
                {
                    for (int y = 0; y < waterHeight + 10 / 4; y++)
                    {
                        result[x, y, z] = BlockType.Water;
                    }
                }

            }
        }

        return result;
    }
}
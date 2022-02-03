using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class HeightMap
{    
    public int mapSize;
    [SerializeField]
    public float[,] mapArray;
    public float multiplier = 25f;

    // Create and config noise
    FastNoiseLite noise = new FastNoiseLite();


    public HeightMap(int _mapSize, float heightMultipler, float pointSpacing, int octaves, float frequency, float lacunarity)
    {
        noise.SetFractalType(FastNoiseLite.FractalType.FBm); 
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFractalLacunarity(lacunarity);
        noise.SetFractalOctaves(octaves);
        noise.SetFrequency(frequency);
        multiplier = heightMultipler * pointSpacing;


        mapSize = _mapSize;
        mapArray = new float[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {

                mapArray[x, y] = noise.GetNoise(x, y) * multiplier;
                
            }
        }
    }
    
}

public class MapCell
{

}

public class TerrainMeshData
{
    public Vector3[] vertices;
    public int[] triangles;
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



[Serializable]
public class MapDTO
{
    public byte[] mapArray;
}

[Serializable]
public class TestClass
{
    public float number = 5f;

    public void SaveMap()
    {
        var dataAsJSON = JsonUtility.ToJson(this, true);
        string path = Application.persistentDataPath + "/savetest.json";
        Debug.Log(dataAsJSON);
        Debug.Log(path);

        File.WriteAllText(path, dataAsJSON);
    }
}

[Serializable]
public class HeightMap
{    
    public int mapSize;
    [SerializeField]
    public MapDTO mapDTO;
    public float multiplier = 25f;

    // Create and config noise
    FastNoiseLite noise = new FastNoiseLite();


    public HeightMap(int _mapSize, float heightMultipler, float pointSpacing, int octaves, float frequency, float lacunarity)
    {
        mapDTO = new MapDTO();
        noise.SetFractalType(FastNoiseLite.FractalType.FBm); 
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        noise.SetFractalLacunarity(lacunarity);
        noise.SetFractalOctaves(octaves);
        noise.SetFrequency(frequency);
        multiplier = heightMultipler * pointSpacing;


        mapSize = _mapSize;
        mapDTO.mapArray = new byte[mapSize* mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {

                //mapDTO.mapArray[x, y] = noise.GetNoise(x, y) * multiplier;
                mapDTO.mapArray[(y * mapSize) + x] = (byte)((((noise.GetNoise(x, y)) + 1)/2)  * multiplier);

            }
        }
    }

    public void InitMap()
    {

    }

    public void SaveMap()
    {
        var dataAsJSON = JsonUtility.ToJson(mapDTO);
        string path = Application.persistentDataPath + "/save2.json";
        string binpath = Application.persistentDataPath + "/save.data";

        FileStream dataStream = new FileStream(binpath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, mapDTO);

        dataStream.Close();

        Debug.Log(dataAsJSON);
        Debug.Log(path);

        File.WriteAllText(path, dataAsJSON);
    }
    
}

public class MapCell
{

}

public class TerrainMeshData
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;

}

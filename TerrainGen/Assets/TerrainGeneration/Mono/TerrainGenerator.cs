using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public HeightMap heightMap;
    public TerrainMeshData terrainMeshData;
    public float pointSpacing = 5f;
    public float heightMultiplier = 5f;
    public int mapSize;
    public int octaves;
    [Range(0f, 0.04f)]
    public float frequency;
    public float lacunarity;
    [Range(0f, 180f)]
    public float steepness;

    public Texture2D splatTexture;

    public Material TerrainMaterial;

    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private MeshFilter meshFilter;

    void Start()
    {

        GenerateHeightMap();


        if (TerrainMaterial)
            meshRenderer.sharedMaterial = TerrainMaterial;

        BuildMesh();
    }

    #region Editor
#if UNITY_EDITOR
    /*
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
        //_OnValidate();
    }*/
    private void OnValidate()
    {
        GenerateHeightMap();
 
        BuildMesh();
        //GenerateSplatTexture();
    }
#endif
    #endregion
    private void GenerateHeightMap()
    {
        heightMap = new HeightMap(mapSize, heightMultiplier, pointSpacing, octaves, frequency, lacunarity);
    }

    private void GenerateSplatTexture()
    {
        splatTexture = new Texture2D(mapSize, mapSize);
        Color[] pixels = new Color[mapSize * mapSize];

        int i = 0;
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                float angle = Vector3.Angle(meshFilter.sharedMesh.normals[i], Vector3.up);
                if (angle < steepness)
                    pixels[i] = Color.black;
                else
                    pixels[i] = Color.blue;

                
                i++;
            }
        }
        splatTexture.filterMode = FilterMode.Trilinear;
        splatTexture.SetPixels(pixels);
        
        splatTexture.Apply();

        meshRenderer.sharedMaterial.SetTexture("_SplatMap", splatTexture);
    }


    private void ClearMesh()
    {
        
    }

    private void BuildMesh()
    {
        int numQuads = (heightMap.mapSize - 1) * (heightMap.mapSize - 1);

        terrainMeshData = new TerrainMeshData();
        AssignVertices();
        AssignTriangles();
        AssignUVs();

        #region testing
        //terrainMeshData.vertices = new Vector3[(heightMap.mapSize * heightMap.mapSize) * 6];
        //Debug.Log("Vertices Length: " + terrainMeshData.vertices.Length);
        //terrainMeshData.triangles = new int[(numQuads * numQuads) * 6];



        //terrainMeshData.vertices = new Vector3[4];
        //terrainMeshData.triangles = new int[6];


        //
        // A Single Quad        
        /*
       terrainMeshData.vertices[0] = new Vector3(0f, heightMap.mapArray[0, 0], 0f);
       terrainMeshData.vertices[1] = new Vector3(5f, heightMap.mapArray[1, 0], 0f);
       terrainMeshData.vertices[2] = new Vector3(0f, heightMap.mapArray[0, 1], 5f);
       terrainMeshData.vertices[3] = new Vector3(5f, heightMap.mapArray[1, 1], 5f);

       terrainMeshData.triangles[0] = 0;
       terrainMeshData.triangles[1] = 2;
       terrainMeshData.triangles[2] = 1;

       terrainMeshData.triangles[3] = 3;
       terrainMeshData.triangles[4] = 1;
       terrainMeshData.triangles[5] = 2;

       //////////////////////////////////////
       ///

       terrainMeshData.vertices[4+ 0] = new Vector3(5f+ 0f, heightMap.mapArray[1+ 0, 0], 0f);
       terrainMeshData.vertices[4+ 1] = new Vector3(5f + 5f, heightMap.mapArray[1+ 1, 0], 0f);
       terrainMeshData.vertices[4+ 2] = new Vector3(5f + 0f, heightMap.mapArray[1+ 0, 1], 5f);
       terrainMeshData.vertices[4+ 3] = new Vector3(5f + 5f, heightMap.mapArray[1+ 1, 1], 5f);

       terrainMeshData.triangles[6 + 0] = 4 + 0;
       terrainMeshData.triangles[6 + 1] = 4 + 2;
       terrainMeshData.triangles[6 + 2] = 4 + 1;

       terrainMeshData.triangles[6 + 3] = 4 + 3;
       terrainMeshData.triangles[6 + 4] = 4 + 1;
       terrainMeshData.triangles[6 + 5] = 4 + 2;

       //////////////////////////////////////
       ///
       terrainMeshData.vertices[8 + 0] = new Vector3(10f + 0f, heightMap.mapArray[2 + 0, 0], 0f);
       terrainMeshData.vertices[8 + 1] = new Vector3(10f + 5f, heightMap.mapArray[2 + 1, 0], 0f);
       terrainMeshData.vertices[8 + 2] = new Vector3(10f + 0f, heightMap.mapArray[2 + 0, 1], 5f);
       terrainMeshData.vertices[8 + 3] = new Vector3(10f + 5f, heightMap.mapArray[2 + 1, 1], 5f);

       terrainMeshData.triangles[12 + 0] = 8 + 0;
       terrainMeshData.triangles[12 + 1] = 8 + 2;
       terrainMeshData.triangles[12 + 2] = 8 + 1;

       terrainMeshData.triangles[12 + 3] = 8 + 3;
       terrainMeshData.triangles[12 + 4] = 8 + 1;
       terrainMeshData.triangles[12 + 5] = 8 + 2;

       *//////////////////////////////////////
        ///
        #endregion


        var mesh = new Mesh();
        mesh.vertices = terrainMeshData.vertices;
        mesh.uv = terrainMeshData.uvs;
        mesh.triangles = terrainMeshData.triangles;


        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        meshFilter.sharedMesh = mesh;
    }

    private void AssignVertices()
    {
        terrainMeshData.vertices = new Vector3[mapSize * mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                terrainMeshData.vertices[(y * mapSize) + x] =
                    new Vector3(
                        x * pointSpacing,
                        heightMap.mapDTO.mapArray[(y * mapSize) + x],
                        y * pointSpacing);
            }
        }
    }
    private void AssignUVs()
    {
        terrainMeshData.uvs = new Vector2[mapSize * mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                terrainMeshData.uvs[(y * mapSize) + x] =
                    new Vector2(
                        ((float)x / mapSize ),                        
                        ((float)y / mapSize));

               
            }
        }
    }
    private void AssignTriangles()
    {
        terrainMeshData.triangles = new int[((mapSize - 1) * (mapSize - 1)) * 6];
        int i = 0; // triangle array index
        int t = 0; // triangle start in vertex array index

        for (int x = 0; x < mapSize - 1; x++)
        {
            for (int y = 0; y < mapSize - 1; y++)
            {
                terrainMeshData.triangles[i + 0] = t  + 0;
                terrainMeshData.triangles[i + 1] = t  + mapSize;
                terrainMeshData.triangles[i + 2] = t  + 1;


                terrainMeshData.triangles[i + 3] = t + mapSize +1;
                terrainMeshData.triangles[i + 4] = t  + 1;
                terrainMeshData.triangles[i + 5] = t  + mapSize;

                i += 6;
                t += 1;
            }
            t += 1; // Add 1 to triangle start index each time a row is finished.
        }
    }

    /*
    private void OnDrawGizmos()
    {
        for (int i = 0; i < meshFilter.sharedMesh.vertices.Length; i++)
        {
            float angle = Vector3.Angle(meshFilter.sharedMesh.normals[i], Vector3.up);
            

            if (angle <= steepness)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(meshFilter.sharedMesh.vertices[i], meshFilter.sharedMesh.vertices[i]+ meshFilter.sharedMesh.normals[i] * 6f);
        }
    }*/

}

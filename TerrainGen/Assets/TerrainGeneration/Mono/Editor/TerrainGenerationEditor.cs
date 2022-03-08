using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGenerationEditor : Editor
{
    Vector3 pos = Vector3.zero;
    float handleRadius = 0.2f;
    float maxHandleRadius = 10f;
    float minHandleRadius = 0.1f;
    float handleRadiusDelta = 0.01f;

    TerrainGenerator terrainGenerator;

    Texture2D emptyTexture;
    TestClass test = new TestClass();


    GUIStyle textStyle = new GUIStyle();



    

    private void OnEnable()
    {
        terrainGenerator = (TerrainGenerator)target;
        textStyle.normal.textColor = Color.red;


        /*
        byte[] fileData;

        if (File.Exists(".empty_32x32.png"))
        {
            fileData = File.ReadAllBytes(".empty_32x32.png");
            emptyTexture = new Texture2D(2, 2);
            emptyTexture.LoadImage(fileData);
        }*/

        

    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

       
        if (GUILayout.Button("Save Map"))
        {
            terrainGenerator.heightMap.SaveMap();
        }

        if(GUILayout.Button("Save Map as Texture"))
        {
           // Texture2D mapTexture = CreateMapTexture();
        }
    }


    /*
    private void OnSceneGUI()
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        var e = Event.current;
        if(e.keyCode == KeyCode.RightBracket)
            IncreaseHandleRadius();
        if(e.keyCode == KeyCode.LeftBracket)
            DecreaseHandleRadius();
        


        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(worldRay, out hitInfo, 10000))
        {
            
            if (hitInfo.collider.gameObject != null)
            {
                Cursor.SetCursor(emptyTexture, Vector2.zero , CursorMode.Auto);
                Handles.color = Color.white;
                //Handles.DrawWireDisc(hitInfo.point, new Vector3(0, 1, 0), handleRadius);
                Handles.color = Color.red;
                

                var arrayPos = ConvertMouseToHeightArrayIndex(hitInfo.point);
               
                Handles.Label(new Vector3(hitInfo.point.x, hitInfo.point.y - 0.7f, hitInfo.point.z), hitInfo.point.ToString() + "\n" + arrayPos.ToString(), textStyle);
                // Handles.Label(r, arrayPos.ToString(), textStyle);

                var yPos = GetHeightAtArrayPoint(new Vector2(arrayPos.x * terrainGenerator.pointSpacing, arrayPos.y * terrainGenerator.pointSpacing));
                Handles.color= Color.red;
                Handles.DrawSolidDisc(new Vector3(arrayPos.x* terrainGenerator.pointSpacing, yPos, arrayPos.y*terrainGenerator.pointSpacing), new Vector3(0,1, 0), 0.25f);

                
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        SceneView.RepaintAll();
    }
    */
    void IncreaseHandleRadius()
    {
        handleRadius += handleRadiusDelta;
        if(handleRadius >= maxHandleRadius)
            handleRadius = maxHandleRadius;

    }

    void DecreaseHandleRadius()
    {
        handleRadius -= handleRadiusDelta;
        if (handleRadius <= minHandleRadius)
            handleRadius = minHandleRadius;

    }
    public Vector2 ConvertMouseToHeightArrayIndex(Vector3 mousePosition)
    {
        var spacing = terrainGenerator.pointSpacing;

        var x = Mathf.Round(mousePosition.x / spacing);
        var y = Mathf.Round(mousePosition.z / spacing);

        return new Vector2(x, y);
    }

    private float GetHeightAtArrayPoint(Vector2 point)
    {
        Ray ray = new Ray(new Vector3(point.x, 1000, point.y), Vector3.down);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 10000))
        {
            return hitInfo.point.y;
        }
        else
        {
            return 0;
        }
    }
}

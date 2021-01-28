using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TerrainToolSet : MonoBehaviour
{

    public List<TerrainSetting> terrainSettings;
    public SizeSetting currentSize;
    public TerrainSetting GetTerrainSetting()
    {


        if (currentSize == SizeSetting.Random)
        {

            return terrainSettings.Random();
        }
        return terrainSettings.Where(t => t.Size == currentSize).ToList()[0];

    }
}

// TODO , Use this instead of easy buttons 
#if UNITY_EDITOR
[CustomEditor(typeof(TerrainToolSet))]
public class TerrainToolSetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (TerrainToolSet)target;


        DrawDefaultInspector();
        if (GUILayout.Button("Change Size"))
        {
            //myTarget.addEddyScripts();
        }




        // myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }


}

#endif


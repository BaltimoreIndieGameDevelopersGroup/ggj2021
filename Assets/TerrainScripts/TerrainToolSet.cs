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
        return terrainSettings.Where(t => t.Size == currentSize).ToList().Random();

    }
}

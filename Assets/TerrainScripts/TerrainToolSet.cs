using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;


public class TerrainToolSet : MonoBehaviour
{

    public List<TerrainSetting> terrainSettings;
    public List<Vector3Int> chunkPos;
    public SizeSetting currentSize;
    public List<Chunk> chunkCache;




    public TerrainSetting GetTerrainSetting()
    {


        if (currentSize == SizeSetting.Random)
        {

            return terrainSettings.Random();
        }
        return terrainSettings.Where(t => t.Size == currentSize).ToList().Random();

    }

    internal void addChunkPos(Vector3Int chunkPosition)
    {
        chunkPos.Add(chunkPosition);
    }

    internal void addChunkPos(Chunk newChunk)
    {
        chunkCache.Add(newChunk);
        newChunk.Voxels.ToList().ForEach(v => print(v.position));
    }
}

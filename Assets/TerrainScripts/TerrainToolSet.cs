using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using static OptIn.Voxel.Voxel;

[Serializable]
public class ChunkPart
{
    private Chunk newChunk;
    public List<Vector3Int> positionList;

    public ChunkPart(Chunk newChunk)
    {
        this.newChunk = newChunk;
    }
    public void runIt()
    {

        // newChunk.
        //   positionList = newChunk.Voxels.ToList().Where(v => v.data == VoxelType.Grass).Select(v => v.position).ToList();

    }
}
public class TerrainToolSet : MonoBehaviour
{

    public List<TerrainSetting> terrainSettings;
    public List<Vector3Int> chunkPos;
    public SizeSetting currentSize;
    //    public List<Chunk> chunkCache;

    public List<ChunkPart> chunkParts;

    public TerrainSetting currentTerrainSetting;

    public TerrainSetting GetTerrainSetting()
    {


        if (currentSize == SizeSetting.Random)
        {

            currentTerrainSetting = terrainSettings.Random();
            return currentTerrainSetting;
        }
        currentTerrainSetting = terrainSettings.Where(t => t.Size == currentSize).ToList().Random();
        return currentTerrainSetting;
    }
    async void Start()
    {

        await Task.Delay(TimeSpan.FromSeconds(5));
        chunkParts.ForEach(part =>
       {
           part.runIt();
       });
    }

    internal void addChunkPos(Chunk newChunk)
    {
        //   chunkCache.Add(newChunk);

        //  chunkParts.Add(new ChunkPart(newChunk));
    }
}

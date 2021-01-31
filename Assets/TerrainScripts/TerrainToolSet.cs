using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using static OptIn.Voxel.Voxel;
using OptIn.Voxel;

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

    public Vector3 spawnOffset = new Vector3(0.5f, 80f, 0.5f);
    public Chunk firstChunk;
    public List<GameObject> gameBuildings;
    public List<GameObject> spawnedObjects;

    public int xSpawnLimit = 2;
    public int zSpawnLimit = 2;
    public float minBuildingSeperation = 5;
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


        placeObject();
    }
    public void placeObject()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) return; // Don't build in title scene.

        gameBuildings.ForEach(building =>
        {
            Vector3 calcLocation = getGoodLocation();

            spawnedObjects.Add(Instantiate(building, calcLocation, new Quaternion()));
        });
    }

    private Vector3 getGoodLocation()
    {
        int x = UnityEngine.Random.Range(xSpawnLimit, currentTerrainSetting.chunkSize.x - xSpawnLimit);
        int z = UnityEngine.Random.Range(zSpawnLimit, currentTerrainSetting.chunkSize.z - zSpawnLimit);
        Vector3 calcLocation = spawnOffset + new Vector3(x, 0, z);

        if (spawnedObjects.Count == 0)
        {

            return calcLocation;
        }
        float distOfCloset = spawnedObjects.Min(spawnedOb =>
        {

            Vector3 offset = calcLocation - spawnedOb.transform.position;
            float sqrLen = offset.sqrMagnitude;
            // Debug.Log(sqrLen);
            return sqrLen;


        });

        if (distOfCloset < minBuildingSeperation)
        {
            return getGoodLocation();
        }



        return calcLocation;
    }

    void setVoxel()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << LayerMask.NameToLayer("Voxel")))
        {
            var worldPosition = hit.point + ray.direction * 0.01f;
            Voxel voxel;
            if (TerrainGenerator.Instance.GetVoxel(worldPosition, out voxel))
            {
            }

        }
    }
    internal void addChunkPos(Chunk newChunk)
    {
        if (firstChunk == null)
        {
            firstChunk = (newChunk);
        }
        //  chunkParts.Add(new ChunkPart(newChunk));
    }
}

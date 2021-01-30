namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using OptIn.Voxel;

    public class FenceBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject fencePrefab;

        private void Start()
        {
            Chunk chunk;
            if (!TerrainGenerator.Instance.GetChunk(Vector3.zero, out chunk))
            {
                Debug.LogError("Can't get terrain to place fence!");
                return;
            }
        }
    }
}

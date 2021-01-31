namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using OptIn.Voxel;

    public class FenceBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject fencePrefab;
        [SerializeField] private GameObject exit;
        [SerializeField] private GameObject aliensPrefab;
        [SerializeField] private GameObject guardPostPrefab;

        private const int unitsOutsideFence = 3;

        private void Start()
        {
            Chunk chunk;
            if (!TerrainGenerator.Instance.GetChunk(Vector3.zero, out chunk))
            {
                Debug.LogError("Can't get terrain to place fence!");
                return;
            }

            var position = chunk.ChunkPosition;
            var size = chunk.ChunkSize;

            // Build fence in top and bottom z:
            for (int x = 0; x < size.x - unitsOutsideFence; x++)
            {
                if (x % 2 == 0) // (fence is 2 units wide, so only instantiate even x's)
                {
                    Instantiate(fencePrefab, new Vector3(x + 0.5f, 0, 0), Quaternion.identity);
                    Instantiate(fencePrefab, new Vector3(x + 0.5f, 0, size.z), Quaternion.identity);
                }
            }

            // Build fence in left and right x, with gap in center of right x:
            for (int z = 0; z <= size.z; z++)
            {
                if (z % 2 == 0)
                {
                    Instantiate(fencePrefab, new Vector3(0, 0, z), Quaternion.Euler(0, 90, 0));
                    if ((z < (size.z / 2) || (z > ((size.z / 2) + 1))))
                    {
                        Instantiate(fencePrefab, new Vector3(size.x - unitsOutsideFence + 0.25f, 0, z + 0.5f), Quaternion.Euler(0, 90, 0));
                    }
                }
            }

            // Place exit at gap in fence:
            Instantiate(exit, new Vector3(size.x - unitsOutsideFence + 0.75f, 0, size.z / 2), Quaternion.identity);

            // Place aliens at corners:
            Instantiate(aliensPrefab, new Vector3(3, 100, size.z - 4), Quaternion.Euler(0, 135, 0));
            Instantiate(aliensPrefab, new Vector3(size.x - 3 - unitsOutsideFence, 100, size.z - 4), Quaternion.Euler(0, 135, 0));

            // Place guard posts:
            Instantiate(guardPostPrefab, new Vector3(0, 88.5f, size.z), Quaternion.Euler(0, 135, 0));
            Instantiate(guardPostPrefab, new Vector3(size.x - 3, 86.5f, size.z), Quaternion.Euler(0, -135, 0));
            Instantiate(guardPostPrefab, new Vector3(size.x - 3, 86.5f, 0), Quaternion.Euler(0, -45, 0));
        }
    }
}

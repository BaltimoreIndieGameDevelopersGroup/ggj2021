namespace Game
{
    using System.Collections;
    using UnityEngine;
    using OptIn.Voxel;

    public class Dynamite : MonoBehaviour
    {
        [SerializeField] private float fuseDuration = 3;
        [SerializeField] private Transform flame;
        [SerializeField] private Light flameLight;
        [SerializeField] private GameObject explosionPrefab;

        private IEnumerator Start()
        {
            // Flicker the fuse light until it's time to explode:
            var finishTime = Time.time + fuseDuration;
            float direction = -1;
            while (Time.time < finishTime)
            {
                if (Random.value < 0.1f)
                {
                    direction *= -1;
                }
                else if (flame.localScale.x < 0.2f)
                {
                    direction = 1;
                }
                else if (flame.localScale.x > 0.5f)
                {
                    direction = -1;
                }
                flame.localScale += direction * Vector3.one * Time.deltaTime;
                flameLight.range = 4 * flame.localScale.x;
                yield return null;
            }

            // Explode:
            //[TODO] Screen shake?

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        var worldPosition = transform.position + new Vector3(x, y, z); ;
                        TerrainGenerator.Instance.SetVoxel(worldPosition, Voxel.VoxelType.Air);
                    }
                }                
            }

            Destroy(gameObject);
        }
    }
}

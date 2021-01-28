namespace Game
{
    using UnityEngine;

    public class DevBuildTestLevel : MonoBehaviour
    {
        [SerializeField] private Vector2Int size = new Vector2Int(40, 40);
        [SerializeField] private int wallHeight = 5;

        [SerializeField] private GameObject dirtPrefab;
        [SerializeField] private GameObject rockPrefab;
        [SerializeField] private GameObject steelPrefab;

        private void Start()
        {
            var min = -size / 2;
            var max = size / 2;
            for (int x = min.x; x <= max.x; x++)
            {
                for (int z = min.y; z <= max.y; z++)
                {
                    var isEdge = (x == min.x) || (x == max.y) || (z == min.y) || (z == max.y);
                    var prefab = isEdge ? steelPrefab : dirtPrefab;
                    Instantiate(prefab, new Vector3(x, -0.5f, z), Quaternion.identity);
                    Instantiate(prefab, new Vector3(x, -1.5f, z), Quaternion.identity);
                    Instantiate(steelPrefab, new Vector3(x, -2.5f, z), Quaternion.identity);
                    if (isEdge)
                    {
                        for (int i = 0; i < wallHeight; i++)
                        {
                            Instantiate(prefab, new Vector3(x, i + 0.5f, z), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }
}

namespace Game
{
    using UnityEngine;

    public class Building : MonoBehaviour
    {
        [SerializeField] private Pickup[] potentialSecrets;
        [SerializeField] private Pickup[] potentialTools;

        private void Start()
        {
            if (potentialSecrets.Length > 0)
            {
                InstantiatePickup(-1, -1, potentialSecrets[Random.Range(0, potentialSecrets.Length)]);
                if (Random.value > 0.5f)
                {
                    InstantiatePickup(1, 1, potentialSecrets[Random.Range(0, potentialSecrets.Length)]);
                }
            }

            if (potentialTools.Length > 0)
            {
                InstantiatePickup(-1, 1, potentialTools[Random.Range(0, potentialTools.Length)]);
                if (Random.value > 0.5f)
                {
                    InstantiatePickup(1, -1, potentialTools[Random.Range(0, potentialTools.Length)]);
                }
            }
        }

        private void InstantiatePickup(float x, float z, Pickup prefab)
        {
            Instantiate(prefab, transform.position + new Vector3(x, 0, z), Quaternion.identity);

        }

    }
}

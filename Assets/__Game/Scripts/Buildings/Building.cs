namespace Game
{
    using UnityEngine;

    public class Building : MonoBehaviour
    {
        [SerializeField] private Pickup plans;
        [SerializeField] private Pickup[] potentialPickups;

        private static bool hasInstantiatedPlans = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            hasInstantiatedPlans = false;
        }

        private void Start()
        {
            var prefab = !hasInstantiatedPlans ? plans : potentialPickups[Random.Range(0, potentialPickups.Length)];
            Instantiate(prefab, transform.position + new Vector3(-1f, 0, -1f), Quaternion.identity);
            prefab = potentialPickups[Random.Range(0, potentialPickups.Length)];
            Instantiate(prefab, transform.position + new Vector3(1f, 0, 1f), Quaternion.identity);
            hasInstantiatedPlans = true;
        }

    }
}

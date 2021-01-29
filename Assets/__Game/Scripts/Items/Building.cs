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
            if (!hasInstantiatedPlans)
            {
                hasInstantiatedPlans = true;
                Instantiate(plans, transform);
            }
            else
            {
                var prefab = potentialPickups[Random.Range(0, potentialPickups.Length)];
                Instantiate(prefab, transform);
            }
        }

    }
}

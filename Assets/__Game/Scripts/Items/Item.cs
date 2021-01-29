namespace Game
{
    using UnityEngine;

    [CreateAssetMenu]
    public class Item : ScriptableObject, IItem
    {
        [SerializeField] private string pickupMessage;

        public string PickupMessage { get { return pickupMessage; } }
    }
}

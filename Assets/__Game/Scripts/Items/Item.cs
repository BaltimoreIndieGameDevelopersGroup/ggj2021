namespace Game
{
    using UnityEngine;

    [CreateAssetMenu]
    public class Item : ScriptableObject, IItem
    {
        [TextArea] [SerializeField] private string pickupMessage;
        [SerializeField] private bool isASecretItem;

        public string PickupMessage { get { return pickupMessage; } }
        public bool IsASecretItem { get { return isASecretItem; } }

    }
}

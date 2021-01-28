namespace Game
{
    using UnityEngine;

    public class Pickup : MonoBehaviour, IPickup
    {
        [SerializeField] private Item item;

        public void PickUp()
        {
            ServiceLocator.Get<IInventory>().AddItem(item);
            ServiceLocator.Get<IMessageUI>().ShowMessage(item.name);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PickUp();
            }
        }
    }
}

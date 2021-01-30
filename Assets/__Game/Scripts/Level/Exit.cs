namespace Game
{
    using UnityEngine;

    public class Exit : MonoBehaviour
    {
        [SerializeField] private Item plans;
        [SerializeField] private GameObject winScreen;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (ServiceLocator.Get<IInventory>().HasItem(plans))
                {
                    ServiceLocator.Get<IPlayerController>().AllowMovement = false;
                    Instantiate(winScreen);
                }
                else
                {
                    ServiceLocator.Get<IMessageUI>().ShowMessage("You can't escape without the plans!");
                }
            }
        }
    }
}

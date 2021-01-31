namespace Game
{
    using UnityEngine;

    public class Exit : MonoBehaviour
    {
        [SerializeField] private GameObject winScreen;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (ServiceLocator.Get<IInventory>().HasASecretItem())
                {
                    ServiceLocator.Get<IPlayerController>().AllowMovement = false;
                    Instantiate(winScreen);
                }
                else
                {
                    ServiceLocator.Get<IMessageUI>().ShowMessage("You can't escape without at least one secret item!");
                }
            }
        }
    }
}

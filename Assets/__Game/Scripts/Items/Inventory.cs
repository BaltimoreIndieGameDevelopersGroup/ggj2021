namespace Game
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Inventory : MonoBehaviour, IInventory
    {
        private List<IItem> items = new List<IItem>();

        private int score = 0;

        public int Score { get { return score; } }

        private void OnEnable()
        {
            ServiceLocator.Register<IInventory>(this);
        }

        private void Start()
        {
            ServiceLocator.Get<IMessageUI>().SetScore(score);
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
            if (item != null && item.IsASecretItem)
            {
                score++;
                ServiceLocator.Get<IMessageUI>().SetScore(score);
            }
        }

        public void RemoveItem(IItem item)
        {
            items.Remove(item);
        }

        public bool HasItem(IItem item)
        {
            return items.Contains(item);
        }

        public bool HasASecretItem()
        {
            return items.Find(x => x.IsASecretItem) != null;
        }
    }
}

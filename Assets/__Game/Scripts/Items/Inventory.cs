namespace Game
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Inventory : MonoBehaviour, IInventory
    {
        private List<IItem> items = new List<IItem>();

        private void OnEnable()
        {
            ServiceLocator.Register<IInventory>(this);
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            items.Remove(item);
        }

        public bool HasItem(IItem item)
        {
            return items.Contains(item);
        }
    }
}

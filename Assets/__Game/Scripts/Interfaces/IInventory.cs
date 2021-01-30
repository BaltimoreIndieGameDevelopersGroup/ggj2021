namespace Game
{

    /// <summary>
    /// Interface for the player's inventory.
    /// </summary>
    public interface IInventory
    {

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        void AddItem(IItem item);

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        void RemoveItem(IItem item);

        /// <summary>
        /// Checks if the inventory has at least one of the specified item.
        /// </summary>
        bool HasItem(IItem item);

        /// <summary>
        /// Checks if the inventory has at least one secret.
        /// </summary>
        /// <returns></returns>
        bool HasASecretItem();

    }
}
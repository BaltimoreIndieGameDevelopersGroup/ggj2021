namespace Game
{
    /// <summary>
    /// Interface for items that can be picked up.
    /// </summary>
    public interface IPickup
    {
        /// <summary>
        /// Remove the pickup from the world and add an item to the player's inventory.
        /// </summary>
        void PickUp();
    }
}

namespace Game
{
    /// <summary>
    /// Interface for the player controller.
    /// </summary>
    public interface IPlayerController
    {

        /// <summary>
        /// Called when a guard detects the player.
        /// </summary>
        void DetectedByGuard();

        /// <summary>
        /// Called when the player dies, such as blowing itself up with dynamite.
        /// </summary>
        void Die();

        bool AllowMovement { get; set; }
    }
}

namespace Game
{
    public interface IVoxel
    {
        Surface Surface { get; }

        /// <summary>
        /// Called when voxel is destroyed (e.g., dug or blown up).
        /// </summary>
        void Disintegrate();
    }
}

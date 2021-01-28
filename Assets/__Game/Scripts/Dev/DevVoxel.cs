namespace Game
{
    using UnityEngine;

    /// <summary>
    /// Temporary development implementation of IVoxel that other modules can
    /// use while the actual voxel implementation is being written.
    /// </summary>
    public class DevVoxel : MonoBehaviour, IVoxel
    {
        [SerializeField] private Surface surface;

        public Surface Surface { get { return surface; } }

        public void Disintegrate()
        {
            Destroy(gameObject);
        }
    }
}

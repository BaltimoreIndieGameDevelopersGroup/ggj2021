namespace Game
{
    using UnityEngine;

    public class DevVoxel : MonoBehaviour, IVoxel
    {
        [SerializeField] private Surface surface;

        public Surface Surface { get { return surface; } }
    }
}

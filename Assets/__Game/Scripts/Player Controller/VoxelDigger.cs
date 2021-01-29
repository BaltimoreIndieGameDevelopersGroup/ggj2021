namespace Game
{
    using OptIn.Voxel;
    using UnityEngine;

    public class VoxelDigger : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << LayerMask.NameToLayer("Voxel")))
                {
                    TerrainGenerator.Instance.SetVoxel(hit.point + ray.direction * 0.01f, Voxel.VoxelType.Air);
                }
            }
        }
    }
}

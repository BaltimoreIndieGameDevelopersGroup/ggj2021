namespace Game
{
    using UnityEngine;

    public class DigController : MonoBehaviour
    {
        private bool digging = false;

        private float raycastDistance = 1.5f;

        private void Update()
        {
            if (!digging && Input.GetButtonDown("Fire1"))
            {
                TryDigging();
            }
        }

        private void TryDigging()
        {
            var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, raycastDistance, LayerMask.GetMask("Voxel")))
            {
                var voxel = hitInfo.collider.GetComponent(typeof(IVoxel)) as IVoxel;
                if (voxel != null)
                {
                    if (voxel.Surface == Surface.Dirt)
                    {
                        Destroy(hitInfo.collider.gameObject);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * raycastDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(ray);
            Gizmos.DrawWireSphere(ray.origin + ray.direction, 0.2f);
        }
    }
}

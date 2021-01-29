namespace Game
{
    using System.Collections;
    using UnityEngine;
    using OptIn.Voxel;

    public class VoxelDigger : MonoBehaviour
    {
        [SerializeField] private FMODUnity.StudioEventEmitter digSoundEmitter;

        private ParticleSystem dirtParticles;
        private Canvas swipeCanvas;

        private void Awake()
        {
            dirtParticles = GetComponentInChildren<ParticleSystem>();
            swipeCanvas = GetComponentInChildren<Canvas>();
            swipeCanvas.enabled = false;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TryDig();
            }
            CheckOverhead();
        }

        /// <summary>
        /// If there is air overhead, play danger music; otherwise play safe music.
        /// Note: This method still considers it danger if you're in a tall underground tunnel.
        /// </summary>
        private void CheckOverhead()
        {
            var worldPosition = transform.position + 1.5f * Vector3.up;
            Voxel voxel;
            if (TerrainGenerator.Instance.GetVoxel(worldPosition, out voxel))
            {
                if (voxel.data == Voxel.VoxelType.Air)
                {
                    ServiceLocator.Get<IAudioManager>().PlayDangerMusic();
                }
                else
                {
                    ServiceLocator.Get<IAudioManager>().PlaySafeMusic();
                }
            }
        }

        private void TryDig()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << LayerMask.NameToLayer("Voxel")))
            {
                var worldPosition = hit.point + ray.direction * 0.01f;
                Voxel voxel;
                if (TerrainGenerator.Instance.GetVoxel(worldPosition, out voxel))
                {
                    if (voxel.data == Voxel.VoxelType.Stone)
                    {
                        Debug.Log("Stone!"); //[TODO]
                    }
                    else
                    {
                        TerrainGenerator.Instance.SetVoxel(worldPosition, Voxel.VoxelType.Air);
                        StartCoroutine(PlayDigEffect());
                    }
                }
            }
        }

        private IEnumerator PlayDigEffect()
        {
            //digSoundEmitter.Play();
            if (dirtParticles != null)
            {
                dirtParticles.Play();
            }
            swipeCanvas.enabled = true;
            yield return new WaitForSeconds(0.1f);
            swipeCanvas.enabled = false;
            yield return new WaitForSeconds(0.4f);
            if (dirtParticles != null)
            {
                dirtParticles.Stop();
            }
        }
    }
}

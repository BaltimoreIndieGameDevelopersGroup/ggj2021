namespace Game
{
    using System;
    using System.Collections;
    using UnityEngine;
    using OptIn.Voxel;

    [Serializable]
    public class DiggingToolInfo
    {
        [SerializeField] private Item item;
        [SerializeField] private bool penetratesRock;
        [SerializeField] private float digDuration = 1;

        public Item Item { get { return item; } }
        public bool PenetratesRock { get { return penetratesRock; } }
        public float DigDuration { get { return digDuration; } }
    }

    public class VoxelDigger : MonoBehaviour
    {
        [Tooltip("List in priority order where best tool is first. Last tool should be bare hands.")]
        [SerializeField] private DiggingToolInfo[] prioritizedDiggingTools;
        [SerializeField] private Item dynamite;
        [SerializeField] private GameObject litDynamite;

        [SerializeField] private FMODUnity.StudioEventEmitter digSoundEmitter;

        private const float ShowSwipeDuration = 0.1f;
        private const float HideSwipeDuration = 0.4f;
        private WaitForSeconds waitShowSwipe = new WaitForSeconds(ShowSwipeDuration);
        private WaitForSeconds waitHideSwipe = new WaitForSeconds(HideSwipeDuration);
        private Canvas swipeCanvas;
        private ParticleSystem dirtParticles;
        private bool isDigging = false;

        private void Awake()
        {
            dirtParticles = GetComponentInChildren<ParticleSystem>();
            swipeCanvas = GetComponentInChildren<Canvas>();
            swipeCanvas.enabled = false;
        }

        private void Update()
        {
            CheckOverhead();
            if (Input.GetButtonDown("Fire1") && !isDigging)
            {
                TryDig();
            }
            else if (Input.GetButtonDown("Fire3"))
            {
                TryDynamite();
            }
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
                    // Identify best tool:
                    DiggingToolInfo tool = null;
                    for (int i = 0; i < prioritizedDiggingTools.Length; i++)
                    {
                        var toolToCheck = prioritizedDiggingTools[i];
                        if (toolToCheck.Item == null || ServiceLocator.Get<IInventory>().HasItem(toolToCheck.Item))
                        {
                            tool = toolToCheck;
                            break;
                        }
                    }
                    if (tool == null) return;

                    // Check if we can dig through the voxel type:
                    var canPenetrate = (voxel.data != Voxel.VoxelType.Stone) || tool.PenetratesRock;
                    if (canPenetrate)
                    {
                        StartCoroutine(Dig(tool, worldPosition));
                    }
                }
            }
        }

        private IEnumerator Dig(DiggingToolInfo tool, Vector3 worldPosition)
        {
            if (tool == null) yield break;

            isDigging = true;

            //digSoundEmitter.Play();

            if (dirtParticles != null)
            {
                dirtParticles.Play();
            }

            float finishTime = Time.time + tool.DigDuration;
            while (Time.time < finishTime)
            {
                swipeCanvas.enabled = true;
                yield return waitShowSwipe;
                swipeCanvas.enabled = false;
                if (Time.time + HideSwipeDuration < finishTime)
                {
                    yield return waitHideSwipe;
                }
                else
                {
                    yield return new WaitForSeconds(finishTime - Time.time);
                }
            }
            TerrainGenerator.Instance.SetVoxel(worldPosition, Voxel.VoxelType.Air);
            yield return new WaitForSeconds(0.25f);
            if (dirtParticles != null)
            {
                dirtParticles.Stop();
            }
            isDigging = false;
        }

        private void TryDynamite()
        {
            if (ServiceLocator.Get<IInventory>().HasItem(dynamite))
            {
                ServiceLocator.Get<IInventory>().RemoveItem(dynamite);
                Instantiate(litDynamite, transform.position + transform.forward, Quaternion.identity);
            }
        }
    }
}

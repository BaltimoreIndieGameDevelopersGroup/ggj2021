namespace Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Random = UnityEngine.Random;

    [Serializable]
    public class Banter
    {
        [SerializeField] private string[] lines;

        public string[] Lines { get { return lines; } }
    }

    public class Aliens : MonoBehaviour
    {
        [SerializeField] private Banter[] banter;

        Rigidbody rb;

        [SerializeField] private Transform[] aliens;

        public float maxMoveForce;
        public float averageMoveTimeInterval;
        public float timeIntervalVariance;

        float lastMoveTime;
        float currentMoveTimeInterval;

        private const float HoverHeight = 8;
        float targetHeight;
        Vector3 tempPosition;
        Vector3 sceneCenter;

        DevSpotLightControl[] spotlightcontrol;
        Quaternion[] spotlightTargetRot;
        float lightMoveSpan;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            targetHeight = transform.position.y;
            sceneCenter = new Vector3(32, targetHeight, 32);
            spotlightcontrol = GetComponentsInChildren<DevSpotLightControl>();
            spotlightTargetRot = new Quaternion[spotlightcontrol.Length];
            for (int i = 0; i < spotlightcontrol.Length; i++)
            {
                spotlightTargetRot[i] = spotlightcontrol[i].transform.rotation;
            }
        }

        private void Update()
        {
            AdjustHoverHeight();
            RandomHover();
        }

        void AdjustHoverHeight()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
            {
                targetHeight = hitInfo.point.y + HoverHeight;
                //targetHeight = hitInfo.point + Vector3.up * HoverHeight;
            }
            tempPosition = transform.position;
            tempPosition.y = Mathf.Lerp(tempPosition.y, targetHeight, Time.deltaTime);
            transform.position = tempPosition;
        }

        void RandomHover()
        {
            sceneCenter.y = transform.position.y;
            Vector3 targetDirection = sceneCenter - transform.position;
            targetDirection.y = 0;

            if (lastMoveTime + currentMoveTimeInterval < Time.time || targetDirection.magnitude > 35)
            {
                lastMoveTime = Time.time;
                currentMoveTimeInterval = averageMoveTimeInterval + timeIntervalVariance * (Random.value - 0.5f);
                lightMoveSpan = currentMoveTimeInterval;
                float moveForce = Random.value * maxMoveForce;
                //Check boundaries
                int directionMinRange = 0;
                int directionMaxRange = 359;
                if (targetDirection.magnitude > 35)
                {
                    directionMinRange = -45;
                    directionMaxRange = 45;
                    moveForce = maxMoveForce;
                }
                else
                {
                    targetDirection = Vector3.forward;
                }
                Vector3 moveDir = Quaternion.Euler(0, Random.Range(directionMinRange, directionMaxRange), 0) * targetDirection.normalized;
                rb.AddForce(moveDir * moveForce, ForceMode.Acceleration);

                MoveSpotLight();
            }
            if (rb.velocity.magnitude >= 0.0001f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), (1 - (lightMoveSpan / currentMoveTimeInterval)) * 1.5f);
            }
            for (int i = 0; i < spotlightcontrol.Length; i++)
            {
                spotlightcontrol[i].transform.localRotation = Quaternion.Lerp(spotlightcontrol[i].transform.localRotation, spotlightTargetRot[i], 1 - (lightMoveSpan / currentMoveTimeInterval));
            }
            lightMoveSpan -= Time.deltaTime;
            /*
            Quaternion alienOrigRot = aliens[0].rotation;
            Vector3 localEuler = aliens[0].rotation.eulerAngles;
            alienOrigRot.SetLookRotation(rb.velocity.normalized);
            float originalY = alienOrigRot.eulerAngles.y;

            localEuler.x = 30 * Mathf.Clamp(rb.velocity.magnitude, 0, 5) / 5f;
            localEuler.y = originalY;
            aliens[0].rotation = alienOrigRot;
            */
        }

        void MoveSpotLight()
        {
            for (int i = 0; i < spotlightTargetRot.Length; i++)
            {
                spotlightTargetRot[i].eulerAngles = new Vector3(Random.Range(-45f, 0), 0, Random.Range(-30f, 30f));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ServiceLocator.Get<IMessageUI>().PlayBanter(banter[UnityEngine.Random.Range(0, banter.Length)].Lines);
            }
        }
    }
}

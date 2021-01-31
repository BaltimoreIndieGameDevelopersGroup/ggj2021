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
            spotlightcontrol = new DevSpotLightControl[aliens.Length];
            spotlightTargetRot = new Quaternion[aliens.Length];
            for (int i = 0; i < aliens.Length; i++)
            {
                spotlightcontrol[i] = aliens[i].GetComponentInChildren<DevSpotLightControl>();
                spotlightTargetRot[i] = aliens[i].rotation;
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
            float timeInc = 1 - (lightMoveSpan / currentMoveTimeInterval);
            if (rb.velocity.magnitude >= 0.0001f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), Time.deltaTime);
            }
            for (int i = 0; i < spotlightcontrol.Length; i++)
            {
                spotlightcontrol[i].transform.localRotation = Quaternion.Lerp(spotlightcontrol[i].transform.localRotation, spotlightTargetRot[i], Time.deltaTime * 5);
            }
            lightMoveSpan -= Time.deltaTime;

            Vector3 alienEuler = Quaternion.LookRotation(rb.velocity.normalized).eulerAngles;
            float angleDiff = transform.rotation.eulerAngles.y - alienEuler.y;
            alienEuler.x = Mathf.Lerp(alienEuler.x, 30 * Mathf.Clamp(rb.velocity.magnitude, 0, 5) / 5f, Time.deltaTime * 5);

            for (int i = 0; i < aliens.Length; i++)
            {
                aliens[i].rotation = Quaternion.Euler(alienEuler);
                aliens[i].RotateAround(aliens[i].position, aliens[i].up, angleDiff);
            }
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

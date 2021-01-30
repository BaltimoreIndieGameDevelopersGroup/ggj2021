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

        public float maxMoveForce;
        public float averageMoveTimeInterval;
        public float timeIntervalVariance;

        float lastMoveTime;
        float currentMoveTimeInterval;

        private const float HoverHeight = 8;
        float targetHeight;
        Vector3 tempPosition;
        Vector3 sceneCenter;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            targetHeight = transform.position.y;
            sceneCenter = new Vector3(32, targetHeight, 32);
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
            
            if (lastMoveTime + currentMoveTimeInterval < Time.time)
            {
                Debug.Log("targetDirection = " + targetDirection.magnitude);
                lastMoveTime = Time.time;
                currentMoveTimeInterval = averageMoveTimeInterval + timeIntervalVariance * (Random.value - 0.5f);

                float moveForce = Random.value * maxMoveForce;
                //Check boundaries
                int directionMinRange = 0;
                int directionMaxRange = 359;
                if (targetDirection.magnitude > 35)
                {
                    directionMinRange = -45;
                    directionMaxRange = 45;
                }
                else
                {
                    targetDirection = Vector3.forward;
                }
                Vector3 moveDir = Quaternion.Euler(0, Random.Range(directionMinRange, directionMaxRange), 0) * targetDirection.normalized;
                rb.AddForce(moveDir * moveForce, ForceMode.Acceleration);
            }
            if (rb.velocity.magnitude >= 0.0001f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), Time.deltaTime * 2.5f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GuardPost : MonoBehaviour
    {
        public float moveSpeed;
        public DevSpotLightControl spotLightControl;
        // Start is called before the first frame update
        void Start()
        {
            targetAngle = Vector3.zero;
            targetBeamAngle = 25;
        }
        float elapsedTime;
        Vector3 targetAngle;
        float targetBeamAngle;
        // Update is called once per frame
        void Update()
        {
            if (elapsedTime >= moveSpeed)
            {
                //pitch
                targetAngle.x = Random.Range(0, -70f);
                //targetAngle.x = -60 * Mathf.PerlinNoise(Time.time, Time.time * 0.5f);
                //yaw
                targetAngle.y = Random.Range(-50, 50);
                //targetAngle.y = 90 * Mathf.PerlinNoise(Time.time * 0.5f, -Time.time) - 45;
                elapsedTime = 0;

                targetBeamAngle = 10 + 15 * (targetAngle.x / 70f + 1);
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetAngle), elapsedTime / moveSpeed);
            spotLightControl.beamAngle = Mathf.Lerp(spotLightControl.beamAngle, targetBeamAngle, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
        }
    }
}
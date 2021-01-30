using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DevSpotLightControl : MonoBehaviour
    {
        public SpotlightDetector spotlight;
        public bool useObjectTransform;
        public Vector3 lightPosition;
        public Vector3 lightRotation;
        [Range(5, 45)]
        public float beamAngle;
        public float maxBeamDistance;
        public float tickMarkInterval;
        [Range(50, 500, order = 20)]
        public float detectionIntervalInMilli;
        void Update()
        {
            //call for spotlight detection
            if (useObjectTransform)
            {
                spotlight.SpotlightSearch(transform.position, transform.rotation.eulerAngles, beamAngle, tickMarkInterval);
            }
            else
            {
                spotlight.SpotlightSearch(lightPosition, lightRotation, beamAngle, tickMarkInterval);
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpotlightDetector : MonoBehaviour
    {
        LayerMask surfaceLayerMask;
        void Awake()
        {
            surfaceLayerMask = LayerMask.GetMask("Player", "Surface");
        }

        public bool SpotlightSearch(Vector3 lightPos, float pitch, float yaw, float beamAngle, float tickInterval, float maxSearchDistance = 100)
        {
            return SpotlightSearch(lightPos, new Vector3(pitch, yaw, 0), beamAngle, tickInterval, maxSearchDistance);
        }

        public bool SpotlightSearch(Vector3 lightPos, Vector3 rot, float beamAngle, float tickInterval, float maxSearchDistance = 100)
        {
            IPlayerController pc = ServiceLocator.Get<IPlayerController>();
            float halfAngle = beamAngle / 2f;
            Vector3 centerDirection = Quaternion.Euler(rot) * Vector3.down;
            RaycastHit hit;
            if (Physics.Raycast(lightPos, centerDirection, out hit, maxSearchDistance, surfaceLayerMask))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    pc.DetectedByGuard();
                    return true;
                }

                Quaternion yawRot = Quaternion.Euler(0, rot.y, 0);

                //get the distance to the ground from the center beam
                float centerBeamDistance = hit.distance;

                float lightHeight = lightPos.y - hit.point.y;
                Vector3 adjustPos = lightPos;
                adjustPos.y = hit.point.y;

                Vector3 lightPostBasePos = lightPos;
                lightPostBasePos.y = hit.point.y;
                Vector3 relativeDirection = hit.point - lightPostBasePos;
                float horizontalDistanceToHitPoint = relativeDirection.magnitude;


                //calculate the projected radius on the ground.
                float centerRadius = Mathf.Tan(Mathf.Deg2Rad * halfAngle) * centerBeamDistance;

                float horizontalDistacneToFrontBeamHitPoint = Mathf.Tan(Mathf.Deg2Rad * (rot.x + halfAngle)) * lightHeight;
                float majorRadius = horizontalDistacneToFrontBeamHitPoint - horizontalDistanceToHitPoint;

                float horizontalDistacneToRearBeamHitPoint = Mathf.Tan(Mathf.Deg2Rad * (rot.x - halfAngle)) * lightHeight;
                float minorRadius = horizontalDistacneToRearBeamHitPoint - horizontalDistanceToHitPoint;

                //Debug.DrawLine(lightPos, (centerDirection * centerBeamDistance) + lightPos, Color.blue);
                //Debug.DrawLine(lightPos, yawRot * ((Vector3.back * horizontalDistacneToFrontBeamHitPoint) + lightPostBasePos), Color.red);
                //Debug.DrawLine(lightPos, yawRot * ((Vector3.back * horizontalDistacneToRearBeamHitPoint) + lightPostBasePos), Color.red);

                int majorCount = (int)(majorRadius / tickInterval);
                int minorCount = (int)(minorRadius / tickInterval);
                int innerTickCount = majorCount + Mathf.Abs(minorCount) + 1;
                int[] centerTicks = new int[innerTickCount];
                for (int i = innerTickCount - 1; i >= 0; i--)
                {
                    centerTicks[i] = i + minorCount; ;
                }


                //add the fore most target pos
                List<Vector3> castTargetPos = new List<Vector3>();
                Vector3 frontTickPos = (Vector3.back * horizontalDistacneToFrontBeamHitPoint);// + lightPostBasePos;
                castTargetPos.Add(frontTickPos);
                //fill in the target positions
                foreach (var centerTickMark in centerTicks)
                {
                    float horizontalDistanceToTickMark = horizontalDistanceToHitPoint + centerTickMark * tickInterval;
                    float farRight = centerRadius;
                    if (centerTickMark != 0)
                    {
                        float tickMarkAngleRadius = Mathf.Atan2(horizontalDistanceToTickMark, lightHeight);
                        float tickAngle = tickMarkAngleRadius - Mathf.Deg2Rad * rot.x;
                        float h = Mathf.Sin(tickAngle) * centerBeamDistance;
                        float hSq = h * h;
                        float b = Mathf.Sqrt(Mathf.Abs(centerRadius * centerRadius - hSq));
                        float middleDistance = Mathf.Sqrt(Mathf.Abs(centerBeamDistance * centerBeamDistance + hSq));
                        float tickHitDistance = Mathf.Sqrt(Mathf.Abs(horizontalDistanceToTickMark * horizontalDistanceToTickMark + lightHeight * lightHeight));

                        farRight = b * tickHitDistance / middleDistance;
                    }
                    int rightCount = (int)(farRight / tickInterval);

                    Vector3 centerTickPos = Vector3.back * horizontalDistanceToTickMark;
                    castTargetPos.Add(centerTickPos);
                    for (int i = 1; i <= rightCount; i++)
                    {
                        Vector3 tickPos = centerTickPos + Vector3.right * (tickInterval * i);
                        castTargetPos.Add(tickPos);
                        Vector3 leftTickPos = tickPos;
                        leftTickPos.x *= -1;
                        castTargetPos.Add(leftTickPos);
                    }
                    Vector3 farTickPos = centerTickPos + Vector3.right * farRight;
                    castTargetPos.Add(farTickPos);
                    Vector3 farLeftTickPos = farTickPos;
                    farLeftTickPos.x *= -1;
                    castTargetPos.Add(farLeftTickPos);

                }
                //add the rear most target pos
                Vector3 rearTickPos = (Vector3.back * horizontalDistacneToRearBeamHitPoint);// + lightPostBasePos;
                castTargetPos.Add(rearTickPos);

                foreach (var targetPos in castTargetPos)
                {
                    Vector3 castDir = ((yawRot * targetPos) + adjustPos) - lightPos;

                    if (Physics.Raycast(lightPos, castDir, out hit, maxSearchDistance))
                    {
                        Debug.DrawLine(lightPos, hit.point, Color.blue);
                        if (hit.transform.gameObject.tag == "Player")
                        {
                            pc.DetectedByGuard();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
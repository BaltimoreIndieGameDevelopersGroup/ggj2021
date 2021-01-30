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
                Debug.DrawLine(lightPos, hit.point, Color.blue);
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    pc.DetectedByGuard();
                    return true;
                }


                //get the distance to the ground from the center beam
                float centerBeamDistance = hit.distance;
                float centerDeamDistanceSq = centerBeamDistance * centerBeamDistance;
                float lightHeight = lightPos.y - hit.point.y;
                float lightHeightSq = lightHeight * lightHeight;
                Vector3 adjustPos = lightPos;
                adjustPos.y = hit.point.y;

                Vector3 lightPostBasePos = lightPos;
                lightPostBasePos.y = hit.point.y;
                Vector3 relativeDirection = hit.point - lightPostBasePos;
                float horizontalDistanceToHitPoint = relativeDirection.magnitude;
                float rotXRadian = Mathf.Atan2(horizontalDistanceToHitPoint, lightHeight);
                float turnAngle = Vector3.SignedAngle(relativeDirection, Vector3.back, Vector3.up);
                Debug.Log("turnAngle = " + turnAngle);
                Quaternion yawRot = Quaternion.Euler(0, -turnAngle, 0);

                //calculate the projected radius on the ground.
                float centerRadius = Mathf.Tan(Mathf.Deg2Rad * halfAngle) * centerBeamDistance;
                float centerRadiusSq = centerRadius * centerRadius;
                float horizontalDistacneToFrontBeamHitPoint = Mathf.Tan(rotXRadian + Mathf.Deg2Rad * halfAngle) * lightHeight;
                float majorRadius = horizontalDistacneToFrontBeamHitPoint - horizontalDistanceToHitPoint;

                float horizontalDistacneToRearBeamHitPoint = Mathf.Tan(rotXRadian - Mathf.Deg2Rad * halfAngle) * lightHeight;
                float minorRadius = horizontalDistacneToRearBeamHitPoint - horizontalDistanceToHitPoint;

                //Debug.DrawLine(lightPos, (centerDirection * centerBeamDistance) + lightPos, Color.blue);
                Debug.DrawLine(lightPos, relativeDirection + lightPostBasePos, Color.black);
                Debug.DrawLine(lightPos, (yawRot * (Vector3.back * horizontalDistacneToFrontBeamHitPoint) + lightPostBasePos), Color.red);
                Debug.DrawLine(lightPos, (yawRot * (Vector3.back * horizontalDistacneToRearBeamHitPoint) + lightPostBasePos), Color.green);

                int majorCount = (int)(majorRadius / tickInterval);
                int minorCount = (int)(minorRadius / tickInterval);
                int innerTickCount = majorCount + Mathf.Abs(minorCount) + 1;
                int[] centerTicks = new int[innerTickCount];
                for (int i = 0; i < innerTickCount; i++)
                {
                    centerTicks[i] = i + minorCount; ;
                }


                //add the fore most target pos
                List<Vector3> castTargetPos = new List<Vector3>();
                Vector3 frontTickPos = (Vector3.back * horizontalDistacneToFrontBeamHitPoint);
                castTargetPos.Add(frontTickPos);

                //fill in the target positions
                foreach (var centerTickMark in centerTicks)
                {
                    Debug.Log("CenterTickMark = " + centerTickMark);
                    // #2
                    float horizontalDistanceToTickMark = horizontalDistanceToHitPoint + centerTickMark * tickInterval;
                    float horizontalDistanceToTickMarkSq = horizontalDistanceToTickMark * horizontalDistanceToTickMark;
                    Debug.Log("horizontalDistanceToTickMark = " + horizontalDistanceToTickMark);
                    float farRight = centerRadius;
                    if (centerTickMark != 0)
                    {
                        // #3
                        float tickMarkAngleRadian = Mathf.Atan2(horizontalDistanceToTickMark, lightHeight);
                        //Debug.Log("tickMarkAngleRadius = " + tickMarkAngleRadius);
                        // #4
                        float tickAngle = tickMarkAngleRadian - rotXRadian;
                        //Debug.Log("tickAngle = " + tickAngle);
                        // #5
                        //float h = Mathf.Sin(tickAngle) * centerBeamDistance;
                        float h = Mathf.Tan(tickAngle) * centerBeamDistance;
                        float hSq = h * h;
                        Debug.Log("centerRadius = " + centerRadius);
                        Debug.Log("h = " + h);
                        // #6
                        float b = Mathf.Sqrt(Mathf.Abs(centerRadiusSq - hSq));
                        Debug.Log("b = " + b);
                        // #7
                        float middleDistance = Mathf.Sqrt(centerDeamDistanceSq + hSq);
                        // #8
                        float tickHitDistance = Mathf.Sqrt(Mathf.Abs(horizontalDistanceToTickMarkSq + lightHeightSq));
                        // #9
                        farRight = b * tickHitDistance / middleDistance;
                    }
                    Debug.Log("farRight = " + farRight);
                    int rightCount = (int)(farRight / tickInterval);

                    Vector3 centerTickPos = Vector3.back * horizontalDistanceToTickMark;
                    //castTargetPos.Add(centerTickPos);
                    /*for (int i = 1; i <= rightCount; i++)
                    {
                        Vector3 tickPos = centerTickPos + Vector3.right * (tickInterval * i);
                        castTargetPos.Add(tickPos);
                        Vector3 leftTickPos = tickPos;
                        leftTickPos.x *= -1;
                        castTargetPos.Add(leftTickPos);
                    }*/
                    Vector3 farTickPos = centerTickPos + Vector3.right * farRight;
                    castTargetPos.Add(farTickPos);
                    Vector3 farLeftTickPos = farTickPos;
                    farLeftTickPos.x *= -1;
                    castTargetPos.Add(farLeftTickPos);

                }
                //add the rear most target pos
                Vector3 rearTickPos = (Vector3.back * horizontalDistacneToRearBeamHitPoint);
                castTargetPos.Add(rearTickPos);

                foreach (var targetPos in castTargetPos)
                {
                    Vector3 castDir = ((yawRot * targetPos) + adjustPos) - lightPos;

                    if (Physics.Raycast(lightPos, castDir, out hit, maxSearchDistance))
                    {
                        Debug.DrawLine(lightPos, hit.point, Color.yellow);
                        if (hit.transform.gameObject.tag == "Player")
                        {
                            //pc.DetectedByGuard();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
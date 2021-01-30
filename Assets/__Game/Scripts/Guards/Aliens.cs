namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Aliens : MonoBehaviour
    {
        private const float HoverHeight = 4;

        private void Update()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
            {
                transform.position = hitInfo.point + Vector3.up * HoverHeight;
            }
        }


    }
}

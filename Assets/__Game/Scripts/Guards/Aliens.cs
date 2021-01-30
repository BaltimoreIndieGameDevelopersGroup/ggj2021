namespace Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class Banter
    {
        [SerializeField] private string[] lines;

        public string[] Lines { get { return lines; } }
    }

    public class Aliens : MonoBehaviour
    {
        [SerializeField] private Banter[] banter;

        private const float HoverHeight = 4;

        private void Update()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
            {
                transform.position = hitInfo.point + Vector3.up * HoverHeight;
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

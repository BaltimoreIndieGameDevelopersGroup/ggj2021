namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Dynamite : MonoBehaviour
    {
        [SerializeField] private float fuseDuration = 3;
        [SerializeField] private Transform flame;
        [SerializeField] private Light flameLight;

        private IEnumerator Start()
        {
            var finishTime = Time.time + fuseDuration;
            while (Time.time < finishTime)
            {
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}

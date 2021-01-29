namespace Game
{
    using System.Collections;
    using UnityEngine;

    public class TimedDestruction : MonoBehaviour
    {
        [SerializeField] private float duration = 2;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}

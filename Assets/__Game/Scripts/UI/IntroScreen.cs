namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class IntroScreen : MonoBehaviour
    {
        private IEnumerator Start()
        {
            GetComponent<Canvas>().enabled = true;
            yield return new WaitForSeconds(4);
            Destroy(gameObject);
        }
    }
}

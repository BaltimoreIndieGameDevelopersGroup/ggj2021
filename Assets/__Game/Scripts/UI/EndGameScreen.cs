namespace Game
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using TMPro;

    public class EndGameScreen : MonoBehaviour
    {
        private IEnumerator Start()
        {
            GetComponent<Canvas>().enabled = true;
            var textMesh = GetComponentInChildren<TextMeshProUGUI>();
            const float ScrollDuration = 4f;

            // Scroll text:
            float elapsed = 0;
            while (elapsed < ScrollDuration)
            {
                var t = elapsed / ScrollDuration;
                textMesh.rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(-600f, -20f, t));
                yield return null;
                elapsed += Time.deltaTime;
            }
        }

        protected virtual void Update()
        {
            if (Input.anyKeyDown || Input.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}

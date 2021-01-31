namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private GameText gameText;

        private const float DetectionGracePeriod = 3;

        private IEnumerator Start()
        {
            ServiceLocator.Get<IPlayerController>().AllowDetection = false;

            GetComponent<Canvas>().enabled = true;
            var textMesh = GetComponentInChildren<TextMeshProUGUI>();
            var image = GetComponentInChildren<Image>();
            const float ScrollDuration = 4f;
            const float FadeDuration = 0.5f;

            var readInput = false;

            // Scroll text:
            textMesh.text = gameText.IntroText;
            float elapsed = 0;
            while (elapsed < ScrollDuration)
            {
                if (Input.anyKeyDown || Input.GetButtonDown("Fire1")) readInput = true;
                var t = elapsed / ScrollDuration;
                textMesh.rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(-600f, -20f, t));
                yield return null;
                elapsed += Time.deltaTime;
            }

            // Wait for input:
            while (!readInput)
            {
                yield return null;
                if (Input.anyKeyDown || Input.GetButtonDown("Fire1")) readInput = true;
            }

            // Fade black:
            elapsed = 0;
            while (elapsed < FadeDuration)
            {
                var t = elapsed / FadeDuration;
                image.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
                yield return null;
                elapsed += Time.deltaTime;
            }

            // Fade text:
            elapsed = 0;
            while (elapsed < FadeDuration)
            {
                var t = elapsed / FadeDuration;
                textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, Mathf.Lerp(1, 0, t));
                yield return null;
                elapsed += Time.deltaTime;
            }

            GetComponent<Canvas>().enabled = true;

            yield return new WaitForSeconds(DetectionGracePeriod);

            ServiceLocator.Get<IPlayerController>().AllowDetection = true;

            Destroy(gameObject);
        }
    }
}

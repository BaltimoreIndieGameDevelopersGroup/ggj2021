namespace Game
{
    using System.Collections;
    using UnityEngine;
    using TMPro;

    public class MessageUI : MonoBehaviour, IMessageUI
    {
        [SerializeField] private TextMeshProUGUI messageTextMesh;
        [SerializeField] private TextMeshProUGUI scoreTextMesh;
        [SerializeField] private TextMeshProUGUI alien1TextMesh;
        [SerializeField] private TextMeshProUGUI alien2TextMesh;

        private const float MessageDuration = 5;

        private Coroutine banterCoroutine = null;

        private void OnEnable()
        {
            ServiceLocator.Register<IMessageUI>(this);
            messageTextMesh.enabled = false;
        }

        public void ShowMessage(string message)
        {
            StopAllCoroutines();
            StartCoroutine(ShowMessageCoroutine(message));
        }

        private IEnumerator ShowMessageCoroutine(string message)
        {
            messageTextMesh.enabled = true;
            messageTextMesh.text = message;
            float elapsed = 0;
            const float FadeDuration = 0.25f;
            while (elapsed < FadeDuration)
            {
                var alpha = Mathf.Lerp(0, 1, elapsed / FadeDuration);
                messageTextMesh.color = new Color(messageTextMesh.color.r, messageTextMesh.color.g, messageTextMesh.color.b, alpha);
                yield return null;
                elapsed += Time.deltaTime;
            }
            messageTextMesh.color = new Color(messageTextMesh.color.r, messageTextMesh.color.g, messageTextMesh.color.b, 1);
            yield return new WaitForSeconds(MessageDuration);
            elapsed = 0;
            while (elapsed < FadeDuration)
            {
                var alpha = Mathf.Lerp(1, 0, elapsed / FadeDuration);
                messageTextMesh.color = new Color(messageTextMesh.color.r, messageTextMesh.color.g, messageTextMesh.color.b, alpha);
                yield return null;
                elapsed += Time.deltaTime;
            }
            messageTextMesh.color = new Color(messageTextMesh.color.r, messageTextMesh.color.g, messageTextMesh.color.b, 0);
            messageTextMesh.enabled = false;
        }

        public void SetScore(int score)
        {
            scoreTextMesh.text = "Score: " + score;
        }

        public void PlayBanter(string[] lines)
        {
            if (banterCoroutine != null) StopCoroutine(banterCoroutine);
            banterCoroutine = StartCoroutine(BanterCoroutine(lines));
        }

        private IEnumerator BanterCoroutine(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                var textMesh = (i % 2 == 0) ? alien1TextMesh : alien2TextMesh;
                textMesh.enabled = true;
                textMesh.text = lines[i];
                yield return new WaitForSeconds(2);
                textMesh.enabled = false;
            }
            banterCoroutine = null;
        }
    }
}

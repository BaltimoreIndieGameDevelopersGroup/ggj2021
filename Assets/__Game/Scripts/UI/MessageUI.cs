namespace Game
{
    using System.Collections;
    using UnityEngine;
    using TMPro;

    public class MessageUI : MonoBehaviour, IMessageUI
    {
        [SerializeField] private TextMeshProUGUI messageTextMesh;
        [SerializeField] private TextMeshProUGUI scoreTextMesh;

        private const float MessageDuration = 5;

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

        public void  SetScore(int score)
        {
            scoreTextMesh.text = "Score: " + score;
        }
    }
}

namespace Game
{
    using UnityEngine;
    using TMPro;

    public class MessageUI : MonoBehaviour, IMessageUI
    {
        [SerializeField] private TextMeshProUGUI messageTextMesh;

        private const float MessageDuration = 5;

        private void OnEnable()
        {
            ServiceLocator.Register<IMessageUI>(this);
            messageTextMesh.enabled = false;
        }

        public void ShowMessage(string message)
        {
            messageTextMesh.enabled = true;
            messageTextMesh.text = message;
            Invoke(nameof(HideMessage), MessageDuration);
        }

        private void HideMessage()
        {
            messageTextMesh.enabled = false;
        }
    }
}

namespace Game
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using TMPro;

    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private int musicLevel;
        [SerializeField] private TextMeshProUGUI textMesh;

        private float timeLeftToAllowContinue = 2;

        private const float ScrollDuration = 4;

        protected virtual void Start()
        {
            ServiceLocator.Get<IAudioManager>().PlayMusicLevel(musicLevel);
            GetComponent<Canvas>().enabled = true;
            StartTextScroll();
        }

        protected virtual void StartTextScroll()
        {
            StartCoroutine(ScrollText(textMesh.text));
        }

        protected IEnumerator ScrollText(string text)
        {
            textMesh.text = text;

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
            timeLeftToAllowContinue -= Time.deltaTime;
            if (timeLeftToAllowContinue <= 0)
            {
                if (Input.anyKeyDown || Input.GetButtonDown("Fire1"))
                {
                    DoneScrolling();
                }
            }
        }

        protected virtual void DoneScrolling()
        {
            SceneManager.LoadScene(0);
        }
    }
}

namespace Game
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Persistent singleton.
    /// </summary>
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] private FMODUnity.StudioEventEmitter musicEmitter;

        private float currentMusicIntensity = 0;

        private static AudioManager instance = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                ServiceLocator.Register<IAudioManager>(this);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            } 
        }

        private void Start()
        {
            SetMusicForCurrentScene();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetMusicForCurrentScene();
        }

        private void SetMusicForCurrentScene()
        { 
            musicEmitter.SetParameter("Intensity Level", SceneManager.GetActiveScene().buildIndex);
        }

        public void PlayDangerMusic()
        {
            if (currentMusicIntensity < 1)
            {
                currentMusicIntensity = 1;
                musicEmitter.SetParameter("Intensity Level", 1);
            }
        }

        public void PlaySafeMusic()
        {
            if (currentMusicIntensity > 0)
            {
                currentMusicIntensity = 0;
                musicEmitter.SetParameter("Intensity Level", 0);
            }
        }

    }
}

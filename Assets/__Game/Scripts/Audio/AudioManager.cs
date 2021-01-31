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
        [SerializeField] private bool debug = false;

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
            musicEmitter.Play();
            SetMusicForCurrentScene();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetMusicForCurrentScene();
        }

        private void SetMusicForCurrentScene()
        {
            if (debug) Debug.Log($"SCENE MUSIC Level Navigation={SceneManager.GetActiveScene().buildIndex}, Intensity Level=0");
            musicEmitter.SetParameter("Level Navigation", SceneManager.GetActiveScene().buildIndex);
            musicEmitter.SetParameter("Intensity Level", 0);
            currentMusicIntensity = 0;
        }

        public void PlayMusicLevel(int level)
        {
            if (debug) Debug.Log($"MUSIC Level Navigation={level}, Intensity Level=0");
            musicEmitter.SetParameter("Level Navigation", level);
            musicEmitter.SetParameter("Intensity Level", 0);
        }

        public void PlayDangerMusic()
        {
            if (currentMusicIntensity < 1)
            {
                currentMusicIntensity = 1;
                musicEmitter.SetParameter("Level Navigation", 1);
                musicEmitter.SetParameter("Intensity Level", 1);
                if (debug) Debug.Log($"MUSIC Intensity Level=1");
            }
        }

        public void PlaySafeMusic()
        {
            if (currentMusicIntensity > 0)
            {
                currentMusicIntensity = 0;
                musicEmitter.SetParameter("Level Navigation", 1);
                musicEmitter.SetParameter("Intensity Level", 0);
                if (debug) Debug.Log($"MUSIC Intensity Level=0");
            }
        }

    }
}

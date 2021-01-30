namespace Game
{
    using UnityEngine;

    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] private FMODUnity.StudioEventEmitter musicEmitter;

        private float currentMusicIntensity = 0;

        private void Awake()
        {
            ServiceLocator.Register<IAudioManager>(this);   
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

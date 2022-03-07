using GameBrains.Extensions.MonoBehaviours;
using GameBrains.SceneManagement;
using UnityEngine;

namespace GameBrains.Audio
{
    // Alternative to AudioManager
    [RequireComponent(typeof(AudioSource))]
    public class SoundsPlayer : SingletonMonoBehaviour<SoundsPlayer>
    {
        public enum SoundPlayerActions
        {
            None,
            PlayButtonClick,
            Pause,
            Resume
        }

        public SoundPlayerActions SoundPlayerAction
        {
            get => soundPlayerAction;
            private set => soundPlayerAction = value;
        }
        [Tooltip("soundPlayerAction selects an action to perform in Update.")]
        [SerializeField] SoundPlayerActions soundPlayerAction;

        [Tooltip("soundsSource is the sound effects audio source component in this scene.")]
        [SerializeField] AudioSource soundsSource;

        [Tooltip("buttonClickClip stores the sound to be played on button click.")]
        [SerializeField] AudioClip buttonClickClip;

        public override void Awake()
        {
            base.Awake();

            if (!soundsSource) { soundsSource = GetComponent<AudioSource>(); }

            if (!PlayerPrefs.HasKey("MuteSounds")) { PlayerPrefs.SetInt("MuteSounds", 0); }

            soundsSource.mute = AreSoundsMuted;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            PauseManager.OnPauseToggled += PausePlayer;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            PauseManager.OnPauseToggled -= PausePlayer;
        }

        public override void Update()
        {
            base.Update();

            switch (SoundPlayerAction)
            {
                case SoundPlayerActions.PlayButtonClick:
                    PlayButtonClick();
                    break;
                case SoundPlayerActions.Pause:
                    PausePlayer(true);
                    break;
                case SoundPlayerActions.Resume:
                    PausePlayer(false);
                    break;
                case SoundPlayerActions.None:
                    break;
            }

            SoundPlayerAction = SoundPlayerActions.None;

            soundsSource.mute = AreSoundsMuted;
        }

        public void PlayButtonClick()
        {
            soundsSource.PlayOneShot(buttonClickClip);

            if (VerbosityDebugOrLog)
            {
                Log.Debug($"Playing clip [{buttonClickClip.name}]");
            }
        }

        void PausePlayer(bool pause)
        {
            if (soundsSource.isPlaying != pause) { return; }

            if (VerbosityDebugOrLog)
            {
                var statusMessage = pause ? "Pausing" : "Resuming";
                Log.Debug($"{statusMessage} current clip.");
            }

            if (pause) { soundsSource.Pause(); } else { soundsSource.UnPause(); }
        }

        public static bool AreSoundsMuted => PlayerPrefs.GetInt("MuteSounds") == 1;
    }
}
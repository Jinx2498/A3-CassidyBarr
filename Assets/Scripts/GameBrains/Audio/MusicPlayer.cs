using GameBrains.Extensions.MonoBehaviours;
using GameBrains.SceneManagement;
using UnityEngine;

namespace GameBrains.Audio
{
    // Alternative to AudioManager
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : SingletonMonoBehaviour<MusicPlayer>
    {
        public enum MusicPlayerActions
        {
            None,
            Play,
            Pause,
            Resume,
            Stop,
            Next,
            Previous,
            Loop,
            OneShot
        }

        public MusicPlayerActions MusicPlayerAction
        {
            get => musicPlayerAction;
            private set => musicPlayerAction = value;
        }
        [Tooltip("musicPlayerAction selects an action to perform in Update.")]
        [SerializeField] MusicPlayerActions musicPlayerAction;

        [Tooltip("musicSource is the music audio source component in this scene.")]
        [SerializeField] AudioSource musicSource;

        [Tooltip("musicTracks holds the music clips to be played continuously during the scene.")]
        public AudioClip[] musicTracks;

        int currentTrack;


        public override void Awake()
        {
            base.Awake();

            if (!musicSource) { musicSource = GetComponent<AudioSource>(); }

            if (!PlayerPrefs.HasKey("MuteMusic")) { PlayerPrefs.SetInt("MuteMusic", 0); }

            currentTrack = 0;
            StartPlayer();
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

            switch (MusicPlayerAction)
            {
                case MusicPlayerActions.Play:
                    StartPlayer();
                    break;
                case MusicPlayerActions.Stop:
                    StopPlayer();
                    break;
                case MusicPlayerActions.Next:
                    NextTrack();
                    break;
                case MusicPlayerActions.Previous:
                    PreviousTrack();
                    break;
                case MusicPlayerActions.Pause:
                    PausePlayer(true);
                    break;
                case MusicPlayerActions.Resume:
                    PausePlayer(false);
                    break;
                case MusicPlayerActions.Loop:
                    LoopPlayer(true);
                    break;
                case MusicPlayerActions.OneShot:
                    LoopPlayer(false);
                    break;
                case MusicPlayerActions.None:
                    break;
            }

            MusicPlayerAction = MusicPlayerActions.None;

            musicSource.mute = IsMusicMuted;
        }

        void StartPlayer()
        {
            musicSource.mute = IsMusicMuted;
            musicSource.clip = musicTracks[currentTrack];
            musicSource.Play();

            if (VerbosityDebugOrLog)
            {
                var trackName = musicTracks[currentTrack].name;
                Log.Debug($"Playing track {currentTrack} [{trackName}]");
            }
        }

        void StopPlayer()
        {
            musicSource.Stop();

            if (VerbosityDebugOrLog)
            {
                var trackName = musicTracks[currentTrack].name;
                Log.Debug($"Stopping track {currentTrack} [{trackName}]");
            }
        }

        void NextTrack() => ChangeTrack(currentTrack + 1);

        void PreviousTrack() => ChangeTrack(currentTrack - 1);

        void ChangeTrack(int track)
        {
            track %= musicTracks.Length;

            if (currentTrack == track) { return; }

            StopPlayer();

            if (VerbosityDebugOrLog)
            {
                var trackName = musicTracks[track].name;
                Log.Debug($"Changing track from {currentTrack} to {track} [{trackName}]");
            }

            currentTrack = track;

            StartPlayer();
        }

        void PausePlayer(bool pause)
        {
            if (musicSource.isPlaying != pause) { return; }

            if (VerbosityDebugOrLog)
            {
                var trackName = musicTracks[currentTrack].name;
                var statusMessage = pause ? "Pausing" : "Resuming";
                Log.Debug($"{statusMessage} track {currentTrack} [{trackName}]");
            }

            if (pause) { musicSource.Pause(); } else { musicSource.UnPause(); }
        }

        void LoopPlayer(bool loop)
        {
            if (musicSource.loop == loop) { return; }

            if (VerbosityDebugOrLog)
            {
                var trackName = musicTracks[currentTrack].name;
                var statusMessage = loop ? "Looping" : "Stopping loop of";
                Log.Debug($"{statusMessage} track {currentTrack}: [{trackName}]");
            }

            musicSource.loop = loop;
        }

        public static bool IsMusicMuted => PlayerPrefs.GetInt("MuteMusic") == 1;
    }
}
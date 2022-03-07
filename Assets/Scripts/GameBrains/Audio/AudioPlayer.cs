using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameBrains.Extensions.ScriptableObjects;
using GameBrains.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Audio
{
#if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Audio",
        FilePathAttribute.Location.ProjectFolder)]
#endif
    [CreateAssetMenu(fileName = "AudioPlayer", menuName = "GameBrains/Audio/AudioPlayer")]
    public sealed class AudioPlayer : ExtendedScriptableObject
    {
        public static readonly Dictionary<string, AudioPlayer> NameToPlayer = new Dictionary<string, AudioPlayer>();
        public static readonly List<AudioPlayer> AudioPlayers = new List<AudioPlayer>();
        public enum AudioCategories { Music, SoundEffect }
        
        [SerializeField] AudioCategories audioCategory = AudioCategories.SoundEffect;
        [SerializeField] AudioClip clip;
        [SerializeField] [Range(0f, 1f)] float volume = 1f;
        [SerializeField] [Range(0.1f, 3f)] float pitch = 1f;
        [SerializeField] bool loop;

        public AudioSource AudioSource { get; set; }
        public static bool AreSoundsMuted => PlayerPrefs.GetInt("MuteSounds") == 1;

        public string Name => name;
        public AudioClip Clip => clip;
        public float Volume => volume;
        public float Pitch => pitch;
        public bool Loop => loop;

        public override void OnEnable()
        {
            base.OnEnable();
            PauseManager.OnPauseToggled += PausePlayer;

#if UNITY_EDITOR
            name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
#endif
            AudioPlayers.Add(this);
            NameToPlayer[Name] = this;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PauseManager.OnPauseToggled -= PausePlayer;
        }

        public static AudioPlayer[] MusicPlayers
        {
            get
            {
                bool MatchMusic(AudioPlayer ap) => ap.audioCategory == AudioCategories.Music;
                return AudioPlayers.Where(MatchMusic).ToArray();
            }
        }
        
        public static AudioPlayer[] SoundPlayers
        {
            get
            {
                bool MatchSound(AudioPlayer ap) => ap.audioCategory == AudioCategories.SoundEffect;
                return AudioPlayers.Where(MatchSound).ToArray();
            }
        }

        public void AttachTo(GameObject gameObject)
        {
            AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.clip = Clip;
            AudioSource.volume = Volume;
            AudioSource.pitch = Pitch;
            AudioSource.loop = Loop;

            if (!PlayerPrefs.HasKey("MuteSounds")) { PlayerPrefs.SetInt("MuteSounds", 0); }

            AudioSource.mute = AreSoundsMuted;
        }
        
        public static void AttachAllTo(GameObject gameObject)
        {
            foreach (AudioPlayer audioEntry in AudioPlayers)
            {
                audioEntry.AttachTo(gameObject);
            }
        }

        public void Play()
        {
            AudioSource.Play();

            if (VerbosityDebugOrLog)
            {
                Log.Debug($"Playing clip [{AudioSource.clip.name}]");
            }
        }
        
        public void Stop()
        {
            AudioSource.Stop();

            if (VerbosityDebugOrLog)
            {
                Log.Debug($"Stopping clip [{AudioSource.clip.name}]");
            }
        }

        public void PausePlayer(bool pause)
        {
            if (AudioSource.isPlaying != pause) { return; }

            if (VerbosityDebugOrLog)
            {
                var statusMessage = pause ? "Pausing" : "Resuming";
                Log.Debug($"{statusMessage} current clip.");
            }

            if (pause) { AudioSource.Pause(); } else { AudioSource.UnPause(); }
        }

        public void LoopPlayer(bool loopPlayer)
        {
            loop = loopPlayer;
            
            if (VerbosityDebugOrLog)
            {
                var statusMessage = Loop ? "Looping" : "Not Looping";
                Log.Debug($"{statusMessage} current clip.");
            }
            
            AudioSource.loop = Loop;
        }

        public void MutePlayer(bool mute)
        {
            if (VerbosityDebugOrLog)
            {
                var statusMessage = mute ? "Muting" : "UnMuting";
                Log.Debug($"{statusMessage} current clip.");
            }
            
            AudioSource.mute = mute;
        }
    }
    
    public partial class AudioPlayers
    {
        public static readonly AudioPlayer ButtonClick
            = Resources.Load<AudioPlayer>($"ScriptableObjects/Audio/{nameof(ButtonClick)}");
        public static readonly AudioPlayer MusicBoxClick
            = Resources.Load<AudioPlayer>($"ScriptableObjects/Audio/{nameof(MusicBoxClick)}");
    }
}
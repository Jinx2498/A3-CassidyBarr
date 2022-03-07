using System;
using System.Collections.Generic;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameBrains.Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        public enum AudioManagerActions
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

        public AudioManagerActions AudioManagerAction
        {
            get => audioManagerAction;
            private set => audioManagerAction = value;
        }

        [Tooltip("audioManagerAction selects an action to perform in Update.")]
        [SerializeField] AudioManagerActions audioManagerAction;
        [SerializeField] List<AudioPlayer> audioPlayers;
        [SerializeField] int currentTrack;
        [SerializeField] bool playMusicAtStart = true;

        InputActions inputActions;
        Action<InputAction.CallbackContext> playTrack;
        Action<InputAction.CallbackContext> nextTrack;
        Action<InputAction.CallbackContext> previousTrack;
        
        public override void Awake()
        {
            base.Awake();
            
            inputActions = new InputActions();
            playTrack = _ => PlayTrack(currentTrack);
            nextTrack = _ => PlayTrack(currentTrack + 1);
            previousTrack = _ => PlayTrack(currentTrack - 1);

            AudioPlayer.AttachAllTo(gameObject);
            audioPlayers = AudioPlayer.AudioPlayers;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            inputActions.Enable();
            inputActions.AudioAction.PlayTrack.performed += playTrack;
            inputActions.AudioAction.NextTrack.performed += nextTrack;
            inputActions.AudioAction.PreviousTrack.performed += previousTrack;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            inputActions.AudioAction.PlayTrack.performed -= playTrack;
            inputActions.AudioAction.NextTrack.performed -= nextTrack;
            inputActions.AudioAction.PreviousTrack.performed -= previousTrack;
            inputActions.Disable();
        }

        public override void Start()
        {
            base.Start();
            if (playMusicAtStart) { PlayTrack(currentTrack); }
        }

        public override void Update()
        {
            base.Update();
            
            switch (AudioManagerAction)
            {
                case AudioManagerActions.Play:
                    PlayTrack(currentTrack);
                    break;
                case AudioManagerActions.Stop:
                    StopTrack(currentTrack);
                    break;
                case AudioManagerActions.Next:
                    PlayTrack(currentTrack + 1);
                    break;
                case AudioManagerActions.Previous:
                    PlayTrack(currentTrack - 1);
                    break;
                case AudioManagerActions.Pause:
                    PauseTrack(true, currentTrack);
                    break;
                case AudioManagerActions.Resume:
                    PauseTrack(false, currentTrack);
                    break;
                case AudioManagerActions.Loop:
                    LoopTrack(true, currentTrack);
                    break;
                case AudioManagerActions.OneShot:
                    LoopTrack(false, currentTrack);
                    break;
                case AudioManagerActions.None:
                    break;
            }
            
            AudioManagerAction = AudioManagerActions.None;
            MuteTrack(IsMusicMuted, currentTrack);
        }

        public void PlayTrack(int track)
        {
            if (AudioPlayer.MusicPlayers.Length <= 0) { return; }
            currentTrack = track % AudioPlayer.MusicPlayers.Length;
            AudioPlayer.MusicPlayers[currentTrack].Play();
        }

        public void StopTrack(int track)
        {
            if (AudioPlayer.MusicPlayers.Length <= 0) { return; }
            track %= AudioPlayer.MusicPlayers.Length;
            AudioPlayer.MusicPlayers[track].Stop();
        }

        public void PauseTrack(bool pause, int track)
        {
            if (AudioPlayer.MusicPlayers.Length <= 0) { return; }
            track %= AudioPlayer.MusicPlayers.Length;
            AudioPlayer.MusicPlayers[track].PausePlayer(pause);
        }

        public void LoopTrack(bool loop, int track)
        {
            if (AudioPlayer.MusicPlayers.Length <= 0) { return; }
            track %= AudioPlayer.MusicPlayers.Length;
            AudioPlayer.MusicPlayers[track].LoopPlayer(loop);
        }

        public void MuteTrack(bool mute, int track)
        {
            if (AudioPlayer.MusicPlayers.Length <= 0) { return; }
            track %= AudioPlayer.MusicPlayers.Length;
            AudioPlayer.MusicPlayers[track].MutePlayer(mute);
        }
        
        public static bool IsMusicMuted => PlayerPrefs.GetInt("MuteMusic") == 1;
    }
}
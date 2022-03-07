using System;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameBrains.SceneManagement
{
    public sealed class PauseManager : SingletonMonoBehaviour<PauseManager>
    {
        public static bool IsPaused { get; private set; }

        public delegate void PauseToggledHandler(bool isPaused);
        public static event PauseToggledHandler OnPauseToggled;

        float timeScale;
        InputActions inputActions;
        Action<InputAction.CallbackContext> togglePause;

        public override void Awake()
        {
            base.Awake();

            inputActions = new InputActions();
            togglePause = _ => TogglePause();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            inputActions.Enable();
            inputActions.PauseAction.PauseGame.performed += togglePause;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            inputActions.PauseAction.PauseGame.performed -= togglePause;
            inputActions.Disable();
        }

        public void TogglePause()
        {
            if (IsPaused) { Time.timeScale = timeScale; }
            else { timeScale = Time.timeScale; Time.timeScale = 0; }

            IsPaused = !IsPaused;

            if (VerbosityDebugOrLog)
            {
                var pauseMessage = IsPaused ? "Pausing" : "Resuming";
                Log.Debug($"PauseManager is {pauseMessage}.");
            }

            OnPauseToggled?.Invoke(IsPaused);
        }
    }
}
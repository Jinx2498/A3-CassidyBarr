using System;
using GameBrains.Extensions;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.InputManagement;
using GameBrains.SceneManagement;
using UnityEngine.InputSystem;

namespace GameBrains.GameManagement
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        InputActions inputActions;
        Action<InputAction.CallbackContext> quit;
        
        public override void Awake()
        {
            base.Awake();
            inputActions = new InputActions();
            quit = _ => Utility.Quit();
            DontDestroyOnLoad(gameObject);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            inputActions.Enable();
            PauseManager.OnPauseToggled += PauseGame;
            inputActions.SceneManagement.Quit.performed += quit;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PauseManager.OnPauseToggled -= PauseGame;
            inputActions.SceneManagement.Quit.performed -= quit;
            inputActions.Disable();
        }
        
        public void PauseGame(bool pause)
        {
            // Game Pause functions here
        }
    }
}
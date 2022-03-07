using System;
using GameBrains.Extensions.GameObjects;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.InputManagement;
using UnityEngine.InputSystem;

namespace GameBrains.SceneManagement
{
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        public Loader.Scene mainScene;
        public bool loadScenesAsynchronously = true;
        public bool useMainMenu;
        public float countDownTimer = 0;

        CountDown countDown;
        InputActions inputActions;
        Action<InputAction.CallbackContext> mainMenu;

        public override void Awake()
        {
            base.Awake();

            inputActions = new InputActions();
            mainMenu = _ => LoadMainMenu();
            countDown = gameObject.GetOrAddComponent<CountDown>();
            countDown.sceneManager = this;
            countDown.useMainMenu = useMainMenu;
            countDown.timer = countDownTimer;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            inputActions.Enable();
            inputActions.SceneManagement.MainMenu.performed += mainMenu;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            inputActions.SceneManagement.MainMenu.performed -= mainMenu;
            inputActions.Disable();
        }

        public void LoadMainMenu()
        {
            if (countDown) { Destroy(countDown); }
            Loader.Load(Loader.Scene.MainMenu, loadScenesAsynchronously);
        }

        public void LoadMainScene()
        {
            if (countDown) { Destroy(countDown); }
            Loader.Load(mainScene,loadScenesAsynchronously);
        }
    }
}
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.SceneManagement;
using UnityEngine;

namespace GameBrains.GameManagement
{
    public class InGameMenuController : SingletonMonoBehaviour<InGameMenuController>
    {
        [SerializeField] GameObject inGameMenu;

        public override void Awake()
        {
            base.Awake();
            if (!inGameMenu)
            {
                var inGameMenuCanvas = GetComponentInChildren<Canvas>();
                if (!inGameMenuCanvas) { return; }
                inGameMenu = inGameMenuCanvas.gameObject;
            }

            ActivateMenu(false);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            PauseManager.OnPauseToggled += ActivateMenu;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PauseManager.OnPauseToggled -= ActivateMenu;
        }
        
        public void ActivateMenu(bool pause)
        {
            if (inGameMenu) { inGameMenu.SetActive(pause); }
        }

        public void ResumeGame()
        {
            PauseManager.Instance.TogglePause();
        }
    }
}
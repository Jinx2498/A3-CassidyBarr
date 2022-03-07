using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.SceneManagement
{
    public class CountDown : ExtendedMonoBehaviour
    {
        public float timer = 2f;
        public SceneManager sceneManager;
        public bool useMainMenu;
        
        public override void Update()
        {
            base.Update();
            timer -= Time.deltaTime;
            if (timer > 0) { return; }

            if (useMainMenu)
            {
                sceneManager.LoadMainMenu(); 
            }
            else
            {
                sceneManager.LoadMainScene(); 
            }
            Destroy(this);
        }
    }
}
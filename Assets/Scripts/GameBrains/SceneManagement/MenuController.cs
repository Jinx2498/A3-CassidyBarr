using GameBrains.Audio;
using GameBrains.Extensions;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace GameBrains.SceneManagement
{
    public class MenuController : ExtendedMonoBehaviour
    {
        [Tooltip("webpageURL is the URL opened when users click on your branding icon.")]
        public string webpageURL = "http://www.alpaca.studio";

        [Tooltip("soundButtons store the SoundOn and SoundOff buttons.")]
        public Button[] soundButtons;

        public void OpenWebpage()
        {
            AudioPlayers.ButtonClick.Play();
            Application.OpenURL(webpageURL);
        }

        public void PlayGame()
        {
            AudioPlayers.ButtonClick.Play();
            Loader.Load(SceneManager.Instance.mainScene, SceneManager.Instance.loadScenesAsynchronously);
        }

        public void Options()
        {
            AudioPlayers.ButtonClick.Play();
            Loader.Load(Loader.Scene.Options, SceneManager.Instance.loadScenesAsynchronously);
        }

        public void MainMenu()
        {
            AudioPlayers.ButtonClick.Play();
            Loader.Load(Loader.Scene.MainMenu, SceneManager.Instance.loadScenesAsynchronously);
        }

        public void MuteMusic(bool mute)
        {
            AudioPlayers.ButtonClick.Play();
            soundButtons[0].interactable = mute;
            soundButtons[1].interactable = !mute;
            PlayerPrefs.SetInt("MuteMusic", mute ? 1 : 0);
        }

        public void MuteSounds(bool mute) { PlayerPrefs.SetInt("MuteSounds", mute ? 1 : 0); }

        public void QuitGame()
        {
            AudioPlayers.ButtonClick.Play();
            Utility.Quit();
        }
    }
}
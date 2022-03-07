using System;
using System.Collections;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.SceneManagement
{
    public static class Loader
    {
        public enum Scene
        {
            MainMenu,
            Options,
            Loading,
            FirstScene,
        }
        
        static Action onLoaderCallback;
        static AsyncOperation loadingAsyncOperation;

        public static void Load(Scene scene, bool async = true)
        {
            if (async)
            {
                LoadAsynchronously(scene.ToString());
            }
            else
            {
                LoadSynchronously(scene.ToString());
            }
        }

        static void LoadSynchronously(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        static void LoadAsynchronously(string sceneName)
        {
            onLoaderCallback = () =>
            {
                var loadingGameObject = new GameObject("Loading Game Object");
                var loadingMonoBehaviour
                    = loadingGameObject.AddComponent<LoadingMonoBehaviour>();
                loadingMonoBehaviour.StartCoroutine(LoadSceneAsync(sceneName));
            };
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scene.Loading.ToString());
        }

        static IEnumerator LoadSceneAsync(string sceneName)
        {
            loadingAsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            while (!loadingAsyncOperation.isDone) { yield return null; }
        }

        public static float GetLoadingProgress() { return loadingAsyncOperation?.progress ?? 1f; }

        public static void LoaderCallback()
        {
            if (onLoaderCallback == null) { return; }

            onLoaderCallback();
            onLoaderCallback = null;
        }

        class LoadingMonoBehaviour : ExtendedMonoBehaviour { }
    }
}
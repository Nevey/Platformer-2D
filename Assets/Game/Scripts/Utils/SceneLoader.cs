using System;
using System.Collections.Generic;
using System.Linq;
using Game.DI;
using UnityEngine.SceneManagement;

namespace Game.Utils
{
    [Injectable]
    public class SceneLoader
    {
        private readonly List<string> loadingScenes = new List<string>();
        private readonly List<string> unloadingScenes = new List<string>();

        private Action loadingFinishedCallback;
        private Action unloadFinishedCallback;

        public SceneLoader()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        ~SceneLoader()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            loadingScenes.Remove(arg0.name);
            CheckForLoadingFinished();
        }

        private void OnSceneUnloaded(Scene arg0)
        {
            unloadingScenes.Remove(arg0.name);
            CheckForUnloadingFinished();
        }

        private void LoadScene(string name, LoadSceneMode loadSceneMode)
        {
            loadingScenes.Add(name);
            SceneManager.LoadSceneAsync(name, loadSceneMode);
        }

        private void UnloadScene(string name)
        {
            unloadingScenes.Add(name);
            SceneManager.UnloadSceneAsync(name);
        }

        private void CheckForLoadingFinished()
        {
            CheckFinished(loadingScenes, ref loadingFinishedCallback);
        }

        private void CheckForUnloadingFinished()
        {
            CheckFinished(unloadingScenes, ref unloadFinishedCallback);
        }

        private void CheckFinished(List<string> sceneNames, ref Action callback)
        {
            if (sceneNames.Count > 0)
            {
                return;
            }

            callback?.Invoke();
            callback = null;
        }

        public void LoadScenes(params string[] sceneNames)
        {
            for (int i = 0; i < sceneNames.Length; i++)
            {
                LoadScene(sceneNames[i], LoadSceneMode.Additive);
            }
        }

        public void LoadScenes(Action callback, params string[] sceneNames)
        {
            loadingFinishedCallback = callback;
            LoadScenes(sceneNames);
        }

        public void UnloadScenes(params string[] sceneNames)
        {
            for (int i = 0; i < sceneNames.Length; i++)
            {
                UnloadScene(sceneNames[i]);
            }
        }

        public void UnloadScenes(Action callback, params string[] sceneNames)
        {
            unloadFinishedCallback = callback;
            UnloadScenes(sceneNames);
        }

        public void UnloadAllScenes(Action callback, params string[] scenesToSkip)
        {
            unloadFinishedCallback = callback;

            int unloadSceneCount = 0;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scenesToSkip.Contains(scene.name))
                {
                    continue;
                }

                UnloadScene(scene.name);
                unloadSceneCount++;
            }

            if (unloadSceneCount == 0)
            {
                CheckForUnloadingFinished();
            }
        }
    }
}

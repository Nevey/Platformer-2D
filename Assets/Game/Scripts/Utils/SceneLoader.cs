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

        private string sceneNameToSetActive;

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

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            loadingScenes.Remove(scene.name);
            CheckForLoadingFinished();

            if (sceneNameToSetActive == scene.name)
            {
                SceneManager.SetActiveScene(scene);
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            unloadingScenes.Remove(scene.name);
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

        public void LoadScenes(bool setFirstSceneAsActive, params string[] sceneNames)
        {
            sceneNameToSetActive = setFirstSceneAsActive ? sceneNames[0] : null;

            for (int i = 0; i < sceneNames.Length; i++)
            {
                LoadScene(sceneNames[i], LoadSceneMode.Additive);
            }
        }

        public void LoadScenes(Action callback, bool setFirstSceneAsActive, params string[] sceneNames)
        {
            loadingFinishedCallback = callback;
            LoadScenes(setFirstSceneAsActive, sceneNames);
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

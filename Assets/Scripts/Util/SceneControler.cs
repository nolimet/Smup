//Author Jesse Stam
//Created 12-2-2016
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Util
{
    /// <summary>
    /// Controles scence changes
    /// this was write because Application.load has been marked as legacy
    /// </summary>
    public static class SceneControler
    {
        /// <summary>
        /// Loads a single scene into the game and unload any otherloaded scenes
        /// </summary>
        /// <param name="SceneName">Name of the scene that will be loaded</param>
        public static void Load(string SceneName)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }

        /// <summary>
        /// Loads a scene as a aditive
        /// </summary>
        /// <param name="SceneName"></param>
        public static void LoadAddative(string SceneName)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unloades a scene
        /// </summary>
        /// <param name="SceneName">Name of scene that will be unloaded</param>
        public static void UnloadScene(string SceneName)
        {
            int scenecount = SceneManager.sceneCount;

            for (int i = 0; i < scenecount; i++)
            {
                if (SceneName == SceneManager.GetSceneAt(i).name)
                    if (SceneManager.GetSceneAt(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
            }
        }
    }
}

//Author Jesse Stam
//Created 12-2-2016

using UnityEngine.SceneManagement;

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
        /// <param name="sceneName">Name of the scene that will be loaded</param>
        public static void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        /// <summary>
        /// Loads a scene as a aditive
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadAddative(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unloades a scene
        /// </summary>
        /// <param name="sceneName">Name of scene that will be unloaded</param>
        public static void UnloadScene(string sceneName)
        {
            var scenecount = SceneManager.sceneCount;

            for (var i = 0; i < scenecount; i++)
                if (sceneName == SceneManager.GetSceneAt(i).name)
                    if (SceneManager.GetSceneAt(i).isLoaded)
                        SceneManager.UnloadSceneAsync(i);
        }
    }
}

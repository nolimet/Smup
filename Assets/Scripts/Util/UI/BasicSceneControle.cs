using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util.UI
{
    /// <summary>
    /// Used in quick prototyping of buttons for the UI sytem
    /// </summary>
    public class BasicSceneControle : MonoBehaviour
    {
        public void OpenScene(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }

        public void CloseGame()
        {
            Application.Quit();
        }
    }
}

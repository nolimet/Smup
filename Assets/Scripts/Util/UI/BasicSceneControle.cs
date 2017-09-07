using UnityEngine;
using System.Collections;

namespace Util.UI
{
    /// <summary>
    /// Used in quick prototyping of buttons for the UI sytem
    /// </summary>
    public class BasicSceneControle : MonoBehaviour
    {

        public void OpenScene(string name)
        {
            SceneControler.Load(name);
        }

        public void CloseGame()
        {
            Application.Quit();
        }
    }
}
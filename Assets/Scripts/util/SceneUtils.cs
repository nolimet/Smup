using UnityEngine;
using System.Collections;

namespace util
{
    public class SceneUtils : MonoBehaviour
    {

        public void OpenScene(string name)
        {
            Application.LoadLevel(name);
        }

        public void CloseGame()
        {
            Application.Quit();
        }
    }
}
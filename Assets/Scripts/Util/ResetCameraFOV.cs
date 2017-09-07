#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Util
{
    /// <summary>
    /// Forces a reset on Edetor Camera FOV
    /// </summary>
    public class ResetCameraFOV : ScriptableObject
    {

        [MenuItem("ToolBar/Camera/ResetNearClipPlane")]
        static void ResetNearClipPlane()
        {
            //SceneView.lastActiveSceneView.camera.fieldOfView = 0.1f;
            SceneView.lastActiveSceneView.camera.nearClipPlane = 0.005f;

            SceneView.lastActiveSceneView.Repaint();
        }
    }
}
#endif
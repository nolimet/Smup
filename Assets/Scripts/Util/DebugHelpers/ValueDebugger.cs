using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Util.DebugHelpers
{
    /// <summary>
    /// On screen debugger usefull when working with a game build but you want to do some error tracking
    /// </summary>
    public class ValueDebugger : MonoBehaviour
    {
        private static ValueDebugger _instance;

        protected Dictionary<string, object> Values;
        protected Text T;

        /// <summary>
        /// Value that will be logged. 
        /// Also create the object that will be rendered onscreen completely by code so it does not need a prefab
        /// </summary>
        /// <param name="name">Value's name so you can find it back</param>
        /// <param name="value">Value of the object</param>
        public static void ValueLog(string name, object value)
        {
            if (_instance == null)
                // if it just lost it's instance
                _instance = FindAnyObjectByType<ValueDebugger>();

            if (_instance == null)
            {
                //Make object if it does not exist
                //Canvas 
                var g = new GameObject();

                var c = g.AddComponent<Canvas>();
                c.renderMode = RenderMode.ScreenSpaceOverlay;
                c.sortingOrder = 7000;

                var sc = g.AddComponent<CanvasScaler>();
                sc.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                sc.referenceResolution = new Vector2(1600, 900);

                //text Display
                var g2 = new GameObject();
                g2.transform.SetParent(g.transform, false);

                var rt = g2.AddComponent<RectTransform>();
                rt.anchorMax = new Vector2(1f, 1f);
                rt.anchorMin = new Vector2(0.5f, 0);
                rt.sizeDelta = new Vector2(-20, -40);

                rt.anchoredPosition = new Vector2(-20, 0);

                var t = g2.AddComponent<Text>();
                t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                t.color = Color.green;
                t.fontSize = 20;
                g2.AddComponent<ValueDebugger>();

                //background Image
                var g3 = new GameObject();
                g3.transform.SetParent(g.transform, false);
                g3.transform.SetAsFirstSibling();

                rt = g3.AddComponent<RectTransform>();
                rt.anchorMax = new Vector2(1f, 1f);
                rt.anchorMin = new Vector2(0.5f, 0);
                rt.sizeDelta = new Vector2(-20, -40);

                rt.anchoredPosition = new Vector2(-20, 0);

                var I = g3.AddComponent<Image>();
                I.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);

                g.name = "util.DebugVisual";
            }

            if (_instance && !_instance.transform.parent.gameObject.activeSelf)
                _instance.transform.parent.gameObject.SetActive(true);

            if (_instance.Values.Keys.Contains(name))
                _instance.Values[name] = value;
            else
                _instance.Values.Add(name, value);
        }

        private void Awake()
        {
            _instance = this;
            T = GetComponent<Text>();
            Values = new Dictionary<string, object>();
        }

        private void Update()
        {
            if (!Debugger.DebugEnabled)
                transform.parent.gameObject.SetActive(false);

            Process();
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        private string _ts;

        /// <summary>
        /// create the string that will be displayed on screen
        /// </summary>
        private void Process()
        {
            _ts = "";

            foreach (var vs in Values) _ts += vs.Key + " : " + vs.Value.ToString() + " \n";

            T.text = _ts;
        }
    }

    /// <summary>
    /// On screen debugger call class
    /// </summary>
    public static class Debugger
    {
        public static bool DebugEnabled = true;

        /// <summary>
        /// Value that will be logged. 
        /// Also create the object that will be rendered onscreen completely by code so it does not need a prefab
        /// </summary>
        /// <param name="name">Value's name so you can find it back</param>
        /// <param name="value">Value of the object</param>
        public static void Log(string name, object value)
        {
            if (DebugEnabled)
                ValueDebugger.ValueLog(name, value);
        }

        public static void QuickLog<T>(this T value, string name)
        {
            if (DebugEnabled)
                ValueDebugger.ValueLog(name, value);
        }
    }
}

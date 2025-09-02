using UnityEngine;
using UnityEngine.Serialization;

namespace Util.DebugHelpers
{
    public class DebugGrid : MonoBehaviour
    {
        public int height, width;

        [FormerlySerializedAs("A")] public Vector2 a;
        [FormerlySerializedAs("B")] public Vector2 b;
        [FormerlySerializedAs("C")] public Vector2 c;

        [FormerlySerializedAs("K")] [SerializeField] private float k = 1;
        [FormerlySerializedAs("L")] [SerializeField] private float l = 1;
        [SerializeField] private float j = 1;

        public enum Modes
        {
            Vectoradd,
            VectorTriangle,
            Line
        }

        [FormerlySerializedAs("Mode")] public Modes mode;

        private bool _state;
        [FormerlySerializedAs("MaxDelay")] public float maxDelay;
        private float _delay = 1f;

        public Transform[] points;

        // Use this for initialization
        private void Start()
        {
            points[0].name = "A";
            points[1].name = "B";
            points[2].name = "C";
            points[3].name = "AB";
            points[4].name = "BC";
            points[5].name = "CA";
        }

        // Update is called once per frame
        private void Update()
        {
            var k = 0;
            if (_delay >= maxDelay)
            {
                for (var i = 0; i < height + 1; i++) Debug.DrawLine(new Vector3(-width / 2f, i - height / 2f, k), new Vector3(width / 2f, i - height / 2f, k), Color.white, maxDelay);
                for (var j = 0; j < width + 1; j++) Debug.DrawLine(new Vector3(j - width / 2f, height / -2f, k), new Vector3(j - width / 2f, height / 2f, k), Color.gray, maxDelay);
                _delay = 0;

                if (mode == Modes.Vectoradd)
                {
                    _state = false;
                    var ant = a + b;

                    Debug.DrawLine(Vector3.zero, ant, Color.blue, maxDelay);
                    Debug.DrawLine(Vector3.zero, a, Color.green, maxDelay);
                    Debug.DrawLine(Vector3.zero, b, Color.magenta, maxDelay);
                    Debug.DrawLine(a, ant, Color.red, maxDelay);
                    Debug.DrawLine(b, ant, Color.red, maxDelay);
                    points[0].position = a;
                    points[1].position = b;
                    points[2].position = ant;

                    points[0].name = "A" + a;
                    points[1].name = "B" + b;

                    points[2].name = "C" + new Vector2(points[2].position.x, points[2].position.y);
                }
                else if (mode == Modes.VectorTriangle)
                {
                    _state = true;
                    //Setting positions
                    points[0].position = a;
                    points[1].position = b;
                    points[2].position = c;
                    points[3].position = a * 0.5f + b * 0.5f;
                    points[4].position = b * 0.5f + c * 0.5f;
                    points[5].position = c * 0.5f + a * 0.5f;

                    //Drawing lines

                    //A
                    Debug.DrawLine(points[0].position, points[3].position, Color.blue, maxDelay);
                    Debug.DrawLine(points[0].position, points[4].position, Color.red, maxDelay);
                    Debug.DrawLine(points[0].position, points[5].position, Color.magenta, maxDelay);
                    //B
                    Debug.DrawLine(points[1].position, points[3].position, Color.blue, maxDelay);
                    Debug.DrawLine(points[1].position, points[4].position, Color.yellow, maxDelay);
                    Debug.DrawLine(points[1].position, points[5].position, Color.red, maxDelay);
                    //C
                    Debug.DrawLine(points[2].position, points[3].position, Color.red, maxDelay);
                    Debug.DrawLine(points[2].position, points[4].position, Color.yellow, maxDelay);
                    Debug.DrawLine(points[2].position, points[5].position, Color.magenta, maxDelay);

                    //renaming
                    points[0].name = "A" + a;
                    points[1].name = "B" + b;
                    points[2].name = "C" + c;
                }
                else if (mode == Modes.Line)
                {
                    _state = true;
                    points[3].position = Vector2.zero;
                    points[4].position = new Vector2(LineX(20) / 5f, LineY(10) / 5f);
                    for (var i = 0; i < points.Length; i++)
                    {
                        points[i].name = " ";
                        if (i == 3) points[i].name = "A";
                        if (i == 4) points[i].name = "B";
                    }
                }

                points[3].gameObject.SetActive(_state);
                points[4].gameObject.SetActive(_state);
                points[5].gameObject.SetActive(_state);
            }

            _delay += Time.deltaTime;
        }

        public static Color RandomColor()
        {
            // float numb = 0.0039215686274509803921568627451f;
            var output = new Color();
            output.b = 1 * Random.value;
            output.g = 1 * Random.value;
            output.r = 1 * Random.value;
            output.a = 1;

            return output;
        }

        private float LineY(float x) => (j - k * x) / l;

        private float LineX(float x) => (j - l * x) / k;

        private void OnGUI() { }
    }
}

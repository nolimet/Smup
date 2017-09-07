using UnityEngine;
using System.Collections;

namespace Util.Debugger
{
    public class DebugGrid : MonoBehaviour
    {

        public int height, width;

        public Vector2 A;
        public Vector2 B;
        public Vector2 C;

        [SerializeField]
        float K = 1;
        [SerializeField]
        float L = 1;
        [SerializeField]
        float j = 1;

        public enum modes
        {
            Vectoradd,
            VectorTriangle,
            line
        }
        public modes Mode;

        bool state;
        public float MaxDelay;
        float delay = 1f;

        public Transform[] points;

        // Use this for initialization
        void Start()
        {
            points[0].name = "A";
            points[1].name = "B";
            points[2].name = "C";
            points[3].name = "AB";
            points[4].name = "BC";
            points[5].name = "CA";
        }

        // Update is called once per frame
        void Update()
        {

            int k = 0;
            if (delay >= MaxDelay)
            {
                for (int i = 0; i < height + 1; i++)
                {
                    Debug.DrawLine(new Vector3((-width / 2f), i - (height / 2f), k), new Vector3(width / 2f, i - (height / 2f), k), Color.white, MaxDelay);
                }
                for (int j = 0; j < width + 1; j++)
                {
                    Debug.DrawLine(new Vector3(j - (width / 2f), height / -2f, k), new Vector3(j - (width / 2f), height / 2f, k), Color.gray, MaxDelay);
                }
                delay = 0;


                if (Mode == modes.Vectoradd)
                {
                    state = false;
                    Vector2 ant = A + B;

                    Debug.DrawLine(Vector3.zero, ant, Color.blue, MaxDelay);
                    Debug.DrawLine(Vector3.zero, A, Color.green, MaxDelay);
                    Debug.DrawLine(Vector3.zero, B, Color.magenta, MaxDelay);
                    Debug.DrawLine(A, ant, Color.red, MaxDelay);
                    Debug.DrawLine(B, ant, Color.red, MaxDelay);
                    points[0].position = A;
                    points[1].position = B;
                    points[2].position = ant;

                    points[0].name = "A" + A;
                    points[1].name = "B" + B;

                    points[2].name = "C" + new Vector2(points[2].position.x, points[2].position.y);
                }
                else if (Mode == modes.VectorTriangle)
                {
                    state = true;
                    //Setting positions
                    points[0].position = A;
                    points[1].position = B;
                    points[2].position = C;
                    points[3].position = A * 0.5f + B * 0.5f;
                    points[4].position = B * 0.5f + C * 0.5f;
                    points[5].position = C * 0.5f + A * 0.5f;

                    //Drawing lines

                    //A
                    Debug.DrawLine(points[0].position, points[3].position, Color.blue, MaxDelay);
                    Debug.DrawLine(points[0].position, points[4].position, Color.red, MaxDelay);
                    Debug.DrawLine(points[0].position, points[5].position, Color.magenta, MaxDelay);
                    //B
                    Debug.DrawLine(points[1].position, points[3].position, Color.blue, MaxDelay);
                    Debug.DrawLine(points[1].position, points[4].position, Color.yellow, MaxDelay);
                    Debug.DrawLine(points[1].position, points[5].position, Color.red, MaxDelay);
                    //C
                    Debug.DrawLine(points[2].position, points[3].position, Color.red, MaxDelay);
                    Debug.DrawLine(points[2].position, points[4].position, Color.yellow, MaxDelay);
                    Debug.DrawLine(points[2].position, points[5].position, Color.magenta, MaxDelay);

                    //renaming
                    points[0].name = "A" + A;
                    points[1].name = "B" + B;
                    points[2].name = "C" + C;
                }
                else if (Mode == modes.line)
                {
                    state = true;
                    points[3].position = Vector2.zero;
                    points[4].position = new Vector2(LineX(20) / 5f, lineY(10) / 5f);
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i].name = " ";
                        if (i == 3)
                        {
                            points[i].name = "A";
                        }
                        if (i == 4)
                        {
                            points[i].name = "B";
                        }

                    }
                }

                points[3].gameObject.SetActive(state);
                points[4].gameObject.SetActive(state);
                points[5].gameObject.SetActive(state);
            }
            delay += Time.deltaTime;
        }
        public static Color randomColor()
        {
            // float numb = 0.0039215686274509803921568627451f;
            Color output = new Color();
            output.b = 1 * Random.value;
            output.g = 1 * Random.value;
            output.r = 1 * Random.value;
            output.a = 1;

            return output;
        }
        float lineY(float x)
        {
            return (j - K * x) / L;
        }

        float LineX(float x)
        {
            return (j - L * x) / K;

        }
        void OnGUI()
        {
        }
    }
}
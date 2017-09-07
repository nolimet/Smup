using UnityEngine;
using System.Collections;

public class MoveBoxScaler : MonoBehaviour {

    [SerializeField]
    new Camera camera;

    public Vector3 screenSize = Vector3.zero;
    void Awake()
    {
        Vector3 p1 = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.localScale = new Vector3(p1.x, p1.y, 1) * 2f;
        screenSize = new Vector3(p1.x, p1.y, 1) * 2f;
    }
}

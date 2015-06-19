using UnityEngine;
using System.Collections;

public class MoveBoxScaler : MonoBehaviour {

    [SerializeField]
    Camera camera;

    void Awake()
    {
        Vector3 p1 = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));


        transform.localScale = new Vector3(p1.x, p1.y, 1) * 2f;
    }
}

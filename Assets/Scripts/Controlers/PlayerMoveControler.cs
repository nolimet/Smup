using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveControler : MonoBehaviour {

    const float speed = 6f;
    Rigidbody2D rig;
	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 dir = Vector2.zero;

        if (Input.GetAxis(Axis.Horizontal) > 0)
            dir.x = speed * Input.GetAxis(Axis.Horizontal);
        if (Input.GetAxis(Axis.Horizontal) < 0)
            dir.x = speed * 0.5f * Input.GetAxis(Axis.Horizontal);

        if (Input.GetAxis(Axis.Vertical) != 0)
            dir.y =  speed * 0.75f * Input.GetAxis(Axis.Vertical);

        rig.velocity = dir;
	}
}

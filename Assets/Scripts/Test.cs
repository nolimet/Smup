using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {


    public interfaces.Move.IMovement Move;
	// Use this for initialization
	void Start () {
        Move = new interfaces.Move.LinearMovement();
        Debug.Log(Move);
	}
	
	// Update is called once per frame
	void Update () {
            Move.Move(gameObject);
            Debug.Log("MOVE");
	}
}

using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public player.Weapon.IBaseWeapon gun;
	// Use this for initialization
	void Start () {
        //Move = new interfaces.Move.LinearMovement();
        gun = new player.Weapon.Cannon();
	}
	
	// Update is called once per frame
	void Update () {
        //Move.Move(gameObject);
        gun.Shoot(gameObject, new Vector3(0.5f, 0), Vector2.zero);
	}
}

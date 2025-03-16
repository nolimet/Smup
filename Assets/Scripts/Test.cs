using Player.Weapons;
using UnityEngine;

public class Test : MonoBehaviour
{
    public IBaseWeapon Gun;

    // Use this for initialization
    private void Start()
    {
        //Move = new interfaces.Move.LinearMovement();
        Gun = new Cannon();
    }

    // Update is called once per frame
    private void Update()
    {
        //Move.Move(gameObject);
        Gun.TryShoot(gameObject, new Vector3(0.5f, 0), Vector2.zero);
    }
}

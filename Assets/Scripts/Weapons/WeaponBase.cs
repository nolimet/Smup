using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class WeaponBase : MonoBehaviour {

    public WeaponTable.Weapons WeaponType;
    protected Vector2 moveDir;
    protected float speed, damage;
    protected Rigidbody2D rigi;
    /// <summary>
    /// Sets the paramaters that the bullet will use to move around
    /// </summary>
    /// <param name="moveDirNormal">The move direction clamped between 1 & -1</param>
    /// <param name="speed">How quickly it will move</param>
    /// <param name="damage">The amount of damage that will be done on impact</param>
    public virtual void Init(Vector2 moveDirNormal, float speed, float damage)
    {
        this.moveDir = moveDirNormal;
        this.speed = speed;
        this.damage = damage;

        if(!rigi)
            rigi = GetComponent<Rigidbody2D>();

        rigi.velocity = moveDir * speed;
    }

    protected virtual void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        
    }
}

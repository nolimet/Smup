using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectileGeneric : MonoBehaviour {

    public WeaponTable.Weapons WeaponType;
    protected Vector2 moveDir;
    protected float speed, damage;
    protected Rigidbody2D rigi;
    protected bool markedForRemove;

    /// <summary>
    /// Sets the paramaters that the bullet will use to move around
    /// </summary>
    /// <param name="moveDirNormal">The move direction clamped between 1 & -1</param>
    /// <param name="speed">How quickly it will move</param>
    /// <param name="damage">The amount of damage that will be done on impact</param>
    /// <param name="addVelo">Add Exstra velocity from for example the player or enemies</param>
    public virtual void Init(Vector2 addVelo,Vector2 moveDirNormal, float speed, float damage)
    {
        this.moveDir = moveDirNormal;
        this.speed = speed;
        this.damage = damage;

        if(!rigi)
            rigi = GetComponent<Rigidbody2D>();

        rigi.velocity = (moveDir * speed) + addVelo;
    }

    protected virtual void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnEnable()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
         GetComponent<SpriteRenderer>().color = Color.white;
        markedForRemove = false;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (!markedForRemove)
        {
            //  rigi.AddTorque(Random.Range(-10, 10));
            StartCoroutine(Remove(0.5f));
            coll.gameObject.SendMessage("hit", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!markedForRemove)
            StartCoroutine(Remove(0f));
       
    }

    IEnumerator Remove(float delay)
    {
        if (!markedForRemove)
        {
            const float frag = 1f / 30;
            SpriteRenderer r = GetComponent<SpriteRenderer>();
            Color StartColor = r.color;
            Color TargetColor = r.color;

            TargetColor.a = 0;
            markedForRemove = true;

            yield return new WaitForSeconds(delay);
            GetComponent<PolygonCollider2D>().enabled = false;

            for (int i = 0; i < 30; i++)
            {
                r.color = Color.Lerp(StartColor, TargetColor, frag * i);
                yield return new WaitForEndOfFrame();
            }

           // BulletPool.RemoveBullet(this);
            
        }
    }
}

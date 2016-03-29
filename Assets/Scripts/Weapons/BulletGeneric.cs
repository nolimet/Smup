using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
public class BulletGeneric : MonoBehaviour
{
    public enum Type
    {
        Fragment,
        Bullet
    }

    public Type WeaponType;

    new public Rigidbody2D rigidbody;

    int fragmentsExplosion;

    float damage;
    public float Damage
    {
        get { return damage; }
    }

    protected bool markedForRemove;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
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

    public void Init(float damage, float direction, float speed)
    {
        this.damage = damage;

        transform.rotation = Quaternion.Euler(0, 0, direction);

        rigidbody.velocity = util.MathHelper.AngleToVector(direction) * speed;

        fragmentsExplosion = 0;
    }

    public void Init(float damage, float direction, float speed, int fragmentsExplosion)
    {
        Init(damage, direction, speed);

        this.fragmentsExplosion = fragmentsExplosion;
    }

    void explode()
    {
        if (fragmentsExplosion <= 0)
            return;

        while (fragmentsExplosion > 0)
        {
            fragmentsExplosion--;

            //TODO: Make them spread out and explode and stuff;
        }
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

            BulletPool.RemoveBullet(this);

        }
    }
}
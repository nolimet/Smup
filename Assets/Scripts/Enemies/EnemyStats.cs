using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyStats : MonoBehaviour {

    public enum Type
    {
        shooter = 0,
        WaveMover = 1,
        SlowDownSpeedUp = 2
        //ToDo add More types
    }

    [SerializeField]
    float health = 10, maxHealth = 10;
    public Type type;
    bool markedForRemove = false;
    public virtual void hit(float dmg)
    {
        health -= dmg;
        if (health <= 0 && !markedForRemove)
            StartCoroutine(Remove(0f));
    }

    protected virtual void OnEnable()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<PolygonCollider2D>().enabled = true;
        markedForRemove = false;
        health = maxHealth;
    }

    IEnumerator Remove(float delay)
    {
        if (!markedForRemove)
        {
            GetComponent<PolygonCollider2D>().enabled = false;

            const float frag = 1f / 30;
            SpriteRenderer r = GetComponent<SpriteRenderer>();
            Color StartColor = r.color;
            Color TargetColor = r.color;

            TargetColor.a = 0;
            markedForRemove = true;

            yield return new WaitForSeconds(delay);

            for (int i = 0; i < 30; i++)
            {
                r.color = Color.Lerp(StartColor, TargetColor, frag * i);
                yield return new WaitForEndOfFrame();
            }

            GetComponent<Rigidbody2D>().isKinematic = true;
            EnemyPool.RemoveEnemy(this);
        }
    }
}

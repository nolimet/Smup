using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyStats : MonoBehaviour {

    public enum Type
    {
        shooter,
        WaveMover,
        SlowDownSpeedUp
        //ToDo add More types
    }

    [SerializeField]
    float health;
    public Type type;
    public void hit(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            remove();
    }

    void remove()
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyStats : MonoBehaviour {

    [SerializeField]
    float health;
    public void hit(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            remove();
    }

    void remove()
    {
        Destroy(this.gameObject, 1f);
    }
}

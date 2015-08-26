using UnityEngine;
using util;
using System.Collections;

public class PickupManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay2D(Collider2D col)
    {
        Rigidbody2D t = col.GetComponent<Rigidbody2D>();
        float v = t.velocity.v();
        if (v < 0) v *= -1;

        if (v < 20)
        {
            Vector2 v2 = transform.position - col.transform.position;
            v2.Normalize();
            t.AddForce(v2 * 5f);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        col.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        PickupPool.removePickup(col.gameObject.GetComponent<Pickup>());
        Debug.Log(col.gameObject.name);
    }
}

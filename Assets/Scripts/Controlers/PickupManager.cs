using UnityEngine;
using util;
using System.Collections;

public class PickupManager : MonoBehaviour
{
    [SerializeField]
    float pickedUpScrap = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        UpgradeData dat = new UpgradeData();
        Serialization.Load("upgrade", Serialization.fileTypes.binary, ref dat);

        dat.UpgradeCurrency += Mathf.FloorToInt(pickedUpScrap * ((dat.ScrapConversionRate + 1) * 1.1f));

        Serialization.Save("upgrade", Serialization.fileTypes.binary, dat);
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 14)
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
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 14)
        {
            col.GetComponent<Rigidbody2D>().drag = 5f;
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 14)
        {
            col.GetComponent<Rigidbody2D>().drag = 1f;
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 14)
        {
            PickupPool.removePickup(col.gameObject.GetComponent<Pickup>());
            pickedUpScrap += col.gameObject.GetComponent<Pickup>().scrapValue;
        }
    }
}

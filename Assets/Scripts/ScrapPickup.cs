using UnityEngine;
using System.Collections;

public class ScrapPickup : MonoBehaviour
{

    //ToDo this is a temp value only for testing and for seeing if it works correct
    public float scrapValue;
    SpriteRenderer spr;
    /// <summary>
    /// flo is used for animations
    /// lifetime does what it says is the time it has left to live
    /// A is alpha of the object
    /// </summary>
    float flo = 0f, lifeTime = 120f, A = 0;
    bool b;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        flo = Random.Range(0, 1f);
        A = Random.Range(0, 0.3f);
    }

    public void OnEnable()
    {
        A = Random.Range(0, 0.3f);
    }

    public virtual void init(int l, float Value = 1f)
    {
        lifeTime = l;
        scrapValue = Value;

        transform.localScale = Vector3.one * (Mathf.Pow(1.1f, Value / 10f) - 0.6f);
    }

    void Update()
    {

        Color r;
        if (b)
            r = Color.Lerp(Color.white, Color.gray, flo);
        else
            r = Color.Lerp(Color.gray, Color.white, flo);
        if(A<=1)
        {
            r.a = A;
            A += Time.deltaTime * 3f;
        }
        spr.color = r;

        if (flo >= 1)
        {
            flo = 0;
            b = !b;
        }
        flo += Time.deltaTime / 2f;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            PickupPool.removePickup(this);
        }
    }
}

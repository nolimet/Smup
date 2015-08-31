using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{

    //ToDo this is a temp value only for testing and for seeing if it works correct
    public float scrapValue;
    SpriteRenderer spr;
    float flo = 0f, lifeTime = 10f, A = 0;
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

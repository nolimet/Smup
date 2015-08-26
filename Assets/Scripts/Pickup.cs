using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	SpriteRenderer spr;
    float flo = 0f;
    bool b;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        flo = Random.Range(0, 1f);
    }

    void Update()
    {

        if (b)
            spr.color = Color.Lerp(Color.white, Color.gray, flo);
        else
            spr.color = Color.Lerp(Color.gray, Color.white, flo);

        if (flo >= 1)
        {
            flo = 0;
            b = !b;
        }
        flo += Time.deltaTime / 2f;
    }
}

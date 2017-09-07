using UnityEngine;
using Util;
using System.Collections;

public class PlayerThrusterEffect : MonoBehaviour {

    [SerializeField]
    ParticleSystem[] Up = null, Down = null, Left = null, Right = null;

    void Update()
    {
        UpdateThursters(new Vector2(Input.GetAxis(Axis.Horizontal), Input.GetAxis(Axis.Vertical)));
    }

    void UpdateThursters(Vector2 dir)
    {
        //ToDo figure out how the new way works or changing emissionRates
        #region dir.y
        if (dir.y < -0.05)
        {
            for (int i = 0; i < Up.Length; i++)
            {
                Up[i].SetEmissionRate(10f);
                Up[i].SetEmissionRate(10f);
            }
            for (int i = 0; i < Down.Length; i++)
			{
                Down[i].SetEmissionRate(0);
            }
        }
        else if(dir.y > 0.05)
        {
            for (int i = 0; i < Up.Length; i++)
            {
                Up[i].SetEmissionRate(0);
                
            }

            for (int i = 0; i < Down.Length; i++)
			{
			    Down[i].SetEmissionRate(10f);

			}
        }
        else
        {
            for (int i = 0; i < Up.Length; i++)
            {
                Up[i].SetEmissionRate(0);

            }

            for (int i = 0; i < Down.Length; i++)
            {
                Down[i].SetEmissionRate(0);
            }
        }
        #endregion
        #region dir.x
        if (dir.x < -0.05)
        {
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i].SetEmissionRate(10f * -dir.x);
            }
            for (int i = 0; i < Right.Length; i++)
            {
                Right[i].SetEmissionRate(0);
            }
        }
        else if (dir.x > 0.05)
        {
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i].SetEmissionRate(0);

            }

            for (int i = 0; i < Right.Length; i++)
            {
                Right[i].SetEmissionRate(10f * dir.x);
            }
        }
        else
        {
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i].SetEmissionRate(0);

            }

            for (int i = 0; i < Right.Length; i++)
            {
                Right[i].SetEmissionRate(0);
            }
        }
        #endregion
    }
}

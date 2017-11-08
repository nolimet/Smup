using UnityEngine;
using Util;
using System.Collections;

public class PlayerThrusterEffect : MonoBehaviour {

    [SerializeField]
    ParticleSystem[] Up = null, Down = null, Left = null, Right = null;
   
    [SerializeField]
    ParticleSystem[] BoostUp = null, BoostDown = null, BoostLeft = null, BoostRight = null;

    int i;
    void Update()
    {
        if (Input.GetAxis(Axis.Boost) == 0)
        {
            UpdateThursters(new Vector2(Input.GetAxis(Axis.Horizontal), Input.GetAxis(Axis.Vertical)));
            UpdateBoostThursters(Vector2.zero);
        }
        else
        {
            UpdateBoostThursters(new Vector2(Input.GetAxis(Axis.Horizontal), Input.GetAxis(Axis.Vertical)));
            UpdateThursters(Vector2.zero);
        }
    }

    void UpdateThursters(Vector2 dir)
    {
        //ToDo figure out how the new way works or changing emissionRates
        #region dir.y
        if (dir.y < -0.05)
        {
            for (i = 0; i < Up.Length; i++)
            {
                Up[i].SetEmissionRate(10f);
                Up[i].SetEmissionRate(10f);
            }
            for (i = 0; i < Down.Length; i++)
			{
                Down[i].SetEmissionRate(0);
            }
        }
        else if(dir.y > 0.05)
        {
            for (i = 0; i < Up.Length; i++)
            {
                Up[i].SetEmissionRate(0);
                
            }

            for (i = 0; i < Down.Length; i++)
			{
			    Down[i].SetEmissionRate(10f);

			}
        }
        else
        {
            for (i = 0; i < Up.Length; i++)
            {
                Up[i].SetEmissionRate(0);

            }

            for (i = 0; i < Down.Length; i++)
            {
                Down[i].SetEmissionRate(0);
            }
        }
        #endregion
        #region dir.x
        if (dir.x < -0.05)
        {
            for (i = 0; i < Left.Length; i++)
            {
                Left[i].SetEmissionRate(10f * -dir.x);
            }
            for (i = 0; i < Right.Length; i++)
            {
                Right[i].SetEmissionRate(0);
            }
        }
        else if (dir.x > 0.05)
        {
            for (i = 0; i < Left.Length; i++)
            {
                Left[i].SetEmissionRate(0);

            }

            for (i = 0; i < Right.Length; i++)
            {
                Right[i].SetEmissionRate(10f * dir.x);
            }
        }
        else
        {
            for (i = 0; i < Left.Length; i++)
            {
                Left[i].SetEmissionRate(0);

            }

            for (i = 0; i < Right.Length; i++)
            {
                Right[i].SetEmissionRate(0);
            }
        }
        #endregion
    }
    void UpdateBoostThursters(Vector2 dir)
    {
        //ToDo figure out how the new way works or changing emissionRates
        #region dir.y
        if (dir.y < -0.05)
        {
            for (i = 0; i < Up.Length; i++)
            {
                BoostUp[i].SetEmissionRate(10f);
                BoostUp[i].SetEmissionRate(10f);
            }
            for (i = 0; i < Down.Length; i++)
            {
                BoostDown[i].SetEmissionRate(0);
            }
        }
        else if (dir.y > 0.05)
        {
            for (i = 0; i < Up.Length; i++)
            {
                BoostUp[i].SetEmissionRate(0);

            }

            for (i = 0; i < Down.Length; i++)
            {
                BoostDown[i].SetEmissionRate(10f);

            }
        }
        else
        {
            for (i = 0; i < Up.Length; i++)
            {
                BoostUp[i].SetEmissionRate(0);

            }

            for (i = 0; i < Down.Length; i++)
            {
                BoostDown[i].SetEmissionRate(0);
            }
        }
        #endregion
        #region dir.x
        if (dir.x < -0.05)
        {
            for (i = 0; i < Left.Length; i++)
            {
                BoostLeft[i].SetEmissionRate(10f * -dir.x);
            }
            for (i = 0; i < Right.Length; i++)
            {
                BoostRight[i].SetEmissionRate(0);
            }
        }
        else if (dir.x > 0.05)
        {
            for (i = 0; i < Left.Length; i++)
            {
                BoostLeft[i].SetEmissionRate(0);

            }

            for (i = 0; i < Right.Length; i++)
            {
                BoostRight[i].SetEmissionRate(10f * dir.x);
            }
        }
        else
        {
            for (i = 0; i < Left.Length; i++)
            {
                BoostLeft[i].SetEmissionRate(0);

            }

            for (i = 0; i < Right.Length; i++)
            {
                BoostRight[i].SetEmissionRate(0);
            }
        }
        #endregion
    }
}

using UnityEngine;
using System.Collections;

public class PlayerThrusterEffect : MonoBehaviour {

    [SerializeField]
    ParticleSystem[] Up, Down, Left, Right;

    void Update()
    {
        UpdateThursters(new Vector2(Input.GetAxis(Axis.Horizontal), Input.GetAxis(Axis.Vertical)));
    }

    void UpdateThursters(Vector2 dir)
    {
        #region dir.y
        if (dir.y < -0.8)
        {
            for (int i = 0; i < Up.Length; i++)
            {
                Up[i].emissionRate = 10f;
            }
            for (int i = 0; i < Down.Length; i++)
			{
                Down[i].emissionRate = 0;
            }
        }
        else if(dir.y > 0.8)
        {
            for (int i = 0; i < Up.Length; i++)
            {
                Up[i].emissionRate = 0;
                
            }

            for (int i = 0; i < Down.Length; i++)
			{
			    Down[i].emissionRate = 10f;
			}
        }
        else
        {
            for (int i = 0; i < Up.Length; i++)
            {
                Up[i].emissionRate = 0;

            }

            for (int i = 0; i < Down.Length; i++)
            {
                Down[i].emissionRate = 0;
            }
        }
        #endregion
        #region dir.x
        if (dir.x < -0.1)
        {
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i].emissionRate = 10f * -dir.x;
            }
            for (int i = 0; i < Right.Length; i++)
            {
                Right[i].emissionRate = 0;
            }
        }
        else if (dir.x > 0.1)
        {
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i].emissionRate = 0;

            }

            for (int i = 0; i < Right.Length; i++)
            {
                Right[i].emissionRate = 10f * dir.x;
            }
        }
        else
        {
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i].emissionRate = 0;

            }

            for (int i = 0; i < Right.Length; i++)
            {
                Right[i].emissionRate = 0;
            }
        }
        #endregion
    }
}

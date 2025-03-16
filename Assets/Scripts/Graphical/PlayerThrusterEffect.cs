using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Graphical
{
    public class PlayerThrusterEffect : MonoBehaviour
    {
        [FormerlySerializedAs("Up")] [SerializeField] private ParticleSystem[] up;
        [FormerlySerializedAs("Down")] [SerializeField] private ParticleSystem[] down;
        [FormerlySerializedAs("Left")] [SerializeField] private ParticleSystem[] left;
        [FormerlySerializedAs("Right")] [SerializeField] private ParticleSystem[] right;

        [FormerlySerializedAs("BoostUp")] [SerializeField] private ParticleSystem[] boostUp;
        [FormerlySerializedAs("BoostDown")] [SerializeField] private ParticleSystem[] boostDown;
        [FormerlySerializedAs("BoostLeft")] [SerializeField] private ParticleSystem[] boostLeft;
        [FormerlySerializedAs("BoostRight")] [SerializeField] private ParticleSystem[] boostRight;

        private void Update()
        {
            var dir = new Vector2(Input.GetAxis(Axis.Horizontal), Input.GetAxis(Axis.Vertical));
            if (Input.GetAxis(Axis.Boost) == 0)
            {
                UpdateThursters(dir);
                UpdateBoostThursters(Vector2.zero);
            }
            else
            {
                UpdateBoostThursters(dir);
                UpdateThursters(Vector2.zero);
            }
        }

        private void UpdateThursters(Vector2 dir)
        {
            var dirY = Mathf.Max(0, Mathf.Abs(dir.y) - 0.05f) * Mathf.Sign(dir.y) * 10;
            SetDirection(up, Mathf.Max(0, -dirY));
            SetDirection(down, Mathf.Max(0, dirY));

            var dirX = Mathf.Max(0, Mathf.Abs(dir.x) - 0.05f) * Mathf.Sign(dir.x) * 10;
            SetDirection(left, Mathf.Max(0, -dirX));
            SetDirection(right, Mathf.Max(0, dirX));
        }

        private void UpdateBoostThursters(Vector2 dir)
        {
            //ToDo figure out how the new way works or changing emissionRates

            var dirY = Mathf.Max(0, Mathf.Abs(dir.y) - 0.05f) * Mathf.Sign(dir.y) * 10;
            SetDirection(boostUp, Mathf.Max(0, -dirY));
            SetDirection(boostDown, Mathf.Max(0, dirY));

            var dirX = Mathf.Max(0, Mathf.Abs(dir.x) - 0.05f) * Mathf.Sign(dir.x) * 10;
            SetDirection(boostLeft, Mathf.Max(0, -dirX));
            SetDirection(boostRight, Mathf.Max(0, dirX));
        }

        private static void SetDirection(ParticleSystem[] sys, float rate)
        {
            foreach (var par in sys) par.SetEmissionRate(rate);
        }
    }
}

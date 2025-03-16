using System;
using UnityEngine;

namespace Enemies
{
    public interface IMovement
    {
        void Move(GameObject enity, Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f);
    }

    public class LinearMovement : IMovement
    {
        private Rigidbody2D _ri;

        public void Move(GameObject enity, Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f)
        {
            _ri = enity.GetComponent<Rigidbody2D>();

            if (_ri.linearVelocity.x < -10f)
                _ri.AddForce(new Vector2(-5 * _ri.mass * speed, 0), ForceMode2D.Force);
        }
    }

    public class UpDownWaveMove : IMovement
    {
        private const float Amplitude = 1f;
        private const float Frequency = 2f;

        private Rigidbody2D _ri;
        private float _yMovement;

        public void Move(GameObject enity, Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f)
        {
            _ri = enity.AddComponent<Rigidbody2D>();

            _yMovement = Amplitude * (Mathf.Sin(2 * Mathf.PI * Frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * Frequency * (Time.time - Time.deltaTime)));

            _ri.AddForce(new Vector2(Time.deltaTime * 10f, _yMovement) * _ri.mass);
            //enity.transform.Translate(Time.deltaTime * 10f, yMovement, 0f);
        }
    }

    public class MoveToLocation : IMovement
    {
        public void Move(GameObject enity, Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f)
        {
            throw new NotImplementedException();
        }
    }
}

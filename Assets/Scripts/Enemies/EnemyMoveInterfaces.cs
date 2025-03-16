using System;
using UnityEngine;

namespace Enemies
{
    public interface IMovement
    {
        void Move(Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f);
    }

    public class LinearMovement : IMovement
    {
        private readonly Rigidbody2D _rigidbody;

        public LinearMovement(GameObject enity)
        {
            _rigidbody = enity.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f)
        {
            if (_rigidbody.linearVelocity.x < -10f)
                _rigidbody.AddForce(new Vector2(-5 * _rigidbody.mass * speed, 0), ForceMode2D.Force);
        }
    }

    public class UpDownWaveMove : IMovement
    {
        private const float Amplitude = 1f;
        private const float Frequency = 2f;

        private readonly Rigidbody2D _rigidbody;
        private float _yMovement;

        public UpDownWaveMove(GameObject enity)
        {
            _rigidbody = enity.AddComponent<Rigidbody2D>();
        }

        public void Move(Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f)
        {
            _yMovement = Amplitude * (Mathf.Sin(2 * Mathf.PI * Frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * Frequency * (Time.time - Time.deltaTime)));

            _rigidbody.AddForce(new Vector2(Time.deltaTime * 10f, _yMovement) * _rigidbody.mass);
            //enity.transform.Translate(Time.deltaTime * 10f, yMovement, 0f);
        }
    }

    public class MoveToLocation : IMovement
    {
        private Rigidbody2D _rigidbody;

        public MoveToLocation(GameObject enity)
        {
            _rigidbody = enity.AddComponent<Rigidbody2D>();
        }

        public void Move(Vector2? movelocation = null, float speed = 1f, float moveDelay = 0f)
        {
            throw new NotImplementedException();
        }
    }
}

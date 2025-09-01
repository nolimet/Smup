using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemies
{
    public interface IMovement
    {
        void Move(Vector2? currentPosition = null, float speed = 1f, float deltaTime = 0f);
        void SetTarget(GameObject entity);
    }

    [Serializable]
    public class LinearMovement : IMovement
    {
        private Rigidbody2D _rigidbody;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2? currentPosition = null, float speed = 1f, float deltaTime = 0f)
        {
            if (_rigidbody.linearVelocity.x < -10f)
                _rigidbody.AddForce(new Vector2(-5 * _rigidbody.mass * speed, 0), ForceMode2D.Force);
        }
    }

    [Serializable]
    public class UpDownWaveMove : IMovement
    {
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float frequency = 2f;

        private Rigidbody2D _rigidbody;
        private float _yMovement;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2? currentPosition = null, float speed = 1f, float deltaTime = 0f)
        {
            _yMovement = amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - Time.deltaTime)));

            _rigidbody.AddForce(new Vector2(Time.deltaTime * 10f, _yMovement) * _rigidbody.mass);
            //enity.transform.Translate(Time.deltaTime * 10f, yMovement, 0f);
        }
    }

    [Serializable]
    public class MoveToLocation : IMovement
    {
        private Rigidbody2D _rigidbody;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2? currentPosition = null, float speed = 1f, float deltaTime = 0f)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class SpeedUpSlowDown : IMovement
    {
        [SerializeField] [MinValue(0)] private float[] speedFactor;
        [SerializeField] private Vector2 intervalRange;
        private Rigidbody2D _rigidbody;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2? currentPosition = null, float speed = 1f, float deltaTime = 0f)
        {
            if (_rigidbody.linearVelocity.x < -10f)
                _rigidbody.AddForce(new Vector2(-5 * _rigidbody.mass * speed, 0), ForceMode2D.Force);
        }
    }
}

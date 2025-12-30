using System;
using UnityEngine;

namespace Enemies.Movement
{
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

        public void Move(Vector2 currentPosition, float speed, float deltaTime)
        {
            _yMovement = amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - deltaTime)));

            _rigidbody.AddForce(new Vector2(deltaTime * -10f * speed, _yMovement) * _rigidbody.mass);
            _rigidbody.linearVelocityX = Mathf.Clamp(_rigidbody.linearVelocityX, -speed, speed);
            //enity.transform.Translate(Time.deltaTime * 10f, yMovement, 0f);
        }
    }
}

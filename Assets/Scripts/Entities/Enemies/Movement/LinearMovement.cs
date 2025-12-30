using System;
using UnityEngine;

namespace Enemies.Movement
{
    [Serializable]
    public class LinearMovement : IMovement
    {
        private Rigidbody2D _rigidbody;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 currentPosition, float speed, float deltaTime)
        {
            _rigidbody.AddForce(new Vector2(-5 * _rigidbody.mass * speed, 0), ForceMode2D.Force);
            _rigidbody.linearVelocityX = Mathf.Clamp(_rigidbody.linearVelocityX, -speed, speed);
        }
    }
}

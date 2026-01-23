using System;
using Entities.Enemies.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Entities.Enemies.Movement
{
    [Serializable]
    public class SpeedUpSlowDown : IMovement
    {
        [SerializeField] [MinValue(0)] private float[] speedFactor;
        [SerializeField] private Vector2 intervalRange;
        private Rigidbody _rigidbody;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody>();
        }

        public void Move(Vector2 currentPosition, float speed, float deltaTime)
        {
            _rigidbody.AddForce(new Vector2(-5 * _rigidbody.mass * speed, 0), ForceMode.Force);
            _rigidbody.maxLinearVelocity = speed;
        }
    }
}

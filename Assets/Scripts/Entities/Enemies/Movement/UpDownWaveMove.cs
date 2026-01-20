using System;
using Entities.Enemies.Interfaces;
using UnityEngine;

namespace Entities.Enemies.Movement
{
    [Serializable]
    public class UpDownWaveMove : IMovement
    {
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float frequency = 2f;

        private Rigidbody _rigidbody;
        private float _yMovement;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody>();
        }

        public void Move(Vector2 currentPosition, float speed, float deltaTime)
        {
            _yMovement = amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - deltaTime)));

            _rigidbody.AddForce(new Vector2(deltaTime * -10f * speed, _yMovement) * _rigidbody.mass);
            _rigidbody.maxLinearVelocity = speed;
            //enity.transform.Translate(Time.deltaTime * 10f, yMovement, 0f);
        }
    }
}

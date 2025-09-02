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

        public void Move(Vector2? currentPosition = null, float speed = 1f, float deltaTime = 0f)
        {
            _yMovement = amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - Time.deltaTime)));

            _rigidbody.AddForce(new Vector2(Time.deltaTime * 10f, _yMovement) * _rigidbody.mass);
            //enity.transform.Translate(Time.deltaTime * 10f, yMovement, 0f);
        }
    }
}

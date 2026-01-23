using System;
using Entities.Enemies.Interfaces;
using UnityEngine;

namespace Entities.Enemies.Movement
{
    [Serializable]
    public class MoveToLocation : IMovement
    {
        private Rigidbody _rigidbody;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody>();
        }

        public void Move(Vector2 currentPosition, float speed, float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using UnityEngine;

namespace Enemies.Movement
{
    [Serializable]
    public class MoveToLocation : IMovement
    {
        private Rigidbody2D _rigidbody;

        public void SetTarget(GameObject entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 currentPosition, float speed, float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}

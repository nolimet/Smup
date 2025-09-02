using UnityEngine;

namespace Enemies
{
    public interface IMovement
    {
        void Move(Vector2? currentPosition = null, float speed = 1f, float deltaTime = 0f);
        void SetTarget(GameObject entity);
    }
}

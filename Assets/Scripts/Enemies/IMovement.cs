﻿using UnityEngine;

namespace Enemies
{
    public interface IMovement
    {
        void Move(Vector2 currentPosition, float speed, float deltaTime);
        void SetTarget(GameObject entity);
    }
}

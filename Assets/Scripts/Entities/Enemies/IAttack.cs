using UnityEngine;

namespace Entities.Enemies
{
    public interface IAttack
    {
        void Attack(Vector2 position, Vector2 motionVector);
    }
}

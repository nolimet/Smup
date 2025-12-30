using System;
using Entities.Enemies.Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Splines;

namespace Entities.Enemies.Movement
{
    [Serializable]
    public class SplineFollow : IMovement, IKillAtEnd
    {
        public event Action AtEnd;

        private float _position;
        private float _splineLength;
        private GameObject _entity;
        private Spline _spline;

        [SerializeField] private bool rotate;
        [SerializeField] private bool killAtEnd = true;

        public void SetSpline([NotNull] Spline spline)
        {
            _spline = spline;

            _position = 0;
            _splineLength = spline.GetLength();
        }

        public void SetTarget([NotNull] GameObject entity)
        {
            _entity = entity;
        }

        public void Move(Vector2 currentPosition, float speed, float deltaTime)
        {
            _position += speed * deltaTime;
            if (_position >= _splineLength)
            {
                if (killAtEnd)
                    AtEnd?.Invoke();
                return;
            }

            _spline.Evaluate(_position / _splineLength, out var position, out var tangent, out var up);
            position.z = _entity.transform.position.z;
            _entity.transform.position = position;

            if (rotate)
            {
                //setting rotation
                var tan2 = new Vector2(tangent.x, tangent.y);
                if (tan2.sqrMagnitude > 0.000001f)
                {
                    var angle = Mathf.Atan2(tan2.y, tan2.x) * Mathf.Rad2Deg;
                    _entity.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }
    }
}

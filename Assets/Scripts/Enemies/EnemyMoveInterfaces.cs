using UnityEngine;
using System.Collections;
using System;

namespace Enemy.Interfaces.Move
{

    public interface IMovement
    {
        void Move(GameObject enity, Vector2? movelocation = null, float Speed = 1f, float moveDelay = 0f);
    }

    public class LinearMovement : IMovement
    {
        Rigidbody2D ri;
        public void Move(GameObject enity, Vector2? movelocation = null, float Speed = 1f, float moveDelay = 0f)
        {
            ri = enity.GetComponent<Rigidbody2D>();

            if (ri.velocity.x < -10f)
                ri.AddForce(new Vector2(-5 * ri.mass * Speed, 0), ForceMode2D.Force);
        }
    }

    public class SenoidalMovement : IMovement
    {
        private const float amplitude = 1f;
        private const float frequency = 2f;

        Rigidbody2D ri;
        float yMovement;

        public void Move(GameObject enity, Vector2? movelocation = null, float Speed = 1f, float moveDelay = 0f)
        {
            ri = enity.AddComponent<Rigidbody2D>();

            yMovement = amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time) - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - Time.deltaTime)));

            ri.AddForce(new Vector2(Time.deltaTime * 10f, yMovement) * ri.mass);
            //enity.transform.Translate(Time.deltaTime * 10f, yMovement, 0f);
        }
    }

    public class MoveToLocation : IMovement
    {
        public void Move(GameObject enity, Vector2? movelocation = null, float Speed = 1f, float moveDelay = 0f)
        {
            throw new NotImplementedException();
        }
    }
}
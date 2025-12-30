using Entities.Player.Weapons;
using UnityEngine;

public class StaticCannon : MonoBehaviour
{
    public IBaseWeapon Gun;
    public Vector3 spawnPosition = new(0.5f, 0);
    public Vector2 inheritVelocity = new();

    public float shootInterval = 0.2f;
    private float _timer = 0;

    private void Start()
    {
        Gun = new Cannon();
        _timer = shootInterval;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Gun.TryShoot(gameObject, spawnPosition, inheritVelocity);
            _timer = shootInterval;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + spawnPosition, 0.1f);
    }
}

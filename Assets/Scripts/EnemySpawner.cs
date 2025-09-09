using Enemies;
using ObjectPools;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string enemyId;
    [SerializeField] [ReadOnly] private Enemy currentTarget;

    private void Start()
    {
        currentTarget = EnemyPool.Instance.GetObject(enemyId);
        currentTarget.moveSpeed.SetOverride(0);
        currentTarget.transform.position = transform.position;
        var rigidbody2D = currentTarget.GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}

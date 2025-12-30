using ObjectPools;
using UnityEngine;

namespace Pickups
{
    public class ScrapPickup : MonoBehaviour
    {
        public float ScrapValue { get; private set; }
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// flo is used for animations
        /// lifetime does what it says is the time it has left to live
        /// A is alpha of the object
        /// </summary>
        private float _lifeTime = 120f, _alpha;

        private float _timer = 0;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _timer = Random.Range(0, 2f);
            _alpha = Random.Range(0, 0.3f);
        }

        public void Init(int l, float value = 1f)
        {
            _lifeTime = l;
            ScrapValue = value;
            _alpha = Random.Range(0, 0.3f);

            transform.localScale = Vector3.one * (Mathf.Pow(1.1f, value / 10f) - 0.6f);
        }

        private void Update()
        {
            var color = Color.Lerp(Color.white, Color.gray, Mathf.PingPong(_timer, 1));

            if (_alpha <= 1)
            {
                color.a = _alpha;
                _alpha = Mathf.MoveTowards(_alpha, 1, Time.deltaTime * 3);
            }

            _spriteRenderer.color = color;

            _timer += Time.deltaTime / 2f;
            _lifeTime -= Time.deltaTime;

            if (_lifeTime <= 0) ScrapPickupPool.RemovePickup(this);
        }
    }
}

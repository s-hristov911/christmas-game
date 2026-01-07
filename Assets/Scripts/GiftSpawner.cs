using UnityEngine;

namespace FallingGifts
{
    public class GiftSpawner : MonoBehaviour
    {
        [SerializeField] private float initialInterval = 1.2f;
        [SerializeField] private float minInterval = 0.35f;
        [SerializeField] private float baseSpeed = 2.6f;
        [SerializeField] private float baseAcceleration = 0.35f;

        private GameManager manager;
        private Sprite[] giftSprites;
        private float horizontalLimit;
        private float spawnHeight;
        private float bottomY;
        private float timer;
        private float currentInterval;
        private bool isActive;

        public void Configure(GameManager owner, Sprite[] sprites, Vector2 bounds)
        {
            manager = owner;
            giftSprites = sprites;
            horizontalLimit = Mathf.Abs(bounds.x) - 0.3f;
            spawnHeight = bounds.y + 1.2f;
            bottomY = owner.BottomBoundary;
            ResetSpawner();
        }

        public void ResetSpawner()
        {
            currentInterval = initialInterval;
            timer = 0f;
            isActive = true;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public void RampDifficulty(float intervalDelta, float speedDelta)
        {
            currentInterval = Mathf.Max(minInterval, currentInterval - intervalDelta);
            baseSpeed += speedDelta;
            baseAcceleration += speedDelta * 0.25f;
        }

        public void SetSpawning(bool active)
        {
            isActive = active;
            if (!active)
            {
                timer = 0f;
            }
        }

        private void Update()
        {
            if (!isActive || manager == null || manager.IsGameOver)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= currentInterval)
            {
                timer = 0f;
                SpawnGift();
            }
        }

        private void SpawnGift()
        {
            if (giftSprites == null || giftSprites.Length == 0)
            {
                return;
            }

            var giftGo = new GameObject("Gift");
            giftGo.transform.SetParent(transform);
            float x = Random.Range(-horizontalLimit, horizontalLimit);
            giftGo.transform.position = new Vector3(x, spawnHeight, 0f);

            var renderer = giftGo.AddComponent<SpriteRenderer>();
            renderer.sprite = giftSprites[Random.Range(0, giftSprites.Length)];
            renderer.sortingOrder = 1;
            renderer.color = Color.white;

            var collider = giftGo.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;

            var body = giftGo.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;

            var gift = giftGo.AddComponent<Gift>();
            float speed = baseSpeed + Random.Range(0f, 1.25f);
            float accel = baseAcceleration + Random.Range(0f, 0.35f);
            gift.Initialize(renderer.sprite, speed, accel, bottomY);
        }
    }
}

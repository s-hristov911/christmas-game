using UnityEngine;

namespace FallingGifts
{
    public class SnowfallController : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 0.12f;
        [SerializeField] private Vector2 scaleRange = new Vector2(0.35f, 0.85f);
        [SerializeField] private Vector2 fallSpeedRange = new Vector2(0.6f, 1.4f);
        [SerializeField] private Vector2 swaySpeedRange = new Vector2(0.6f, 1.4f);
        [SerializeField] private Vector2 swayAmplitudeRange = new Vector2(0.12f, 0.38f);
        [SerializeField] private Vector2 windRange = new Vector2(-0.15f, 0.15f);

        private Sprite snowflakeSprite;
        private float horizontalLimit;
        private float spawnHeight;
        private float bottomY;
        private float timer;
        private bool configured;

        public void Configure(Sprite sprite, Vector2 bounds, float bottomBoundary)
        {
            snowflakeSprite = sprite;
            horizontalLimit = Mathf.Abs(bounds.x) + 0.5f;
            spawnHeight = Mathf.Abs(bounds.y) + 1.5f;
            bottomY = bottomBoundary - 0.5f;
            configured = snowflakeSprite != null;
        }

        private void Update()
        {
            if (!configured)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnFlake();
            }
        }

        private void SpawnFlake()
        {
            var flakeGo = new GameObject("Snowflake");
            flakeGo.transform.SetParent(transform);
            float x = Random.Range(-horizontalLimit, horizontalLimit);
            float y = spawnHeight + Random.Range(-0.4f, 0.4f);
            flakeGo.transform.position = new Vector3(x, y, 0f);

            var renderer = flakeGo.AddComponent<SpriteRenderer>();
            renderer.sprite = snowflakeSprite;
            renderer.sortingOrder = -2;
            Color tint = Color.Lerp(new Color(0.8f, 0.9f, 1f, 0.85f), Color.white, Random.Range(0.1f, 0.6f));
            renderer.color = tint;

            float scale = Random.Range(scaleRange.x, scaleRange.y);
            flakeGo.transform.localScale = new Vector3(scale, scale, 1f);

            var particle = flakeGo.AddComponent<SnowflakeParticle>();
            float fallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
            float swaySpeed = Random.Range(swaySpeedRange.x, swaySpeedRange.y);
            float swayAmplitude = Random.Range(swayAmplitudeRange.x, swayAmplitudeRange.y);
            float wind = Random.Range(windRange.x, windRange.y);
            particle.Initialize(bottomY, fallSpeed, swaySpeed, swayAmplitude, wind);
        }
    }
}

using UnityEngine;

namespace FallingGifts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LightBulbTwinkle : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Color[] palette = { Color.white };
        private Vector2 intervalRange = new Vector2(0.35f, 0.9f);
        private float timer;
        private float duration;
        private Color startColor = Color.white;
        private Color targetColor = Color.white;
        private bool configured;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            startColor = spriteRenderer.color;
            targetColor = startColor;
            duration = Random.Range(intervalRange.x, intervalRange.y);
        }

        public void Configure(Color[] colors, Vector2 durationRange)
        {
            if (colors != null && colors.Length > 0)
            {
                palette = colors;
            }

            intervalRange = new Vector2(Mathf.Max(0.05f, durationRange.x), Mathf.Max(durationRange.x, durationRange.y));
            startColor = palette[Random.Range(0, palette.Length)];
            targetColor = palette[Random.Range(0, palette.Length)];
            spriteRenderer.color = startColor;
            duration = Random.Range(intervalRange.x, intervalRange.y);
            timer = 0f;
            configured = true;
        }

        private void Update()
        {
            if (!configured || spriteRenderer == null)
            {
                return;
            }

            timer += Time.deltaTime;
            float t = duration <= 0.001f ? 1f : Mathf.Clamp01(timer / duration);
            spriteRenderer.color = Color.Lerp(startColor, targetColor, t);

            if (timer >= duration)
            {
                startColor = targetColor;
                targetColor = palette[Random.Range(0, palette.Length)];
                duration = Random.Range(intervalRange.x, intervalRange.y);
                timer = 0f;
            }
        }
    }
}

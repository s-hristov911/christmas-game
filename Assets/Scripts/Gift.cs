using UnityEngine;

namespace FallingGifts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Gift : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D body;
        private float fallSpeed;
        private float acceleration;
        private float bottomY;
        private bool isActive;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            body = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Sprite sprite, float startSpeed, float acceleration, float bottomY)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = Color.white;
            fallSpeed = startSpeed;
            this.acceleration = acceleration;
            this.bottomY = bottomY;
            isActive = true;
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            fallSpeed += acceleration * Time.deltaTime;
            body.velocity = Vector2.down * fallSpeed;

            if (transform.position.y < bottomY)
            {
                GameManager.Instance?.RegisterGiftMissed(this);
            }
        }

        public void HandleCatch()
        {
            if (!isActive)
            {
                return;
            }

            isActive = false;
            body.velocity = Vector2.zero;
            Destroy(gameObject);
        }

        public void HandleMiss()
        {
            if (!isActive)
            {
                return;
            }

            isActive = false;
            body.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

namespace FallingGifts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class SantaController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6.5f;

        private Rigidbody2D body;
        private float horizontalLimit = 8f;
        private bool inputEnabled = true;
        private float cachedInput;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        public void Configure(float limit)
        {
            horizontalLimit = Mathf.Abs(limit);
        }

        public void SetInputEnabled(bool enabled)
        {
            inputEnabled = enabled;
            if (!enabled)
            {
                body.velocity = Vector2.zero;
                cachedInput = 0f;
            }
        }

        public void ResetPosition(float yPosition)
        {
            body.position = new Vector2(0f, yPosition);
            body.velocity = Vector2.zero;
            cachedInput = 0f;
        }

        private void Update()
        {
            cachedInput = inputEnabled ? Input.GetAxisRaw("Horizontal") : 0f;
        }

        private void FixedUpdate()
        {
            if (!inputEnabled)
            {
                return;
            }

            Vector2 current = body.position;
            current.x += cachedInput * moveSpeed * Time.fixedDeltaTime;
            current.x = Mathf.Clamp(current.x, -horizontalLimit, horizontalLimit);
            body.MovePosition(current);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!inputEnabled)
            {
                return;
            }

            var gift = other.GetComponent<Gift>();
            if (gift == null)
            {
                return;
            }

            GameManager.Instance?.RegisterGiftCaught(gift);
        }
    }
}

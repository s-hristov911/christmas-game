using UnityEngine;

namespace FallingGifts
{
    public class SnowflakeParticle : MonoBehaviour
    {
        private float bottomY;
        private float fallSpeed;
        private float swaySpeed;
        private float swayAmplitude;
        private float windDrift;
        private float swayOffset;
        private float baseX;

        public void Initialize(float bottomBoundary, float fallSpeed, float swaySpeed, float swayAmplitude, float wind)
        {
            bottomY = bottomBoundary;
            this.fallSpeed = fallSpeed;
            this.swaySpeed = swaySpeed;
            this.swayAmplitude = swayAmplitude;
            windDrift = wind;
            swayOffset = Random.Range(0f, Mathf.PI * 2f);
            baseX = transform.position.x;
        }

        private void Update()
        {
            baseX += windDrift * Time.deltaTime;
            Vector3 position = transform.position;
            position.y -= fallSpeed * Time.deltaTime;
            position.x = baseX + Mathf.Sin(Time.time * swaySpeed + swayOffset) * swayAmplitude;
            transform.position = position;

            if (position.y < bottomY)
            {
                Destroy(gameObject);
            }
        }
    }
}

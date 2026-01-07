using UnityEngine;

namespace FallingGifts
{
    public static class HolidaySpriteFactory
    {
        private static readonly Color32 Transparent = new Color32(0, 0, 0, 0);

        public static Sprite CreateSantaSprite(float desiredWidthUnits)
        {
            var canvas = new PixelCanvas(64, 96, Transparent);

            var coat = new Color32(204, 42, 42, 255);
            var coatShadow = new Color32(168, 28, 32, 255);
            var trim = new Color32(250, 247, 242, 255);
            var beard = new Color32(236, 235, 230, 255);
            var face = new Color32(255, 220, 184, 255);
            var belt = new Color32(26, 26, 26, 255);
            var buckle = new Color32(252, 204, 88, 255);
            var mitten = new Color32(235, 72, 72, 255);

            // Coat body and shading
            canvas.FillRect(20, 10, 24, 40, coat);
            canvas.FillRect(20, 10, 8, 40, coatShadow);

            // Arms
            canvas.FillRect(10, 24, 12, 10, coatShadow);
            canvas.FillRect(42, 24, 12, 10, coat);
            canvas.FillRect(8, 20, 8, 8, mitten);
            canvas.FillRect(48, 20, 8, 8, mitten);

            // Trim and belt
            canvas.FillRect(20, 8, 24, 4, trim);
            canvas.FillRect(20, 46, 24, 6, trim);
            canvas.FillRect(16, 34, 32, 6, belt);
            canvas.FillRect(28, 34, 8, 6, buckle);

            // Face & beard
            canvas.FillRect(22, 50, 20, 10, face);
            canvas.FillRect(18, 56, 28, 12, beard);
            canvas.FillRect(20, 66, 24, 6, trim);

            // Hat
            canvas.FillRect(20, 70, 24, 6, trim);
            canvas.FillRect(24, 76, 16, 10, coat);
            canvas.FillRect(36, 80, 10, 6, coat);
            canvas.FillRect(44, 78, 6, 6, trim);

            // Boots
            canvas.FillRect(20, 4, 10, 6, belt);
            canvas.FillRect(34, 4, 10, 6, belt);

            return canvas.Build("SantaSprite", desiredWidthUnits);
        }

        public static Sprite CreateGiftSprite(Color32 wrapColor, Color32 ribbonColor, Color32 accentColor, float desiredWidthUnits)
        {
            var canvas = new PixelCanvas(48, 48, Transparent);

            // Base
            canvas.FillRect(4, 4, 40, 40, wrapColor);

            // Simple shading and highlight
            var shadow = Darken(wrapColor, 0.25f);
            canvas.FillRect(4, 4, 12, 40, shadow);
            var highlight = Lighten(wrapColor, 0.2f);
            canvas.FillRect(32, 4, 12, 40, highlight);

            // Ribbon cross
            canvas.FillRect(21, 4, 6, 40, ribbonColor);
            canvas.FillRect(4, 21, 40, 6, ribbonColor);
            canvas.FillRect(18, 30, 12, 8, accentColor);
            canvas.FillRect(18, 12, 12, 8, accentColor);

            // Tag
            canvas.FillRect(30, 34, 6, 8, Lighten(accentColor, 0.3f));

            return canvas.Build("GiftSprite", desiredWidthUnits);
        }

        public static Sprite CreateSnowGroundSprite(float widthUnits, float heightUnits)
        {
            int width = Mathf.CeilToInt(widthUnits * 32f);
            int height = Mathf.CeilToInt(heightUnits * 32f);
            var canvas = new PixelCanvas(width, height, Transparent);

            var baseColor = new Color32(232, 238, 250, 255);
            var rimColor = new Color32(250, 250, 255, 255);
            var shadow = new Color32(188, 198, 214, 255);
            canvas.FillRect(0, 0, width, height, baseColor);

            for (int x = 0; x < width; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.05f, 0.5f);
                int rimHeight = Mathf.RoundToInt(Mathf.Lerp(height * 0.25f, height * 0.55f, noise));
                for (int y = height - rimHeight; y < height; y++)
                {
                    canvas.SetPixel(x, y, rimColor);
                }

                if (x % 3 == 0)
                {
                    canvas.SetPixel(x, height - rimHeight - 2, shadow);
                }
            }

            canvas.FillRect(0, 0, width, Mathf.CeilToInt(height * 0.18f), shadow);

            return canvas.Build("SnowGroundSprite", widthUnits);
        }

        public static Sprite CreateSkySprite(Vector2 bounds, int starCount)
        {
            float widthUnits = Mathf.Max(1f, bounds.x * 2f + 2f);
            float heightUnits = Mathf.Max(1f, bounds.y * 2f + 2f);
            int width = Mathf.CeilToInt(widthUnits * 32f);
            int height = Mathf.CeilToInt(heightUnits * 32f);
            var canvas = new PixelCanvas(width, height, Transparent);

            var horizon = new Color(0.07f, 0.12f, 0.22f, 1f);
            var zenith = new Color(0.02f, 0.04f, 0.12f, 1f);

            for (int y = 0; y < height; y++)
            {
                float t = y / (float)(height - 1);
                Color rowColor = Color.Lerp(horizon, zenith, t * t);
                for (int x = 0; x < width; x++)
                {
                    canvas.SetPixel(x, y, rowColor);
                }
            }

            var glow = new Color(0.35f, 0.52f, 0.76f, 0.12f);
            for (int stripe = 0; stripe < 3; stripe++)
            {
                int stripeY = Mathf.RoundToInt(height * (0.3f + stripe * 0.15f));
                for (int x = 0; x < width; x++)
                {
                    int y = stripeY + Mathf.RoundToInt(Mathf.Sin((x / (float)width) * Mathf.PI * 2f + stripe) * 18f);
                    canvas.BlendPixel(x, Mathf.Clamp(y, 0, height - 1), glow);
                }
            }

            for (int i = 0; i < starCount; i++)
            {
                int x = Random.Range(4, width - 4);
                int y = Random.Range(height / 2, height - 4);
                DrawStar(canvas, x, y, i % 3 == 0 ? 2 : 1);
            }

            return canvas.Build("SkySprite", widthUnits);
        }

        public static Sprite CreateTreeSprite(float desiredWidthUnits)
        {
            var canvas = new PixelCanvas(64, 96, Transparent);
            var trunk = new Color32(120, 84, 60, 255);
            var foliageDark = new Color32(16, 72, 44, 255);
            var foliageLight = new Color32(28, 110, 60, 255);
            var ornament = new Color32(250, 234, 120, 255);

            canvas.FillRect(28, 0, 8, 18, trunk);

            int centerX = 32;
            int layerHeight = 18;
            for (int layer = 0; layer < 3; layer++)
            {
                int startY = 18 + layer * layerHeight;
                int segmentHeight = layerHeight + layer * 2;
                for (int y = 0; y < segmentHeight; y++)
                {
                    float t = y / (float)segmentHeight;
                    int halfWidth = Mathf.RoundToInt(Mathf.Lerp(6 + layer * 2, 28 - layer, 1f - t));
                    Color32 color = layer % 2 == 0 ? foliageLight : foliageDark;
                    canvas.FillRect(centerX - halfWidth, startY + y, halfWidth * 2, 1, color);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                int x = 16 + i * 6;
                int y = 26 + Random.Range(0, 50);
                canvas.FillCircle(x, y, 2f, i % 2 == 0 ? ornament : Lighten(ornament, 0.2f));
            }

            return canvas.Build("TreeSprite", desiredWidthUnits);
        }

        public static Sprite CreateSnowflakeSprite(float desiredWidthUnits)
        {
            var canvas = new PixelCanvas(32, 32, Transparent);
            var bright = new Color32(255, 255, 255, 220);
            var soft = new Color32(210, 230, 255, 200);

            int center = 16;
            for (int offset = -8; offset <= 8; offset++)
            {
                canvas.SetPixel(center + offset, center, bright);
                canvas.SetPixel(center, center + offset, bright);
            }

            for (int offset = -6; offset <= 6; offset++)
            {
                canvas.SetPixel(center + offset, center + offset, soft);
                canvas.SetPixel(center - offset, center + offset, soft);
            }

            canvas.FillCircle(center, center, 3f, bright);

            return canvas.Build("SnowflakeSprite", desiredWidthUnits);
        }

        public static Sprite CreateBulbSprite(float desiredWidthUnits)
        {
            var canvas = new PixelCanvas(24, 32, Transparent);
            var casing = new Color32(40, 40, 48, 255);
            var baseColor = new Color32(255, 255, 255, 255);

            canvas.FillRect(8, 4, 8, 6, casing);
            canvas.FillCircle(12, 16, 10f, baseColor);
            canvas.FillCircle(12, 18, 6f, Lighten(baseColor, 0.15f));

            return canvas.Build("BulbSprite", desiredWidthUnits);
        }

        private static void DrawStar(PixelCanvas canvas, int x, int y, int radius)
        {
            var core = new Color32(255, 255, 220, 255);
            var glow = new Color32(255, 255, 255, 180);
            canvas.SetPixel(x, y, core);
            for (int i = -radius; i <= radius; i++)
            {
                canvas.BlendPixel(x + i, y, glow);
                canvas.BlendPixel(x, y + i, glow);
            }
        }

        private static Color32 Lighten(Color32 color, float amount)
        {
            var c = (Color)color;
            c = Color.Lerp(c, Color.white, Mathf.Clamp01(amount));
            return (Color32)c;
        }

        private static Color32 Darken(Color32 color, float amount)
        {
            var c = (Color)color;
            c = Color.Lerp(c, Color.black, Mathf.Clamp01(amount));
            return (Color32)c;
        }

        private sealed class PixelCanvas
        {
            private readonly int width;
            private readonly int height;
            private readonly Color32[] pixels;

            public PixelCanvas(int width, int height, Color32 background)
            {
                this.width = width;
                this.height = height;
                pixels = new Color32[width * height];
                Clear(background);
            }

            public void Clear(Color32 color)
            {
                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = color;
                }
            }

            public void SetPixel(int x, int y, Color color)
            {
                if (!Contains(x, y))
                {
                    return;
                }

                pixels[y * width + x] = color;
            }

            public void BlendPixel(int x, int y, Color color)
            {
                if (!Contains(x, y))
                {
                    return;
                }

                Color existing = pixels[y * width + x];
                float a = color.a;
                Color blended = Color.Lerp(existing, color, a);
                pixels[y * width + x] = blended;
            }

            public void FillRect(int x, int y, int rectWidth, int rectHeight, Color32 color)
            {
                int startX = Mathf.Clamp(x, 0, width);
                int endX = Mathf.Clamp(x + rectWidth, 0, width);
                int startY = Mathf.Clamp(y, 0, height);
                int endY = Mathf.Clamp(y + rectHeight, 0, height);

                for (int yy = startY; yy < endY; yy++)
                {
                    int row = yy * width;
                    for (int xx = startX; xx < endX; xx++)
                    {
                        pixels[row + xx] = color;
                    }
                }
            }

            public void FillCircle(int cx, int cy, float radius, Color32 color)
            {
                float sqrRadius = radius * radius;
                int minX = Mathf.Max(0, Mathf.FloorToInt(cx - radius));
                int maxX = Mathf.Min(width - 1, Mathf.CeilToInt(cx + radius));
                int minY = Mathf.Max(0, Mathf.FloorToInt(cy - radius));
                int maxY = Mathf.Min(height - 1, Mathf.CeilToInt(cy + radius));

                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        float dx = x - cx;
                        float dy = y - cy;
                        if (dx * dx + dy * dy <= sqrRadius)
                        {
                            pixels[y * width + x] = color;
                        }
                    }
                }
            }

            public Sprite Build(string name, float desiredWidthUnits)
            {
                float pixelsPerUnit = Mathf.Max(1f, width / Mathf.Max(0.01f, desiredWidthUnits));
                var texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp,
                    name = name + "Texture"
                };

                texture.SetPixels32(pixels);
                texture.Apply();

                var sprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                sprite.name = name;
                return sprite;
            }

            private bool Contains(int x, int y)
            {
                return x >= 0 && x < width && y >= 0 && y < height;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace FallingGifts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Session")]
        [SerializeField] private int startingLives = 3;
        [SerializeField] private Vector2 playBounds = new Vector2(8f, 5f);
        [SerializeField] private Color backgroundColor = new Color(0.05f, 0.08f, 0.15f);

        [Header("Visuals")]
        [SerializeField] private float santaVisualWidth = 1.6f;
        [SerializeField] private float snowHeight = 1.4f;
        [SerializeField] private int starCount = 60;

        [Header("Difficulty")]
        [SerializeField] private int catchesPerLevel = 4;
        [SerializeField] private float intervalStep = 0.1f;
        [SerializeField] private float speedBoost = 0.25f;

        private SantaController santa;
        private GiftSpawner spawner;
        private Text scoreText;
        private Text livesText;
        private Text messageText;

        private Sprite santaSprite;
        private Sprite groundSprite;
        private Sprite skySprite;
        private Sprite treeSprite;
        private Sprite bulbSprite;
        private Sprite snowflakeSprite;
        private Sprite[] giftSprites;
        private SnowfallController snowfall;

        private readonly Color[] bulbPalette =
        {
            new Color(0.98f, 0.54f, 0.22f),
            new Color(0.34f, 0.82f, 0.98f),
            new Color(0.99f, 0.32f, 0.4f),
            new Color(0.58f, 0.96f, 0.64f),
            new Color(0.97f, 0.89f, 0.35f),
            new Color(0.82f, 0.5f, 0.95f)
        };

        private readonly Color uiTextColor = new Color(0.96f, 0.93f, 0.86f);

        private int score;
        private int lives;
        private int tierCatchCount;
        private bool isGameOver;

        public bool IsGameOver => isGameOver;
        public float BottomBoundary => -playBounds.y - 1.2f;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Application.targetFrameRate = 120;
            EnsureCamera();
            PrepareSprites();
            BuildWorld();
            BuildUi();
            ResetSession();
        }

        private void Update()
        {
            if (isGameOver && Input.GetKeyDown(KeyCode.R))
            {
                ResetSession();
            }
        }

        private void EnsureCamera()
        {
            var camera = Camera.main;
            if (camera == null)
            {
                var camObject = new GameObject("Main Camera");
                camera = camObject.AddComponent<Camera>();
                camObject.AddComponent<AudioListener>();
            }

            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = backgroundColor;
            camera.orthographic = true;
            camera.orthographicSize = playBounds.y;
            camera.transform.position = new Vector3(0f, 0f, -10f);
        }

        private void PrepareSprites()
        {
            santaSprite = HolidaySpriteFactory.CreateSantaSprite(santaVisualWidth);
            groundSprite = HolidaySpriteFactory.CreateSnowGroundSprite(playBounds.x * 2f + 2f, snowHeight);
            skySprite = HolidaySpriteFactory.CreateSkySprite(playBounds, starCount);
            treeSprite = HolidaySpriteFactory.CreateTreeSprite(2.8f);
            bulbSprite = HolidaySpriteFactory.CreateBulbSprite(0.36f);
            snowflakeSprite = HolidaySpriteFactory.CreateSnowflakeSprite(0.45f);

            giftSprites = new[]
            {
                HolidaySpriteFactory.CreateGiftSprite(new Color32(189, 36, 43, 255), new Color32(250, 216, 124, 255), new Color32(255, 255, 255, 255), 1.1f),
                HolidaySpriteFactory.CreateGiftSprite(new Color32(22, 124, 78, 255), new Color32(197, 34, 50, 255), new Color32(253, 238, 197, 255), 1.1f),
                HolidaySpriteFactory.CreateGiftSprite(new Color32(32, 150, 170, 255), new Color32(254, 255, 255, 255), new Color32(210, 62, 72, 255), 1.1f),
                HolidaySpriteFactory.CreateGiftSprite(new Color32(110, 62, 168, 255), new Color32(255, 166, 221, 255), new Color32(240, 244, 255, 255), 1.1f)
            };

            if (groundSprite == null)
            {
                groundSprite = CreateSolidSprite(new Color(0.92f, 0.95f, 1f), new Vector2(playBounds.x * 2f, snowHeight), "FallbackGround");
            }

            if (santaSprite == null)
            {
                santaSprite = CreateSolidSprite(new Color(0.86f, 0.11f, 0.2f), new Vector2(santaVisualWidth, 1.4f), "FallbackSanta");
            }

            if (giftSprites == null || giftSprites.Length == 0)
            {
                giftSprites = new[] { CreateSolidSprite(new Color(0.95f, 0.95f, 0.3f), new Vector2(1f, 1f), "FallbackGift") };
            }
        }

        private void BuildWorld()
        {
            BuildBackdrop();
            BuildGround();
            BuildSantaObject();
            BuildSpawnerObject();
            BuildDecorations();
            BuildSnowfall();
        }

        private void BuildBackdrop()
        {
            if (skySprite == null)
            {
                return;
            }

            var sky = new GameObject("Sky Backdrop");
            sky.transform.SetParent(transform);
            sky.transform.position = new Vector3(0f, 0.1f, 5f);
            var renderer = sky.AddComponent<SpriteRenderer>();
            renderer.sprite = skySprite;
            renderer.sortingOrder = -10;
        }

        private void BuildGround()
        {
            var ground = new GameObject("Snowbank");
            ground.transform.SetParent(transform);
            var groundRenderer = ground.AddComponent<SpriteRenderer>();
            groundRenderer.sprite = groundSprite;
            groundRenderer.sortingOrder = -1;
            ground.transform.position = new Vector3(0f, -playBounds.y - 0.2f, 0f);
        }

        private void BuildSantaObject()
        {
            var santaObject = new GameObject("Santa");
            santaObject.transform.SetParent(transform);
            santaObject.transform.position = new Vector3(0f, -playBounds.y + 0.85f, 0f);

            var santaRenderer = santaObject.AddComponent<SpriteRenderer>();
            santaRenderer.sprite = santaSprite;
            santaRenderer.sortingOrder = 4;

            var santaBody = santaObject.AddComponent<Rigidbody2D>();
            santaBody.gravityScale = 0f;
            santaBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            santaBody.interpolation = RigidbodyInterpolation2D.Interpolate;

            var santaCollider = santaObject.AddComponent<BoxCollider2D>();
            santaCollider.isTrigger = true;
            if (santaSprite != null)
            {
                var size = santaSprite.bounds.size;
                santaCollider.size = new Vector2(size.x * 0.55f, size.y * 0.75f);
                santaCollider.offset = new Vector2(0f, size.y * -0.12f);
            }
            else
            {
                santaCollider.size = new Vector2(0.9f, 1.4f);
            }

            santa = santaObject.AddComponent<SantaController>();
            santa.Configure(playBounds.x - 0.5f);
        }

        private void BuildSpawnerObject()
        {
            var spawnerObject = new GameObject("GiftSpawner");
            spawnerObject.transform.SetParent(transform);
            spawner = spawnerObject.AddComponent<GiftSpawner>();
            spawner.Configure(this, giftSprites, playBounds);
        }

        private void BuildDecorations()
        {
            if (treeSprite != null)
            {
                CreateTree(new Vector3(-playBounds.x + 2.1f, -playBounds.y + 0.6f, 0f), 1.05f, false, "TreeLeft");
                CreateTree(new Vector3(playBounds.x - 2.1f, -playBounds.y + 0.65f, 0f), 0.9f, true, "TreeRight");
            }

            BuildGarland();
        }

        private void CreateTree(Vector3 position, float scale, bool flipX, string name)
        {
            var tree = new GameObject(name);
            tree.transform.SetParent(transform);
            tree.transform.position = position;
            tree.transform.localScale = Vector3.one * scale;
            var renderer = tree.AddComponent<SpriteRenderer>();
            renderer.sprite = treeSprite;
            renderer.sortingOrder = 0;
            renderer.flipX = flipX;
        }

        private void BuildGarland()
        {
            if (bulbSprite == null)
            {
                return;
            }

            var garland = new GameObject("LightGarland");
            garland.transform.SetParent(transform);
            int bulbCount = Mathf.Max(10, Mathf.RoundToInt(playBounds.x * 3f));
            float left = -playBounds.x + 0.8f;
            float right = playBounds.x - 0.8f;

            for (int i = 0; i < bulbCount; i++)
            {
                float t = bulbCount == 1 ? 0f : i / (float)(bulbCount - 1);
                float x = Mathf.Lerp(left, right, t);
                float wave = Mathf.Sin(t * Mathf.PI) * 0.45f;
                float y = playBounds.y - 0.35f + wave;

                var bulb = new GameObject($"Bulb_{i}");
                bulb.transform.SetParent(garland.transform);
                bulb.transform.position = new Vector3(x, y, 0f);

                var renderer = bulb.AddComponent<SpriteRenderer>();
                renderer.sprite = bulbSprite;
                renderer.sortingOrder = 3;
                renderer.color = bulbPalette[i % bulbPalette.Length];

                var twinkle = bulb.AddComponent<LightBulbTwinkle>();
                twinkle.Configure(bulbPalette, new Vector2(0.35f, 0.85f));
            }
        }

        private void BuildSnowfall()
        {
            if (snowflakeSprite == null)
            {
                return;
            }

            var snow = new GameObject("Snowfall");
            snow.transform.SetParent(transform);
            snowfall = snow.AddComponent<SnowfallController>();
            snowfall.Configure(snowflakeSprite, playBounds, BottomBoundary);
        }

        private void BuildUi()
        {
            var canvasGo = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            scoreText = CreateText(canvasGo.transform, "ScoreText", font, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(150f, -80f), TextAnchor.UpperLeft);
            livesText = CreateText(canvasGo.transform, "LivesText", font, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-150f, -80f), TextAnchor.UpperRight);
            messageText = CreateText(canvasGo.transform, "MessageText", font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, TextAnchor.MiddleCenter);
            messageText.fontSize = 40;
            messageText.gameObject.SetActive(false);
        }

        private Text CreateText(Transform parent, string name, Font font, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, TextAnchor alignment)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = new Vector2(460f, 80f);
            rect.anchoredPosition = anchoredPosition;

            var text = go.AddComponent<Text>();
            text.font = font;
            text.fontSize = 32;
            text.alignment = alignment;
            text.color = uiTextColor;
            text.fontStyle = FontStyle.Bold;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;

            var shadow = go.AddComponent<Shadow>();
            shadow.effectDistance = new Vector2(2f, -2f);
            shadow.effectColor = new Color(0f, 0f, 0f, 0.55f);
            return text;
        }

        private void ResetSession()
        {
            score = 0;
            lives = startingLives;
            tierCatchCount = 0;
            isGameOver = false;
            messageText.gameObject.SetActive(false);

            UpdateUi();
            santa.ResetPosition(-playBounds.y + 0.8f);
            santa.SetInputEnabled(true);
            spawner.ResetSpawner();
        }

        private void UpdateUi()
        {
            scoreText.text = $"Score: {score}";
            livesText.text = $"Lives: {lives}";
        }

        public void RegisterGiftCaught(Gift gift)
        {
            if (gift == null || isGameOver)
            {
                return;
            }

            score += 1;
            tierCatchCount++;
            UpdateUi();
            gift.HandleCatch();

            if (tierCatchCount >= catchesPerLevel)
            {
                tierCatchCount = 0;
                spawner.RampDifficulty(intervalStep, speedBoost);
            }
        }

        public void RegisterGiftMissed(Gift gift)
        {
            if (gift == null || isGameOver)
            {
                return;
            }

            gift.HandleMiss();
            lives -= 1;
            UpdateUi();

            if (lives <= 0)
            {
                TriggerGameOver();
            }
        }

        private void TriggerGameOver()
        {
            isGameOver = true;
            messageText.gameObject.SetActive(true);
            messageText.text = "Out of gifts!\nPress R to restart";
            santa.SetInputEnabled(false);
            spawner.SetSpawning(false);
        }

        private Sprite CreateSolidSprite(Color color, Vector2 size, string name)
        {
            const int texSize = 64;
            var texture = new Texture2D(texSize, texSize, TextureFormat.RGBA32, false)
            {
                name = name,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            var pixels = new Color[texSize * texSize];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            texture.Apply();

            float ppu = texSize / Mathf.Max(size.x, size.y);
            return Sprite.Create(texture, new Rect(0f, 0f, texSize, texSize), new Vector2(0.5f, 0.5f), ppu);
        }
    }

    public static class GameBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureGameManager()
        {
            if (GameManager.Instance != null)
            {
                return;
            }

            var go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
        }
    }
}

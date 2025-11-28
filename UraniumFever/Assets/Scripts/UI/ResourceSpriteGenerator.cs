using UnityEngine;

namespace UraniumFever.UI
{
    /// <summary>
    /// Generates simple placeholder sprites for resources.
    /// This is a temporary solution until proper art assets are created.
    /// </summary>
    public class ResourceSpriteGenerator : MonoBehaviour
    {
        [Header("Generate Sprites")]
        [SerializeField] private bool generateOnStart = true;
        [SerializeField] private int spriteSize = 128;

        [Header("Output (Will be populated)")]
        public Sprite electricitySprite;
        public Sprite foodSprite;
        public Sprite medicineSprite;
        public Sprite playerChoiceSprite;

        private void Start()
        {
            if (generateOnStart)
            {
                GenerateAllSprites();
            }
        }

        [ContextMenu("Generate All Resource Sprites")]
        public void GenerateAllSprites()
        {
            electricitySprite = GenerateSprite(new Color(1f, 0.9f, 0.2f), "⚡"); // Yellow
            foodSprite = GenerateSprite(new Color(0.2f, 0.8f, 0.2f), "🍎"); // Green
            medicineSprite = GenerateSprite(new Color(0.9f, 0.2f, 0.2f), "💊"); // Red
            playerChoiceSprite = GenerateSprite(new Color(0.6f, 0.4f, 0.9f), "⭐"); // Purple

            Debug.Log("Resource sprites generated!");
        }

        private Sprite GenerateSprite(Color color, string symbol)
        {
            // Create a simple colored square texture
            Texture2D texture = new Texture2D(spriteSize, spriteSize);

            // Fill with color and add a border
            for (int x = 0; x < spriteSize; x++)
            {
                for (int y = 0; y < spriteSize; y++)
                {
                    // Border (10% of size)
                    int border = spriteSize / 10;
                    bool isBorder = x < border || x >= spriteSize - border ||
                                   y < border || y >= spriteSize - border;

                    if (isBorder)
                    {
                        // Darker border
                        texture.SetPixel(x, y, color * 0.6f);
                    }
                    else
                    {
                        // Center - gradient
                        float centerX = spriteSize / 2f;
                        float centerY = spriteSize / 2f;
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                        float maxDistance = spriteSize / 2f;
                        float gradient = 1f - (distance / maxDistance) * 0.3f; // Subtle gradient

                        texture.SetPixel(x, y, color * gradient);
                    }
                }
            }

            texture.Apply();

            // Create sprite from texture
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, spriteSize, spriteSize),
                new Vector2(0.5f, 0.5f),
                100f
            );

            sprite.name = $"Resource_{symbol}";

            return sprite;
        }
    }
}

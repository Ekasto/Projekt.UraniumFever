using UnityEngine;

namespace UraniumFever.UI
{
    /// <summary>
    /// Library for resource icons and visual representation.
    /// </summary>
    public class ResourceIconLibrary : MonoBehaviour
    {
        [Header("Resource Icons")]
        [SerializeField] private Sprite electricityIcon;
        [SerializeField] private Sprite foodIcon;
        [SerializeField] private Sprite medicineIcon;
        [SerializeField] private Sprite playerChoiceIcon;

        private static ResourceIconLibrary _instance;

        public static ResourceIconLibrary Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ResourceIconLibrary>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public Sprite GetIcon(Game.ResourceType resourceType)
        {
            switch (resourceType)
            {
                case Game.ResourceType.Electricity:
                    return electricityIcon;
                case Game.ResourceType.Food:
                    return foodIcon;
                case Game.ResourceType.Medicine:
                    return medicineIcon;
                case Game.ResourceType.PlayerChoice:
                    return playerChoiceIcon;
                default:
                    return null;
            }
        }

        public Color GetColor(Game.ResourceType resourceType)
        {
            switch (resourceType)
            {
                case Game.ResourceType.Electricity:
                    return new Color(1f, 0.9f, 0.2f); // Yellow
                case Game.ResourceType.Food:
                    return new Color(0.2f, 0.8f, 0.2f); // Green
                case Game.ResourceType.Medicine:
                    return new Color(0.9f, 0.2f, 0.2f); // Red
                case Game.ResourceType.PlayerChoice:
                    return new Color(0.6f, 0.4f, 0.9f); // Purple
                default:
                    return Color.white;
            }
        }

        public string GetSymbol(Game.ResourceType resourceType)
        {
            switch (resourceType)
            {
                case Game.ResourceType.Electricity:
                    return "⚡";
                case Game.ResourceType.Food:
                    return "🍎";
                case Game.ResourceType.Medicine:
                    return "💊";
                case Game.ResourceType.PlayerChoice:
                    return "⭐";
                default:
                    return "?";
            }
        }

        public string GetName(Game.ResourceType resourceType)
        {
            switch (resourceType)
            {
                case Game.ResourceType.Electricity:
                    return "Electricity";
                case Game.ResourceType.Food:
                    return "Food";
                case Game.ResourceType.Medicine:
                    return "Medicine";
                case Game.ResourceType.PlayerChoice:
                    return "Player Choice";
                default:
                    return "Unknown";
            }
        }
    }
}

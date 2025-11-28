using UnityEngine;

namespace UraniumFever.Game
{
    /// <summary>
    /// Visualizes the 3 player headquarters on the grid.
    /// </summary>
    public class HQVisualizer : MonoBehaviour
    {
        [Header("HQ Prefabs (Optional)")]
        [SerializeField] private GameObject foodHQPrefab;
        [SerializeField] private GameObject electricityHQPrefab;
        [SerializeField] private GameObject medicineHQPrefab;

        [Header("Settings")]
        [SerializeField] private float hqHeight = 1f;
        [SerializeField] private float tileSize = 1f;
        [SerializeField] private float tileSpacing = 0.1f;

        private GameObject[] _hqObjects;

        public void PlaceHeadquarters(Player[] players, Transform gridParent)
        {
            _hqObjects = new GameObject[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                var localPosition = CalculateLocalPosition(player.HQPosition);

                GameObject hqObject = CreateHQ(player.HQType, gridParent, localPosition);
                hqObject.name = $"HQ_{player.HQType}_{player.PlayerId}";

                _hqObjects[i] = hqObject;
            }
        }

        private Vector3 CalculateLocalPosition(Vector2Int gridPos)
        {
            return new Vector3(
                gridPos.x * (tileSize + tileSpacing),
                hqHeight,
                gridPos.y * (tileSize + tileSpacing)
            );
        }

        private GameObject CreateHQ(ResourceType hqType, Transform gridParent, Vector3 localPosition)
        {
            GameObject hqObject = null;

            // Try to use prefabs if assigned
            switch (hqType)
            {
                case ResourceType.Food:
                    if (foodHQPrefab != null)
                        hqObject = Instantiate(foodHQPrefab, gridParent);
                    break;
                case ResourceType.Electricity:
                    if (electricityHQPrefab != null)
                        hqObject = Instantiate(electricityHQPrefab, gridParent);
                    break;
                case ResourceType.Medicine:
                    if (medicineHQPrefab != null)
                        hqObject = Instantiate(medicineHQPrefab, gridParent);
                    break;
            }

            // Fallback: Create simple colored cube
            if (hqObject == null)
            {
                hqObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                hqObject.transform.SetParent(gridParent, false); // Set parent first with worldPositionStays = false
                hqObject.transform.localScale = new Vector3(tileSize * 0.8f, hqHeight * 2f, tileSize * 0.8f);

                var renderer = hqObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    mat.color = GetColorForResourceType(hqType);
                    renderer.material = mat;
                }
            }

            // Set local position after parenting
            hqObject.transform.localPosition = localPosition;

            return hqObject;
        }

        private Color GetColorForResourceType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Food:
                    return new Color(0.2f, 0.8f, 0.2f); // Green
                case ResourceType.Electricity:
                    return new Color(1f, 0.9f, 0.2f); // Yellow/Gold
                case ResourceType.Medicine:
                    return new Color(1f, 0.3f, 0.3f); // Red
                case ResourceType.PlayerChoice:
                    return new Color(0.5f, 0.3f, 0.9f); // Purple
                default:
                    return Color.white;
            }
        }
    }
}

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
                var position = CalculateWorldPosition(player.HQPosition);

                GameObject hqObject = CreateHQ(player.HQType, position);
                hqObject.name = $"HQ_{player.HQType}_{player.PlayerId}";
                hqObject.transform.parent = gridParent;

                _hqObjects[i] = hqObject;
            }
        }

        private Vector3 CalculateWorldPosition(Vector2Int gridPos)
        {
            return new Vector3(
                gridPos.x * (tileSize + tileSpacing),
                hqHeight,
                gridPos.y * (tileSize + tileSpacing)
            );
        }

        private GameObject CreateHQ(ResourceType hqType, Vector3 position)
        {
            GameObject hqObject = null;

            // Try to use prefabs if assigned
            switch (hqType)
            {
                case ResourceType.Food:
                    if (foodHQPrefab != null)
                        hqObject = Instantiate(foodHQPrefab, position, Quaternion.identity);
                    break;
                case ResourceType.Electricity:
                    if (electricityHQPrefab != null)
                        hqObject = Instantiate(electricityHQPrefab, position, Quaternion.identity);
                    break;
                case ResourceType.Medicine:
                    if (medicineHQPrefab != null)
                        hqObject = Instantiate(medicineHQPrefab, position, Quaternion.identity);
                    break;
            }

            // Fallback: Create simple colored cube
            if (hqObject == null)
            {
                hqObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                hqObject.transform.position = position;
                hqObject.transform.localScale = new Vector3(tileSize * 0.8f, hqHeight * 2f, tileSize * 0.8f);

                var renderer = hqObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    mat.color = GetColorForResourceType(hqType);
                    renderer.material = mat;
                }
            }

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

using UnityEngine;

namespace UraniumFever.Core
{
    /// <summary>
    /// Visualizes the game grid in the Unity scene.
    /// </summary>
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private int gridWidth = 8;
        [SerializeField] private int gridHeight = 8;
        [SerializeField] private float tileSize = 1f;
        [SerializeField] private float tileSpacing = 0.1f;

        private GridManager _gridManager;
        private GameObject[,] _tileObjects;

        private void Start()
        {
            CreateVisualGrid();
        }

        private void CreateVisualGrid()
        {
            _gridManager = new GridManager();
            _gridManager.CreateGrid(gridWidth, gridHeight);
            _tileObjects = new GameObject[gridWidth, gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    GridTile tile = _gridManager.GetTile(x, y);
                    Vector3 position = new Vector3(
                        x * (tileSize + tileSpacing),
                        0,
                        y * (tileSize + tileSpacing)
                    );

                    GameObject tileObj = CreateTile(position, x, y);
                    _tileObjects[x, y] = tileObj;
                }
            }

            CenterGrid();
        }

        private GameObject CreateTile(Vector3 position, int x, int y)
        {
            GameObject tileObj;

            if (tilePrefab != null)
            {
                tileObj = Instantiate(tilePrefab, position, Quaternion.identity, transform);
            }
            else
            {
                // Create a simple cube if no prefab is assigned
                tileObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tileObj.transform.position = position;
                tileObj.transform.localScale = new Vector3(tileSize, 0.1f, tileSize);
                tileObj.transform.parent = transform;

                // Add some color variation for a checkerboard pattern
                var renderer = tileObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    mat.color = (x + y) % 2 == 0 ? new Color(0.8f, 0.8f, 0.8f) : new Color(0.6f, 0.6f, 0.6f);
                    renderer.material = mat;
                }
            }

            tileObj.name = $"Tile_{x}_{y}";
            return tileObj;
        }

        private void CenterGrid()
        {
            float totalWidth = (gridWidth - 1) * (tileSize + tileSpacing);
            float totalHeight = (gridHeight - 1) * (tileSize + tileSpacing);
            transform.position = new Vector3(-totalWidth / 2f, 0, -totalHeight / 2f);
        }

        public GridManager GetGridManager()
        {
            return _gridManager;
        }

        public GameObject GetTileObject(int x, int y)
        {
            if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                return _tileObjects[x, y];
            return null;
        }
    }
}

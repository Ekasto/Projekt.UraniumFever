using UnityEngine;
using UnityEngine.InputSystem;

namespace UraniumFever.Core
{
    /// <summary>
    /// Handles tile selection and highlighting via mouse interaction using new Input System.
    /// </summary>
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private GridVisualizer gridVisualizer;
        [SerializeField] private Color hoverColor = new Color(1f, 1f, 0.5f, 1f);
        [SerializeField] private Color selectedColor = new Color(0.5f, 1f, 0.5f, 1f);
        [SerializeField] private LayerMask tileLayerMask = ~0;

        private GameObject _hoveredTile;
        private GameObject _selectedTile;
        private Color _originalHoverColor;
        private Color _originalSelectedColor;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;

            if (gridVisualizer == null)
            {
                gridVisualizer = FindFirstObjectByType<GridVisualizer>();
            }
        }

        private void Update()
        {
            HandleHover();
            HandleSelection();
        }

        private void HandleHover()
        {
            if (Mouse.current == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            GameObject newHoveredTile = null;

            if (Physics.Raycast(ray, out hit, 1000f, tileLayerMask))
            {
                // Check if hit object is a tile
                if (hit.collider.gameObject.name.StartsWith("Tile_"))
                {
                    newHoveredTile = hit.collider.gameObject;
                }
            }

            // Clear previous hover if different
            if (_hoveredTile != newHoveredTile && _hoveredTile != null && _hoveredTile != _selectedTile)
            {
                ResetTileColor(_hoveredTile, _originalHoverColor);
            }

            // Apply new hover
            if (newHoveredTile != null && newHoveredTile != _selectedTile)
            {
                var renderer = newHoveredTile.GetComponent<Renderer>();
                if (renderer != null && _hoveredTile != newHoveredTile)
                {
                    _originalHoverColor = renderer.material.color;
                    renderer.material.color = hoverColor;
                }
            }

            _hoveredTile = newHoveredTile;
        }

        private void HandleSelection()
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (_hoveredTile != null)
                {
                    // Clear previous selection
                    if (_selectedTile != null)
                    {
                        ResetTileColor(_selectedTile, _originalSelectedColor);
                    }

                    // Set new selection
                    _selectedTile = _hoveredTile;
                    var renderer = _selectedTile.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        _originalSelectedColor = _originalHoverColor;
                        renderer.material.color = selectedColor;
                    }

                    // Parse tile coordinates from name
                    string[] parts = _selectedTile.name.Split('_');
                    if (parts.Length == 3)
                    {
                        int x = int.Parse(parts[1]);
                        int y = int.Parse(parts[2]);
                        Debug.Log($"Selected tile at ({x}, {y})");
                    }
                }
            }
        }

        private void ResetTileColor(GameObject tile, Color originalColor)
        {
            var renderer = tile.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
        }

        public GameObject GetSelectedTile()
        {
            return _selectedTile;
        }

        public Vector2Int? GetSelectedTileCoordinates()
        {
            if (_selectedTile == null)
                return null;

            string[] parts = _selectedTile.name.Split('_');
            if (parts.Length == 3)
            {
                int x = int.Parse(parts[1]);
                int y = int.Parse(parts[2]);
                return new Vector2Int(x, y);
            }

            return null;
        }
    }
}

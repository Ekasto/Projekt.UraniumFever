using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UraniumFever.Core;

namespace UraniumFever.Game
{
    /// <summary>
    /// Handles input for placing bridges and buildings.
    /// </summary>
    public class BuildingInputHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Camera mainCamera;

        [Header("Placement Settings")]
        [SerializeField] private LayerMask tileLayer;
        [SerializeField] private float raycastDistance = 100f;

        [Header("Ghost Preview Settings")]
        [SerializeField] private Color validPlacementColor = new Color(0f, 1f, 0f, 0.5f); // Green, transparent
        [SerializeField] private Color invalidPlacementColor = new Color(1f, 0f, 0f, 0.5f); // Red, transparent

        private BuildingType? _selectedBuildingType = null;
        private bool _isBridgeMode = false;

        // Ghost preview objects
        private GameObject _ghostBuilding = null;
        private List<GameObject> _ghostBridgePreviews = new List<GameObject>();
        private Vector2Int? _lastHoveredTile = null;
        private bool _lastPlacementWasValid = false;
        private Vector2Int? _hoveredBridgeStart = null;
        private Vector2Int? _hoveredBridgeEnd = null;

        private void Update()
        {
            HandleInput();
            UpdateGhostPreview();
        }

        private void OnDisable()
        {
            ClearGhostObjects();
        }

        private void HandleInput()
        {
            if (gameManager == null || gameManager.GameSetup == null)
                return;

            // Check if keyboard and mouse are available
            if (Keyboard.current == null || Mouse.current == null)
                return;

            // Keyboard shortcuts for building selection
            if (Keyboard.current[Key.B].wasPressedThisFrame)
            {
                SelectBridge();
            }
            else if (Keyboard.current[Key.H].wasPressedThisFrame)
            {
                SelectBuilding(BuildingType.CheapHouse);
            }
            else if (Keyboard.current[Key.F].wasPressedThisFrame)
            {
                SelectBuilding(BuildingType.Factory);
            }
            else if (Keyboard.current[Key.R].wasPressedThisFrame)
            {
                SelectBuilding(BuildingType.Research);
            }
            else if (Keyboard.current[Key.D].wasPressedThisFrame)
            {
                SelectBuilding(BuildingType.Defense);
            }
            else if (Keyboard.current[Key.Escape].wasPressedThisFrame)
            {
                CancelSelection();
            }

            // Mouse click to place - only if placement is valid
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (_lastPlacementWasValid)
                {
                    HandlePlacementClick();
                }
                else
                {
                    Debug.LogWarning("Cannot place here - invalid placement location!");
                }
            }
        }

        private void HandlePlacementClick()
        {
            Vector2Int? tilePos = GetTileUnderMouse();
            if (!tilePos.HasValue)
                return;

            Player currentPlayer = gameManager.GameSetup.GetCurrentPlayer();

            if (_isBridgeMode)
            {
                HandleBridgePlacement(tilePos.Value, currentPlayer);
            }
            else if (_selectedBuildingType.HasValue)
            {
                HandleBuildingPlacement(tilePos.Value, currentPlayer);
            }
        }

        private void HandleBridgePlacement(Vector2Int tilePos, Player currentPlayer)
        {
            // Single click placement - use the hovered bridge preview
            if (!_hoveredBridgeStart.HasValue || !_hoveredBridgeEnd.HasValue)
            {
                Debug.LogWarning("No valid bridge to place!");
                return;
            }

            PlacementResult result = gameManager.BuildingPlacer.TryPlaceBridge(_hoveredBridgeStart.Value, _hoveredBridgeEnd.Value, currentPlayer);

            if (result.Success)
            {
                Debug.Log($"Bridge placed between {_hoveredBridgeStart.Value} and {_hoveredBridgeEnd.Value}!");

                // Refresh edge visualizer
                if (gameManager.GridVisualizer != null)
                {
                    var edgeVis = FindFirstObjectByType<Core.EdgeVisualizer>();
                    if (edgeVis != null)
                    {
                        edgeVis.RefreshAllBridges();
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Cannot place bridge: {result.ErrorMessage}");
            }
        }

        private void HandleBuildingPlacement(Vector2Int tilePos, Player currentPlayer)
        {
            PlacementResult result = gameManager.BuildingPlacer.TryPlaceBuilding(_selectedBuildingType.Value, tilePos, currentPlayer);

            if (result.Success)
            {
                Debug.Log($"{_selectedBuildingType} placed at {tilePos}!");

                // Visual update would go here (create building cube on tile)
                CreateBuildingVisual(tilePos, _selectedBuildingType.Value);

                // Keep the building type selected for quick placement, but clear ghost momentarily
                // Ghost will reappear when mouse moves again
            }
            else
            {
                Debug.LogWarning($"Cannot place building: {result.ErrorMessage}");
            }
        }

        private void CreateBuildingVisual(Vector2Int tilePos, BuildingType buildingType)
        {
            // Simple visualization - create colored cube on tile
            GameObject buildingObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            buildingObj.name = $"Building_{buildingType}_{tilePos}";

            // Position on grid
            if (gameManager.GridVisualizer != null)
            {
                GameObject tileObj = gameManager.GridVisualizer.GetTileObject(tilePos.x, tilePos.y);
                if (tileObj != null)
                {
                    buildingObj.transform.position = tileObj.transform.position + Vector3.up * 0.6f;
                    buildingObj.transform.localScale = new Vector3(0.7f, 1.2f, 0.7f);
                    buildingObj.transform.parent = gameManager.GridVisualizer.transform;

                    // Color by building type
                    var renderer = buildingObj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                        mat.color = GetBuildingColor(buildingType);
                        renderer.material = mat;
                    }
                }
            }
        }

        private Color GetBuildingColor(BuildingType buildingType)
        {
            switch (buildingType)
            {
                case BuildingType.CheapHouse: return Color.blue;
                case BuildingType.Factory: return Color.yellow;
                case BuildingType.Research: return Color.cyan;
                case BuildingType.Defense: return Color.magenta;
                case BuildingType.Car: return Color.white;
                default: return Color.gray;
            }
        }

        private Vector2Int? GetTileUnderMouse()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            if (Mouse.current == null)
                return null;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Parse tile position from GameObject name (format: "Tile_X_Y")
                string tileName = hit.collider.gameObject.name;
                if (tileName.StartsWith("Tile_"))
                {
                    string[] parts = tileName.Split('_');
                    if (parts.Length >= 3)
                    {
                        if (int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
                        {
                            return new Vector2Int(x, y);
                        }
                    }
                }
            }

            return null;
        }

        public void SelectBridge()
        {
            ClearGhostObjects();
            _isBridgeMode = true;
            _selectedBuildingType = null;
            Debug.Log("Bridge mode activated. Hover over edges to preview, click to place. (Press ESC to cancel)");
        }

        public void SelectBuilding(BuildingType buildingType)
        {
            ClearGhostObjects();
            _isBridgeMode = false;
            _selectedBuildingType = buildingType;
            Debug.Log($"{buildingType} selected. Click a tile to place. (Press ESC to cancel)");
        }

        public void CancelSelection()
        {
            _isBridgeMode = false;
            _selectedBuildingType = null;
            ClearGhostObjects();
            Debug.Log("Selection cancelled.");
        }

        private void UpdateGhostPreview()
        {
            if (gameManager == null || gameManager.GameSetup == null)
            {
                ClearGhostObjects();
                return;
            }

            Vector2Int? hoveredTile = GetTileUnderMouse();

            // No tile hovered - clear ghosts
            if (!hoveredTile.HasValue)
            {
                ClearGhostObjects();
                _lastHoveredTile = null;
                _lastPlacementWasValid = false;
                return;
            }

            _lastHoveredTile = hoveredTile;

            Player currentPlayer = gameManager.GameSetup.GetCurrentPlayer();
            if (currentPlayer == null)
                return;

            // Building mode
            if (_selectedBuildingType.HasValue)
            {
                UpdateBuildingGhost(hoveredTile.Value, currentPlayer);
            }
            // Bridge mode
            else if (_isBridgeMode)
            {
                UpdateBridgeGhost(hoveredTile.Value, currentPlayer);
            }
            // Nothing selected - clear ghosts
            else
            {
                ClearGhostObjects();
                _lastPlacementWasValid = false;
            }
        }

        private void UpdateBuildingGhost(Vector2Int tilePos, Player player)
        {
            // Create ghost if doesn't exist
            if (_ghostBuilding == null)
            {
                _ghostBuilding = CreateGhostBuilding(_selectedBuildingType.Value);
            }

            // Position ghost on tile
            if (gameManager.GridVisualizer != null)
            {
                GameObject tileObj = gameManager.GridVisualizer.GetTileObject(tilePos.x, tilePos.y);
                if (tileObj != null)
                {
                    _ghostBuilding.transform.position = tileObj.transform.position + Vector3.up * 0.6f;
                }
            }

            // Check if placement is valid
            var gridManager = gameManager.GridVisualizer.GetGridManager();
            var tile = gridManager.GetTile(tilePos.x, tilePos.y);

            bool isValid = tile != null &&
                           tile.Building == null && // Tile is empty
                           BridgeConnectivity.CanPlaceBuilding(gridManager, tilePos) && // Has adjacent bridge
                           BuildingCostValidator.CanAfford(player, BuildingLibrary.GetCost(_selectedBuildingType.Value)); // Can afford

            _lastPlacementWasValid = isValid;

            // Update color
            UpdateGhostColor(_ghostBuilding, isValid);
        }

        private void UpdateBridgeGhost(Vector2Int tilePos, Player player)
        {
            var gridManager = gameManager.GridVisualizer.GetGridManager();

            // Clear old preview bridges
            foreach (var preview in _ghostBridgePreviews)
            {
                if (preview != null) Destroy(preview);
            }
            _ghostBridgePreviews.Clear();

            // Find the closest edge to the mouse cursor
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            if (mouseWorldPos == Vector3.zero)
            {
                _lastPlacementWasValid = false;
                return;
            }

            // Check all 4 edges around the hovered tile
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(tilePos.x + 1, tilePos.y),  // Right
                new Vector2Int(tilePos.x - 1, tilePos.y),  // Left
                new Vector2Int(tilePos.x, tilePos.y + 1),  // Up
                new Vector2Int(tilePos.x, tilePos.y - 1)   // Down
            };

            float closestDistance = float.MaxValue;
            Vector2Int? closestTile1 = null;
            Vector2Int? closestTile2 = null;

            foreach (Vector2Int targetTile in directions)
            {
                var edge = gridManager.GetEdge(tilePos, targetTile);
                if (edge != null)
                {
                    // Calculate edge midpoint in world space
                    Vector3 edgeMidpoint = edge.GetMidpoint(1f, 0.1f);
                    float distance = Vector3.Distance(mouseWorldPos, edgeMidpoint);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTile1 = tilePos;
                        closestTile2 = targetTile;
                    }
                }
            }

            // Show ghost bridge on the closest edge
            if (closestTile1.HasValue && closestTile2.HasValue)
            {
                var edge = gridManager.GetEdge(closestTile1.Value, closestTile2.Value);
                bool edgeValid = edge != null &&
                                 !edge.HasBridge &&
                                 BridgeConnectivity.CanPlaceBridge(gridManager, closestTile1.Value, closestTile2.Value, player.HQPosition) &&
                                 BuildingCostValidator.CanAfford(player, BuildingLibrary.GetCost(BuildingType.Bridge));

                GameObject previewBridge = CreateGhostBridge();
                PositionGhostBridge(closestTile1.Value, closestTile2.Value, previewBridge);
                UpdateGhostColor(previewBridge, edgeValid);
                _ghostBridgePreviews.Add(previewBridge);

                _hoveredBridgeStart = closestTile1.Value;
                _hoveredBridgeEnd = closestTile2.Value;
                _lastPlacementWasValid = edgeValid;
            }
            else
            {
                _lastPlacementWasValid = false;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            if (Mouse.current == null)
                return Vector3.zero;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // Raycast to find world position on Y=0 plane
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
            {
                return hit.point;
            }

            // Fallback: calculate intersection with Y=0 plane
            float distance = -ray.origin.y / ray.direction.y;
            return ray.origin + ray.direction * distance;
        }

        private GameObject CreateGhostBuilding(BuildingType buildingType)
        {
            GameObject ghost = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ghost.name = "GhostBuilding";
            ghost.transform.localScale = new Vector3(0.7f, 1.2f, 0.7f);

            // Make transparent
            var renderer = ghost.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.SetFloat("_Surface", 1); // Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                mat.renderQueue = 3000;
                renderer.material = mat;
            }

            // Disable collider so it doesn't interfere with raycasts
            var collider = ghost.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            return ghost;
        }

        private GameObject CreateGhostBridge()
        {
            GameObject ghost = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ghost.name = "GhostBridge";

            // Make transparent
            var renderer = ghost.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.SetFloat("_Surface", 1); // Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                mat.renderQueue = 3000;
                renderer.material = mat;
            }

            // Disable collider
            var collider = ghost.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            return ghost;
        }

        private void PositionGhostBridge(Vector2Int tile1, Vector2Int tile2, GameObject ghostBridge)
        {
            if (ghostBridge == null || gameManager.GridVisualizer == null)
                return;

            // Calculate midpoint between tiles
            GameObject tile1Obj = gameManager.GridVisualizer.GetTileObject(tile1.x, tile1.y);
            GameObject tile2Obj = gameManager.GridVisualizer.GetTileObject(tile2.x, tile2.y);

            if (tile1Obj != null && tile2Obj != null)
            {
                Vector3 midpoint = (tile1Obj.transform.position + tile2Obj.transform.position) / 2f;
                midpoint.y = 0.5f; // Slightly above ground

                ghostBridge.transform.position = midpoint;

                // Determine orientation and scale
                bool isHorizontal = tile1.y == tile2.y;
                float bridgeWidth = 0.2f;
                float bridgeHeight = 0.3f;
                float tileSize = 1f;
                float tileSpacing = 0.1f;
                float bridgeLength = (tileSize * 2.0f) + tileSpacing; // Span 2 full tiles PLUS spacing

                if (isHorizontal)
                {
                    ghostBridge.transform.localScale = new Vector3(bridgeLength, bridgeHeight, bridgeWidth);
                }
                else
                {
                    ghostBridge.transform.localScale = new Vector3(bridgeWidth, bridgeHeight, bridgeLength);
                }
            }
        }

        private void UpdateGhostColor(GameObject ghost, bool isValid)
        {
            if (ghost == null)
                return;

            var renderer = ghost.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = isValid ? validPlacementColor : invalidPlacementColor;
                renderer.material.color = color;
            }
        }

        private void ClearGhostObjects()
        {
            if (_ghostBuilding != null)
            {
                Destroy(_ghostBuilding);
                _ghostBuilding = null;
            }

            foreach (var preview in _ghostBridgePreviews)
            {
                if (preview != null) Destroy(preview);
            }
            _ghostBridgePreviews.Clear();

            _lastPlacementWasValid = false;
            _hoveredBridgeStart = null;
            _hoveredBridgeEnd = null;
        }
    }
}

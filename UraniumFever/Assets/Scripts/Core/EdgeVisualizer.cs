using UnityEngine;
using System.Collections.Generic;

namespace UraniumFever.Core
{
    /// <summary>
    /// Visualizes grid edges and bridges in the Unity scene.
    /// </summary>
    public class EdgeVisualizer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float bridgeWidth = 0.2f;
        [SerializeField] private float bridgeHeight = 0.3f;
        [SerializeField] private float tileSize = 1f;
        [SerializeField] private float tileSpacing = 0.1f;

        [Header("Player Colors")]
        [SerializeField] private Color player1Color = new Color(0.2f, 0.8f, 0.2f); // Green
        [SerializeField] private Color player2Color = new Color(1f, 0.9f, 0.2f);   // Yellow
        [SerializeField] private Color player3Color = new Color(1f, 0.3f, 0.3f);   // Red

        private GridManager _gridManager;
        private Dictionary<GridEdge, GameObject> _bridgeObjects = new Dictionary<GridEdge, GameObject>();

        /// <summary>
        /// Initializes the edge visualizer with the grid manager.
        /// </summary>
        public void Initialize(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        /// <summary>
        /// Visualizes a bridge on the specified edge.
        /// </summary>
        public void VisualizeBridge(GridEdge edge)
        {
            if (_bridgeObjects.ContainsKey(edge))
            {
                // Bridge already visualized, update it
                UpdateBridgeVisual(edge);
                return;
            }

            // Create new bridge visual
            GameObject bridgeObj = CreateBridgeObject(edge);
            _bridgeObjects[edge] = bridgeObj;
        }

        /// <summary>
        /// Removes the visual for a bridge on the specified edge.
        /// </summary>
        public void RemoveBridge(GridEdge edge)
        {
            if (_bridgeObjects.TryGetValue(edge, out GameObject bridgeObj))
            {
                Destroy(bridgeObj);
                _bridgeObjects.Remove(edge);
            }
        }

        /// <summary>
        /// Updates all bridge visuals (call after bridges are placed/removed).
        /// </summary>
        public void RefreshAllBridges()
        {
            if (_gridManager == null)
                return;

            // Clear existing visuals
            foreach (var kvp in _bridgeObjects)
            {
                if (kvp.Value != null)
                    Destroy(kvp.Value);
            }
            _bridgeObjects.Clear();

            // Visualize all bridges
            GridEdge[] allEdges = _gridManager.GetAllEdges();
            foreach (GridEdge edge in allEdges)
            {
                if (edge.HasBridge)
                {
                    VisualizeBridge(edge);
                }
            }
        }

        private GameObject CreateBridgeObject(GridEdge edge)
        {
            GameObject bridgeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bridgeObj.name = $"Bridge_{edge.Tile1Position}_{edge.Tile2Position}";
            bridgeObj.transform.parent = transform;

            // Position at center of both tiles
            Vector3 bridgeCenter = edge.GetBridgeCenter(tileSize, tileSpacing);
            bridgeObj.transform.localPosition = bridgeCenter;

            // Scale based on edge orientation
            // Bridge should span 2 full tiles PLUS the spacing between them
            float bridgeLength = (tileSize * 2.0f) + tileSpacing;

            if (edge.IsHorizontal)
            {
                // Horizontal edge: thin in Z direction (perpendicular), long in X (along the two tiles)
                bridgeObj.transform.localScale = new Vector3(bridgeLength, bridgeHeight, bridgeWidth);
            }
            else
            {
                // Vertical edge: long in Z (along the two tiles), thin in X (perpendicular)
                bridgeObj.transform.localScale = new Vector3(bridgeWidth, bridgeHeight, bridgeLength);
            }

            // Apply player color
            var renderer = bridgeObj.GetComponent<Renderer>();
            if (renderer != null && edge.OwnerId != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = GetPlayerColor(edge.OwnerId.Value);
                renderer.material = mat;
            }

            return bridgeObj;
        }

        private void UpdateBridgeVisual(GridEdge edge)
        {
            if (!_bridgeObjects.TryGetValue(edge, out GameObject bridgeObj))
                return;

            // Update color if owner changed
            var renderer = bridgeObj.GetComponent<Renderer>();
            if (renderer != null && edge.OwnerId != null)
            {
                renderer.material.color = GetPlayerColor(edge.OwnerId.Value);
            }
        }

        private Color GetPlayerColor(int playerId)
        {
            switch (playerId)
            {
                case 1: return player1Color;
                case 2: return player2Color;
                case 3: return player3Color;
                default: return Color.white;
            }
        }
    }
}

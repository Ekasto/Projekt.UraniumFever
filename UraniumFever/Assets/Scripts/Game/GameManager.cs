using UnityEngine;

namespace UraniumFever.Game
{
    /// <summary>
    /// Main game manager that coordinates all game systems.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Core.GridVisualizer gridVisualizer;
        [SerializeField] private HQVisualizer hqVisualizer;
        [SerializeField] private Core.EdgeVisualizer edgeVisualizer;

        private GameSetup _gameSetup;
        private BuildingPlacer _buildingPlacer;
        private NetworkManager _networkManager;

        public GameSetup GameSetup => _gameSetup;
        public BuildingPlacer BuildingPlacer => _buildingPlacer;
        public NetworkManager NetworkManager => _networkManager;
        public Core.GridVisualizer GridVisualizer => gridVisualizer;

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            _gameSetup = new GameSetup();
            _gameSetup.Initialize();

            // Initialize building placer
            if (gridVisualizer != null)
            {
                var gridManager = gridVisualizer.GetGridManager();
                _buildingPlacer = new BuildingPlacer(gridManager);
                _networkManager = new NetworkManager(gridManager, _gameSetup.Players);

                // Initialize edge visualizer
                if (edgeVisualizer != null)
                {
                    edgeVisualizer.Initialize(gridManager);
                }
            }

            // Place headquarters on grid
            if (hqVisualizer != null && gridVisualizer != null)
            {
                hqVisualizer.PlaceHeadquarters(_gameSetup.Players, gridVisualizer.transform);
            }

            Debug.Log($"Game initialized: {_gameSetup.Players.Length} players, Round {_gameSetup.RoundManager.CurrentRound}");
            Debug.Log($"Deck: {_gameSetup.Deck.TotalCards} total cards, {_gameSetup.Deck.RemainingCards} remaining");
            Debug.Log($"Player {_gameSetup.PlayerWithStartingCar + 1} starts with car");
        }

        /// <summary>
        /// Draws a card for the current player with building bonuses applied.
        /// </summary>
        public void DrawCardForCurrentPlayer()
        {
            var currentPlayer = _gameSetup.GetCurrentPlayer();
            var isSafeRound = _gameSetup.RoundManager.IsSafeRound();

            // Check if player has Research (draw twice)
            bool hasResearch = _networkManager != null && _networkManager.HasResearchBonus(currentPlayer);
            int drawCount = hasResearch ? 2 : 1;

            for (int i = 0; i < drawCount; i++)
            {
                var card = _gameSetup.Deck.DrawCard(isSafeRound);

                if (card == null)
                {
                    Debug.LogWarning("No cards available to draw!");
                    continue;
                }

                if (card.IsResource)
                {
                    ResourceType resourceType = card.ResourceType.Value;

                    // Add base resource
                    currentPlayer.AddResource(resourceType);
                    int baseAmount = 1;

                    // Apply building bonuses (Factory +4, House +1)
                    int bonus = _networkManager != null ? _networkManager.GetResourceBonus(currentPlayer, resourceType) : 0;

                    for (int b = 0; b < bonus; b++)
                    {
                        currentPlayer.AddResource(resourceType);
                    }

                    int totalGained = baseAmount + bonus;
                    string bonusText = bonus > 0 ? $" (+{bonus} from network)" : "";
                    Debug.Log($"Player {currentPlayer.PlayerId} drew {resourceType} and gained {totalGained} resources{bonusText}. Total: {currentPlayer.GetResourceCount(resourceType)}");
                }
                else if (card.IsDisaster)
                {
                    int defenseReduction = _networkManager != null ? _networkManager.GetDisasterReduction(currentPlayer) : 0;
                    string defenseText = defenseReduction > 0 ? $" (Defense reduces by -{defenseReduction})" : "";
                    Debug.Log($"Player {currentPlayer.PlayerId} drew DISASTER: {card.DisasterType}!{defenseText} (Effects not fully implemented)");
                }

                if (hasResearch && i == 0)
                {
                    Debug.Log("Research bonus: Drawing second card!");
                }
            }

            // Advance turn
            _gameSetup.RoundManager.NextTurn();
            Debug.Log($"Round {_gameSetup.RoundManager.CurrentRound}, Player {_gameSetup.GetCurrentPlayer().PlayerId}'s turn");
        }
    }
}

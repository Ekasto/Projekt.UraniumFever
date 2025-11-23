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

        private GameSetup _gameSetup;

        public GameSetup GameSetup => _gameSetup;

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            _gameSetup = new GameSetup();
            _gameSetup.Initialize();

            // Place headquarters on grid
            if (hqVisualizer != null && gridVisualizer != null)
            {
                hqVisualizer.PlaceHeadquarters(_gameSetup.Players, gridVisualizer.transform);
            }

            Debug.Log($"Game initialized: {_gameSetup.Players.Length} players, Round {_gameSetup.RoundManager.CurrentRound}");
            Debug.Log($"Deck: {_gameSetup.Deck.TotalCards} total cards, {_gameSetup.Deck.RemainingCards} remaining");
        }

        /// <summary>
        /// Draws a card for the current player.
        /// </summary>
        public void DrawCardForCurrentPlayer()
        {
            var currentPlayer = _gameSetup.GetCurrentPlayer();
            var isSafeRound = _gameSetup.RoundManager.IsSafeRound();

            var card = _gameSetup.Deck.DrawCard(isSafeRound);

            if (card == null)
            {
                Debug.LogWarning("No cards available to draw!");
                return;
            }

            if (card.IsResource)
            {
                currentPlayer.AddResource(card.ResourceType.Value);
                Debug.Log($"Player {currentPlayer.PlayerId} drew {card.ResourceType}. Total: {currentPlayer.GetResourceCount(card.ResourceType.Value)}");
            }
            else if (card.IsDisaster)
            {
                Debug.Log($"Player {currentPlayer.PlayerId} drew DISASTER: {card.DisasterType}! (No effects yet in Phase 3)");
            }

            // Advance turn
            _gameSetup.RoundManager.NextTurn();
            Debug.Log($"Round {_gameSetup.RoundManager.CurrentRound}, Player {_gameSetup.GetCurrentPlayer().PlayerId}'s turn");
        }
    }
}

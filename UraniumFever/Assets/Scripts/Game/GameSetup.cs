using UnityEngine;

namespace UraniumFever.Game
{
    /// <summary>
    /// Sets up the game with 3 players, headquarters, deck, and round manager.
    /// </summary>
    public class GameSetup
    {
        public Player[] Players { get; private set; }
        public Deck Deck { get; private set; }
        public RoundManager RoundManager { get; private set; }

        public void Initialize()
        {
            // Create 3 players with their headquarters at edge midpoints
            Players = new Player[3];

            // Player 1: Food HQ at left edge (0, 4)
            Players[0] = new Player(1, ResourceType.Food, new Vector2Int(0, 4));

            // Player 2: Electricity HQ at bottom edge (4, 0)
            Players[1] = new Player(2, ResourceType.Electricity, new Vector2Int(4, 0));

            // Player 3: Medicine HQ at right edge (7, 4)
            Players[2] = new Player(3, ResourceType.Medicine, new Vector2Int(7, 4));

            // Initialize endless deck (70% resources, 30% disasters)
            Deck = new Deck();
            Deck.Initialize(resourceCardCount: 70, disasterCardCount: 30);

            // Initialize round manager (starts at round 1)
            RoundManager = new RoundManager();
        }

        public Player GetCurrentPlayer()
        {
            return Players[RoundManager.CurrentPlayerIndex];
        }
    }
}

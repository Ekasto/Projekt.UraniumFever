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
        public int PlayerWithStartingCar { get; private set; }

        public void Initialize()
        {
            // Create 3 players with their headquarters at edge midpoints (12x12 grid)
            Players = new Player[3];

            // Player 1: Food HQ at left edge (0, 6)
            Players[0] = new Player(1, ResourceType.Food, new Vector2Int(3, 6));

            // Player 2: Electricity HQ at bottom edge (6, 0)
            Players[1] = new Player(2, ResourceType.Electricity, new Vector2Int(6, 3));

            // Player 3: Medicine HQ at right edge (11, 6)
            Players[2] = new Player(3, ResourceType.Medicine, new Vector2Int(8, 6));

            // Initialize endless deck (70% resources, 30% disasters)
            Deck = new Deck();
            Deck.Initialize(resourceCardCount: 70, disasterCardCount: 30);

            // Initialize round manager (starts at round 1)
            RoundManager = new RoundManager();

            // Randomly select which player gets the starting car
            PlayerWithStartingCar = UnityEngine.Random.Range(0, 3);
        }

        public Player GetCurrentPlayer()
        {
            return Players[RoundManager.CurrentPlayerIndex];
        }

        public int GetCardDrawCount(Player player)
        {
            // Base 1 card, +1 for each house the player owns
            // This will be calculated by BuildingPlacer in GameManager
            return 1;
        }
    }
}

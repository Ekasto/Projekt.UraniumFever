namespace UraniumFever.Game
{
    /// <summary>
    /// Manages game rounds and turn order for 3 players.
    /// Tracks safe rounds (1-3) where disasters cannot be drawn.
    /// </summary>
    public class RoundManager
    {
        private const int SAFE_ROUNDS = 3;
        private const int PLAYER_COUNT = 3;

        public int CurrentRound { get; private set; }
        public int CurrentPlayerIndex { get; private set; }

        public RoundManager()
        {
            CurrentRound = 1;
            CurrentPlayerIndex = 0;
        }

        /// <summary>
        /// Returns true if current round is a safe round (1-3).
        /// During safe rounds, only resource cards can be drawn.
        /// </summary>
        public bool IsSafeRound()
        {
            return CurrentRound <= SAFE_ROUNDS;
        }

        /// <summary>
        /// Advances to the next player's turn.
        /// When all players have gone, advances to the next round.
        /// </summary>
        public void NextTurn()
        {
            CurrentPlayerIndex++;

            if (CurrentPlayerIndex >= PLAYER_COUNT)
            {
                CurrentPlayerIndex = 0;
                NextRound();
            }
        }

        /// <summary>
        /// Advances to the next round.
        /// </summary>
        public void NextRound()
        {
            CurrentRound++;
        }
    }
}

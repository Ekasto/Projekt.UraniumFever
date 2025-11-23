using System.Collections.Generic;
using System.Linq;

namespace UraniumFever.Game
{
    /// <summary>
    /// Manages the endless card deck with reshuffle mechanic.
    /// </summary>
    public class Deck
    {
        private List<Card> _drawPile;
        private List<Card> _discardPile;
        private System.Random _random;

        public int TotalCards { get; private set; }
        public int RemainingCards => _drawPile.Count;
        public int DiscardedCards => _discardPile.Count;

        public Deck()
        {
            _drawPile = new List<Card>();
            _discardPile = new List<Card>();
            _random = new System.Random();
        }

        /// <summary>
        /// Initializes the deck with resource and disaster cards.
        /// </summary>
        public void Initialize(int resourceCardCount, int disasterCardCount)
        {
            _drawPile.Clear();
            _discardPile.Clear();

            // Add resource cards (evenly distributed among 4 types)
            var resourceTypes = new[] { ResourceType.Electricity, ResourceType.Food, ResourceType.Medicine, ResourceType.PlayerChoice };
            int cardsPerType = resourceCardCount / resourceTypes.Length;
            int remainder = resourceCardCount % resourceTypes.Length;

            foreach (var resourceType in resourceTypes)
            {
                int count = cardsPerType + (remainder > 0 ? 1 : 0);
                remainder--;

                for (int i = 0; i < count; i++)
                {
                    _drawPile.Add(Card.CreateResourceCard(resourceType));
                }
            }

            // Add disaster cards (evenly distributed among 5 types)
            var disasterTypes = new[] { DisasterType.Earthquake, DisasterType.Flood, DisasterType.Tornado, DisasterType.Thief, DisasterType.Donkey };
            int disastersPerType = disasterCardCount / disasterTypes.Length;
            int disasterRemainder = disasterCardCount % disasterTypes.Length;

            foreach (var disasterType in disasterTypes)
            {
                int count = disastersPerType + (disasterRemainder > 0 ? 1 : 0);
                disasterRemainder--;

                for (int i = 0; i < count; i++)
                {
                    _drawPile.Add(Card.CreateDisasterCard(disasterType));
                }
            }

            TotalCards = _drawPile.Count;
            Shuffle();
        }

        /// <summary>
        /// Draws a card from the deck. If safe round, only resource cards are returned.
        /// Automatically reshuffles discard pile when draw pile is empty.
        /// </summary>
        public Card DrawCard(bool isSafeRound)
        {
            // Reshuffle if needed
            if (_drawPile.Count == 0)
            {
                ReshuffleDiscardPile();
            }

            // If still no cards (shouldn't happen in normal gameplay), return null
            if (_drawPile.Count == 0)
                return null;

            Card drawnCard;

            if (isSafeRound)
            {
                // Safe round: only draw resource cards
                // Find a resource card in the draw pile
                var resourceCards = _drawPile.Where(c => c.IsResource).ToList();

                if (resourceCards.Count == 0)
                {
                    // No resource cards in draw pile, try reshuffling
                    ReshuffleDiscardPile();
                    resourceCards = _drawPile.Where(c => c.IsResource).ToList();

                    if (resourceCards.Count == 0)
                        return null; // No resource cards available at all
                }

                // Pick a random resource card
                int index = _random.Next(resourceCards.Count);
                drawnCard = resourceCards[index];
                _drawPile.Remove(drawnCard);
            }
            else
            {
                // Not safe round: draw from top
                drawnCard = _drawPile[0];
                _drawPile.RemoveAt(0);
            }

            _discardPile.Add(drawnCard);
            return drawnCard;
        }

        private void Shuffle()
        {
            // Fisher-Yates shuffle
            int n = _drawPile.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                Card temp = _drawPile[k];
                _drawPile[k] = _drawPile[n];
                _drawPile[n] = temp;
            }
        }

        private void ReshuffleDiscardPile()
        {
            if (_discardPile.Count == 0)
                return;

            // Move all discard cards back to draw pile
            _drawPile.AddRange(_discardPile);
            _discardPile.Clear();

            // Shuffle the draw pile
            Shuffle();
        }
    }
}

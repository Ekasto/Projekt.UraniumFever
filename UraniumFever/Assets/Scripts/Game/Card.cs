namespace UraniumFever.Game
{
    /// <summary>
    /// Represents a card that can be drawn from the deck (either Resource or Disaster).
    /// </summary>
    public class Card
    {
        public CardType Type { get; private set; }
        public ResourceType? ResourceType { get; private set; }
        public DisasterType? DisasterType { get; private set; }
        public int Value { get; private set; }

        public bool IsResource => Type == CardType.Resource;
        public bool IsDisaster => Type == CardType.Disaster;

        private Card() { }

        public static Card CreateResourceCard(ResourceType resourceType, int value)
        {
            return new Card
            {
                Type = CardType.Resource,
                ResourceType = resourceType,
                DisasterType = null,
                Value = value
            };
        }

        public static Card CreateDisasterCard(DisasterType disasterType)
        {
            return new Card
            {
                Type = CardType.Disaster,
                ResourceType = null,
                DisasterType = disasterType,
                Value = 0
            };
        }

        public override string ToString()
        {
            if (IsResource)
                return $"Resource: {ResourceType}";
            else
                return $"Disaster: {DisasterType}";
        }
    }
}

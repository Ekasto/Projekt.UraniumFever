namespace UraniumFever.Game
{
    /// <summary>
    /// Validates and deducts building costs from player inventory.
    /// Handles PlayerChoice resources which can act as primary or secondary.
    /// </summary>
    public static class BuildingCostValidator
    {
        public static bool CanAfford(Player player, BuildingCost cost)
        {
            if (cost.TotalCost == 0)
                return true; // Free building

            // Count available resources
            int primaryAvailable = player.GetResourceCount(player.HQType);
            int playerChoiceAvailable = player.GetResourceCount(ResourceType.PlayerChoice);
            int secondaryAvailable = CountSecondaryResources(player);

            // Check if we can satisfy primary requirement
            int primaryNeeded = cost.PrimaryCount;
            int primaryFromHQ = System.Math.Min(primaryAvailable, primaryNeeded);
            int primaryStillNeeded = primaryNeeded - primaryFromHQ;

            // Use PlayerChoice for remaining primary if needed
            int playerChoiceForPrimary = System.Math.Min(playerChoiceAvailable, primaryStillNeeded);
            primaryStillNeeded -= playerChoiceForPrimary;

            if (primaryStillNeeded > 0)
                return false; // Can't afford primary cost

            // Check if we can satisfy secondary requirement
            int secondaryNeeded = cost.SecondaryCount;
            int secondaryFromOthers = System.Math.Min(secondaryAvailable, secondaryNeeded);
            int secondaryStillNeeded = secondaryNeeded - secondaryFromOthers;

            // Use remaining PlayerChoice for secondary if needed
            int playerChoiceRemaining = playerChoiceAvailable - playerChoiceForPrimary;
            int secondaryFromPlayerChoice = System.Math.Min(playerChoiceRemaining, secondaryStillNeeded);
            secondaryStillNeeded -= secondaryFromPlayerChoice;

            return secondaryStillNeeded == 0; // Can afford if all requirements met
        }

        public static void DeductCost(Player player, BuildingCost cost)
        {
            if (cost.TotalCost == 0)
                return; // Free building, nothing to deduct

            // Deduct primary resources
            int primaryNeeded = cost.PrimaryCount;
            int primaryFromHQ = System.Math.Min(player.GetResourceCount(player.HQType), primaryNeeded);
            player.RemoveResource(player.HQType, primaryFromHQ);
            primaryNeeded -= primaryFromHQ;

            // Deduct PlayerChoice for remaining primary
            if (primaryNeeded > 0)
            {
                int playerChoiceForPrimary = System.Math.Min(player.GetResourceCount(ResourceType.PlayerChoice), primaryNeeded);
                player.RemoveResource(ResourceType.PlayerChoice, playerChoiceForPrimary);
                primaryNeeded -= playerChoiceForPrimary;
            }

            // Deduct secondary resources (non-HQ, non-PlayerChoice)
            int secondaryNeeded = cost.SecondaryCount;
            secondaryNeeded = DeductSecondaryResources(player, secondaryNeeded);

            // Deduct remaining PlayerChoice for secondary
            if (secondaryNeeded > 0)
            {
                player.RemoveResource(ResourceType.PlayerChoice, secondaryNeeded);
            }
        }

        private static int CountSecondaryResources(Player player)
        {
            int count = 0;
            var allTypes = new[] { ResourceType.Electricity, ResourceType.Food, ResourceType.Medicine };

            foreach (var type in allTypes)
            {
                if (type != player.HQType)
                {
                    count += player.GetResourceCount(type);
                }
            }

            return count;
        }

        private static int DeductSecondaryResources(Player player, int amountNeeded)
        {
            var allTypes = new[] { ResourceType.Electricity, ResourceType.Food, ResourceType.Medicine };

            foreach (var type in allTypes)
            {
                if (type != player.HQType && amountNeeded > 0)
                {
                    int available = player.GetResourceCount(type);
                    int toRemove = System.Math.Min(available, amountNeeded);
                    player.RemoveResource(type, toRemove);
                    amountNeeded -= toRemove;
                }
            }

            return amountNeeded; // Return remaining amount still needed
        }
    }
}

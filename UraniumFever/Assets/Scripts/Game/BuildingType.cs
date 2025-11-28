namespace UraniumFever.Game
{
    /// <summary>
    /// Types of buildings that can be constructed in Uranium Fever.
    /// </summary>
    public enum BuildingType
    {
        Bridge,          // Placed on grid edges, opens 4 adjacent tiles (2 primary + 1 secondary)
        Defense,         // Reduces disaster strength by -1 (7 primary + 1 secondary)
        Factory,         // +4 resources when drawing (2 primary + 1 secondary)
        Research,        // Draw twice per turn (5 primary + 1 secondary)
        CheapHouse,      // +1 resources when drawing (2 primary + 1 secondary)
        Car,             // Resource transport (free)
        BrokenCar,       // Spawned at start, must be repaired (3 primary + 1 secondary to repair)
        HouseUpgrade     // Makes building immune to earthquakes & tornadoes (3 primary + 1 secondary)
    }
}

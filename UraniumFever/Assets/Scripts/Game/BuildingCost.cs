namespace UraniumFever.Game
{
    /// <summary>
    /// Represents the resource cost of a building.
    /// Primary resources = player's HQ type (or PlayerChoice)
    /// Secondary resources = any other resource types
    /// </summary>
    public class BuildingCost
    {
        public int PrimaryCount { get; private set; }
        public int SecondaryCount { get; private set; }
        public int TotalCost => PrimaryCount + SecondaryCount;

        public BuildingCost(int primaryCount, int secondaryCount)
        {
            PrimaryCount = primaryCount;
            SecondaryCount = secondaryCount;
        }

        public static BuildingCost Free()
        {
            return new BuildingCost(0, 0);
        }
    }
}

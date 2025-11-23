namespace UraniumFever.Game
{
    /// <summary>
    /// Types of disasters that can occur in Uranium Fever.
    /// </summary>
    public enum DisasterType
    {
        Earthquake,  // Zerstört Straßen/Brücken (Destroys roads/bridges)
        Flood,       // Blockiert (Blocks areas)
        Tornado,     // Zerstört Gebäude (Destroys buildings)
        Thief,       // Stiehlt Ressourcen (Steals resources)
        Donkey       // Esel/Joker - Zerstört Gebäude nach Würfel (Destroys buildings by dice)
    }
}

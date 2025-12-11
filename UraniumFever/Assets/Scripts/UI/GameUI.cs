using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UraniumFever.UI
{
    /// <summary>
    /// Displays game information (round, current player, inventories, draw button).
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Game.GameManager gameManager;
        [SerializeField] private Button drawCardButton;
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private TextMeshProUGUI currentPlayerText;
        [SerializeField] private TextMeshProUGUI[] playerInventoryTexts; // 3 text fields for 3 players
        [SerializeField] private TextMeshProUGUI deckInfoText;

        [Header("UI Styling")]
        [SerializeField] private bool useColoredText = true;
        [SerializeField] private bool showResourceIcons = true;

        private void Start()
        {
            if (drawCardButton != null)
            {
                drawCardButton.onClick.AddListener(OnDrawCardClicked);
                Debug.Log("GameUI: Draw card button listener added");
            }
            else
            {
                Debug.LogError("GameUI: Draw card button reference is missing! Please assign it in the Inspector.");
            }

            if (gameManager == null)
            {
                Debug.LogError("GameUI: GameManager reference is missing! Please assign it in the Inspector.");
            }

            // Initial update
            Invoke(nameof(UpdateUI), 0.1f); // Delay to let GameManager initialize
        }

        private void OnDrawCardClicked()
        {
            Debug.Log("GameUI: Draw card button clicked!");

            if (gameManager == null)
            {
                Debug.LogError("GameUI: Cannot draw card - gameManager is null!");
                return;
            }

            if (gameManager.GameSetup == null)
            {
                Debug.LogError("GameUI: Cannot draw card - GameSetup is null! Has the game initialized?");
                return;
            }

            gameManager.DrawCardForCurrentPlayer();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (gameManager == null || gameManager.GameSetup == null)
                return;

            var setup = gameManager.GameSetup;
            var currentPlayer = setup.GetCurrentPlayer();

            // Update round
            if (roundText != null)
            {
                var safeIndicator = setup.RoundManager.IsSafeRound() ? " (SAFE)" : "";
                roundText.text = $"Round: {setup.RoundManager.CurrentRound}{safeIndicator}";
            }

            // Update current player
            if (currentPlayerText != null)
            {
                currentPlayerText.text = $"Current Player: {currentPlayer.PlayerId} ({currentPlayer.HQType})";
            }

            // Update player inventories
            if (playerInventoryTexts != null && playerInventoryTexts.Length >= 3)
            {
                for (int i = 0; i < setup.Players.Length; i++)
                {
                    var player = setup.Players[i];
                    bool isCurrentPlayer = player.PlayerId == currentPlayer.PlayerId;

                    string text = FormatPlayerInventory(player, isCurrentPlayer);

                    if (playerInventoryTexts[i] != null)
                    {
                        playerInventoryTexts[i].text = text;
                    }
                }
            }

            // Update deck info
            if (deckInfoText != null)
            {
                deckInfoText.text = $"<b>Deck:</b> {setup.Deck.RemainingCards} cards  |  <b>Discard:</b> {setup.Deck.DiscardedCards}";
            }
        }

        private string FormatPlayerInventory(Game.Player player, bool isCurrentPlayer)
        {
            string headerColor = isCurrentPlayer ? "yellow" : "white";
            string text = $"<color={headerColor}><b>Player {player.PlayerId}</b> ({player.HQType})</color>\n";

            text += "<size=90%>"; // Slightly smaller resource text

            if (showResourceIcons)
            {
                text += FormatResourceLine("⚡", "Electricity", Game.ResourceType.Electricity, player);
                text += FormatResourceLine("🍎", "Food", Game.ResourceType.Food, player);
                text += FormatResourceLine("💊", "Medicine", Game.ResourceType.Medicine, player);
                text += FormatResourceLine("⭐", "Choice", Game.ResourceType.PlayerChoice, player);
            }
            else
            {
                text += $"Electricity: {player.GetResourceCount(Game.ResourceType.Electricity)}\n";
                text += $"Food: {player.GetResourceCount(Game.ResourceType.Food)}\n";
                text += $"Medicine: {player.GetResourceCount(Game.ResourceType.Medicine)}\n";
                text += $"Choice: {player.GetResourceCount(Game.ResourceType.PlayerChoice)}";
            }

            text += "</size>";

            return text;
        }

        private string FormatResourceLine(string icon, string name, Game.ResourceType resourceType, Game.Player player)
        {
            int count = player.GetResourceCount(resourceType);
            string colorHex = GetResourceColorHex(resourceType);

            if (useColoredText)
            {
                return $"<color={colorHex}>{icon} {name}:</color> <b>{count}</b>\n";
            }
            else
            {
                return $"{icon} {name}: {count}\n";
            }
        }

        private string GetResourceColorHex(Game.ResourceType resourceType)
        {
            switch (resourceType)
            {
                case Game.ResourceType.Electricity:
                    return "#FFE800"; // Bright yellow
                case Game.ResourceType.Food:
                    return "#44FF44"; // Bright green
                case Game.ResourceType.Medicine:
                    return "#FF4444"; // Bright red
                case Game.ResourceType.PlayerChoice:
                    return "#BB88FF"; // Purple
                default:
                    return "#FFFFFF"; // White
            }
        }
    }
}

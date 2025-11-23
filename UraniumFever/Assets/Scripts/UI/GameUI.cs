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

        private void Start()
        {
            if (drawCardButton != null)
            {
                drawCardButton.onClick.AddListener(OnDrawCardClicked);
            }

            // Initial update
            Invoke(nameof(UpdateUI), 0.1f); // Delay to let GameManager initialize
        }

        private void OnDrawCardClicked()
        {
            if (gameManager != null && gameManager.GameSetup != null)
            {
                gameManager.DrawCardForCurrentPlayer();
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (gameManager == null || gameManager.GameSetup == null)
                return;

            var setup = gameManager.GameSetup;

            // Update round
            if (roundText != null)
            {
                var safeIndicator = setup.RoundManager.IsSafeRound() ? " (SAFE)" : "";
                roundText.text = $"Round: {setup.RoundManager.CurrentRound}{safeIndicator}";
            }

            // Update current player
            if (currentPlayerText != null)
            {
                var currentPlayer = setup.GetCurrentPlayer();
                currentPlayerText.text = $"Current Player: {currentPlayer.PlayerId} ({currentPlayer.HQType})";
            }

            // Update player inventories
            if (playerInventoryTexts != null && playerInventoryTexts.Length >= 3)
            {
                for (int i = 0; i < setup.Players.Length; i++)
                {
                    var player = setup.Players[i];
                    var text = $"Player {player.PlayerId} ({player.HQType}):\n";
                    text += $"⚡{player.GetResourceCount(Game.ResourceType.Electricity)} ";
                    text += $"🍎{player.GetResourceCount(Game.ResourceType.Food)} ";
                    text += $"💊{player.GetResourceCount(Game.ResourceType.Medicine)} ";
                    text += $"⭐{player.GetResourceCount(Game.ResourceType.PlayerChoice)}";

                    if (playerInventoryTexts[i] != null)
                    {
                        playerInventoryTexts[i].text = text;
                    }
                }
            }

            // Update deck info
            if (deckInfoText != null)
            {
                deckInfoText.text = $"Deck: {setup.Deck.RemainingCards} cards | Discard: {setup.Deck.DiscardedCards}";
            }
        }
    }
}

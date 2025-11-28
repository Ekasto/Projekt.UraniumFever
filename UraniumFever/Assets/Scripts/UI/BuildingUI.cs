using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UraniumFever.Game;

namespace UraniumFever.UI
{
    /// <summary>
    /// UI for building selection and placement.
    /// </summary>
    public class BuildingUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private BuildingInputHandler inputHandler;

        [Header("Menu")]
        [SerializeField] private GameObject buildingMenuPanel;
        [SerializeField] private Button toggleMenuButton;

        [Header("Building Buttons")]
        [SerializeField] private Button bridgeButton;
        [SerializeField] private Button houseButton;
        [SerializeField] private Button factoryButton;
        [SerializeField] private Button researchButton;
        [SerializeField] private Button defenseButton;
        [SerializeField] private Button cancelButton;

        [Header("Button Labels (Optional - for cost display)")]
        [SerializeField] private TextMeshProUGUI bridgeButtonText;
        [SerializeField] private TextMeshProUGUI houseButtonText;
        [SerializeField] private TextMeshProUGUI factoryButtonText;
        [SerializeField] private TextMeshProUGUI researchButtonText;
        [SerializeField] private TextMeshProUGUI defenseButtonText;

        [Header("Info Display")]
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private TextMeshProUGUI networkBonusText;
        [SerializeField] private TextMeshProUGUI selectedBuildingInfo;

        private void Start()
        {
            SetupButtons();
            UpdateButtonLabels();

            // Hide building menu by default
            if (buildingMenuPanel != null)
                buildingMenuPanel.SetActive(false);
        }

        private void SetupButtons()
        {
            if (toggleMenuButton != null)
                toggleMenuButton.onClick.AddListener(ToggleBuildingMenu);

            if (bridgeButton != null)
                bridgeButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.Bridge));

            if (houseButton != null)
                houseButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.CheapHouse));

            if (factoryButton != null)
                factoryButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.Factory));

            if (researchButton != null)
                researchButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.Research));

            if (defenseButton != null)
                defenseButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.Defense));

            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancelClicked);
        }

        public void ToggleBuildingMenu()
        {
            if (buildingMenuPanel != null)
            {
                buildingMenuPanel.SetActive(!buildingMenuPanel.activeSelf);
            }
        }

        private void Update()
        {
            UpdateInfoText();
            UpdateNetworkBonus();
            UpdateSelectedBuildingInfo();
            UpdateButtonStates();
        }

        private void OnBuildingSelected(BuildingType buildingType)
        {
            if (inputHandler == null)
                return;

            if (buildingType == BuildingType.Bridge)
            {
                inputHandler.SelectBridge();
            }
            else
            {
                inputHandler.SelectBuilding(buildingType);
            }

            // Close menu after selection
            if (buildingMenuPanel != null)
                buildingMenuPanel.SetActive(false);
        }

        private void UpdateInfoText()
        {
            if (infoText == null || gameManager == null || gameManager.GameSetup == null)
                return;

            Player currentPlayer = gameManager.GameSetup.GetCurrentPlayer();
            if (currentPlayer == null)
                return;

            string text = $"Player {currentPlayer.PlayerId} ({currentPlayer.HQType})\n";
            text += $"Resources:\n";
            text += $"  Food: {currentPlayer.GetResourceCount(ResourceType.Food)}\n";
            text += $"  Electricity: {currentPlayer.GetResourceCount(ResourceType.Electricity)}\n";
            text += $"  Medicine: {currentPlayer.GetResourceCount(ResourceType.Medicine)}\n";
            text += $"  Choice: {currentPlayer.GetResourceCount(ResourceType.PlayerChoice)}\n";
            text += $"\nRound: {gameManager.GameSetup.RoundManager.CurrentRound}";

            infoText.text = text;
        }

        private void UpdateNetworkBonus()
        {
            if (networkBonusText == null || gameManager == null || gameManager.NetworkManager == null || gameManager.GameSetup == null)
                return;

            Player currentPlayer = gameManager.GameSetup.GetCurrentPlayer();
            if (currentPlayer == null)
                return;

            string bonusText = gameManager.NetworkManager.GetBonusDescription(currentPlayer);
            networkBonusText.text = $"Network Bonuses:\n{bonusText}";
        }

        public void OnDrawCardClicked()
        {
            if (gameManager != null)
            {
                gameManager.DrawCardForCurrentPlayer();
            }
        }

        public void OnCancelClicked()
        {
            if (inputHandler != null)
            {
                inputHandler.CancelSelection();
            }
        }

        private void UpdateButtonLabels()
        {
            UpdateButtonLabel(bridgeButtonText, BuildingType.Bridge, "Bridge");
            UpdateButtonLabel(houseButtonText, BuildingType.CheapHouse, "House");
            UpdateButtonLabel(factoryButtonText, BuildingType.Factory, "Factory");
            UpdateButtonLabel(researchButtonText, BuildingType.Research, "Research");
            UpdateButtonLabel(defenseButtonText, BuildingType.Defense, "Defense");
        }

        private void UpdateButtonLabel(TextMeshProUGUI labelText, BuildingType buildingType, string buildingName)
        {
            if (labelText == null) return;

            BuildingCost cost = BuildingLibrary.GetCost(buildingType);
            string costText = $"{buildingName}\n";

            if (cost != null)
            {
                costText += $"Cost: {cost.TotalCost}\n";
                costText += $"Primary: {cost.PrimaryCount}";
            }

            labelText.text = costText;
        }

        private void UpdateSelectedBuildingInfo()
        {
            if (selectedBuildingInfo == null || inputHandler == null)
                return;

            // Check if a building is selected via reflection (since fields are private)
            var selectedTypeField = inputHandler.GetType().GetField("_selectedBuildingType",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var isBridgeModeField = inputHandler.GetType().GetField("_isBridgeMode",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (selectedTypeField != null && isBridgeModeField != null)
            {
                var selectedType = selectedTypeField.GetValue(inputHandler) as BuildingType?;
                var isBridgeMode = (bool)isBridgeModeField.GetValue(inputHandler);

                if (isBridgeMode)
                {
                    selectedBuildingInfo.text = "<color=yellow>BRIDGE MODE ACTIVE</color>\nHover near tile edges to place\nPress ESC or Cancel to exit";
                }
                else if (selectedType.HasValue)
                {
                    BuildingCost cost = BuildingLibrary.GetCost(selectedType.Value);
                    string info = $"<color=yellow>SELECTED: {selectedType.Value}</color>\n";
                    info += $"Total Cost: {cost.TotalCost} ({cost.PrimaryCount} primary)\n";
                    info += "Click tile to place | ESC to cancel";
                    selectedBuildingInfo.text = info;
                }
                else
                {
                    selectedBuildingInfo.text = "No building selected\nPress B to toggle menu";
                }
            }
        }

        private void UpdateButtonStates()
        {
            if (gameManager == null || gameManager.GameSetup == null)
                return;

            Player currentPlayer = gameManager.GameSetup.GetCurrentPlayer();
            if (currentPlayer == null)
                return;

            // Update button interactability based on affordability
            UpdateButtonState(bridgeButton, BuildingType.Bridge, currentPlayer);
            UpdateButtonState(houseButton, BuildingType.CheapHouse, currentPlayer);
            UpdateButtonState(factoryButton, BuildingType.Factory, currentPlayer);
            UpdateButtonState(researchButton, BuildingType.Research, currentPlayer);
            UpdateButtonState(defenseButton, BuildingType.Defense, currentPlayer);
        }

        private void UpdateButtonState(Button button, BuildingType buildingType, Player player)
        {
            if (button == null) return;

            BuildingCost cost = BuildingLibrary.GetCost(buildingType);
            bool canAfford = BuildingCostValidator.CanAfford(player, cost);

            button.interactable = canAfford;

            // Update visual feedback
            var colors = button.colors;
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            button.colors = colors;
        }
    }
}

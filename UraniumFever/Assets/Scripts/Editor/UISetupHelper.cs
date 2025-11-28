using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using UraniumFever.UI;

namespace UraniumFever.Editor
{
    public class UISetupHelper : MonoBehaviour
    {
        [MenuItem("Tools/Setup UI References")]
        public static void SetupUIReferences()
        {
            // Find the BuildingUI component
            BuildingUI buildingUI = FindObjectOfType<BuildingUI>();
            if (buildingUI == null)
            {
                Debug.LogError("BuildingUI component not found in scene!");
                return;
            }

            // Find and assign Cancel Button
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (var button in buttons)
            {
                if (button.name == "CancelButton")
                {
                    SerializedObject so = new SerializedObject(buildingUI);
                    SerializedProperty prop = so.FindProperty("cancelButton");
                    prop.objectReferenceValue = button;
                    so.ApplyModifiedProperties();
                    Debug.Log("Assigned CancelButton");
                    break;
                }
            }

            // Find and assign SelectedBuildingInfo
            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
            foreach (var text in texts)
            {
                if (text.name == "SelectedBuildingInfo")
                {
                    SerializedObject so = new SerializedObject(buildingUI);
                    SerializedProperty prop = so.FindProperty("selectedBuildingInfo");
                    prop.objectReferenceValue = text;
                    so.ApplyModifiedProperties();
                    Debug.Log("Assigned SelectedBuildingInfo");
                    break;
                }
            }

            // Assign button text fields
            AssignButtonText(buildingUI, "BridgeButton", "bridgeButtonText");
            AssignButtonText(buildingUI, "HouseButton", "houseButtonText");
            AssignButtonText(buildingUI, "FactoryButton", "factoryButtonText");
            AssignButtonText(buildingUI, "ResearchButton", "researchButtonText");
            AssignButtonText(buildingUI, "DefenseButton", "defenseButtonText");

            Debug.Log("UI Setup Complete!");
        }

        private static void AssignButtonText(BuildingUI buildingUI, string buttonName, string propertyName)
        {
            Button[] buttons = FindObjectsOfType<Button>();
            foreach (var button in buttons)
            {
                if (button.name == buttonName)
                {
                    TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        SerializedObject so = new SerializedObject(buildingUI);
                        SerializedProperty prop = so.FindProperty(propertyName);
                        prop.objectReferenceValue = textComponent;
                        so.ApplyModifiedProperties();
                        Debug.Log($"Assigned {propertyName} from {buttonName}");
                    }
                    break;
                }
            }
        }
    }
}

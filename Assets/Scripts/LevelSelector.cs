
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    
    private GameObject layout;
    [SerializeField] Button buttonObject;
    List<string> levelCount;
 
    private void Start()
    {
        string path = "Assets/Prefabs/ScriptableObj/";
        levelCount = AssetDatabase.FindAssets("", new[] { path }).ToList();
        layout = GameObject.Find("Layout");
  

        PopulateGrid();
        
    }

    void PopulateGrid()
    {
        for (int i = 0; i < levelCount.Count; i++)
        {
            Button button = Instantiate(buttonObject, layout.transform);
            button.name = $"Level {i+1}";
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {i+1}";
            int currentIndex = i;
            button.onClick.AddListener(() =>
            {
            
                string assetPath = AssetDatabase.GUIDToAssetPath(levelCount[currentIndex]);    
                LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
                LevelManager.Instance.SetSelectedLevel(levelData);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            });
        }
        //AdjustLayoutSize();
    }
    void AdjustLayoutSize()
    {
        // Get the RectTransform of the layout
        RectTransform layoutRect = layout.GetComponent<RectTransform>();

        // Get the number of buttons
        int buttonCount = layout.transform.childCount;
        if (buttonCount > 0)
        {
            // Get the size of the button
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();

            // Example: Set the desired number of buttons per row
            int buttonsPerRow = 2; // Change this as needed for your design

            // Calculate the new width and height based on button dimensions
            float buttonHeight = buttonRect.rect.height;
            float buttonWidth = buttonRect.rect.width;

            // Calculate the new width and height for the layout
            float newWidth = buttonsPerRow * buttonWidth; // Total width for the number of buttons in a row
            float newHeight = Mathf.Ceil((float)buttonCount / buttonsPerRow) * buttonHeight; // Calculate height based on rows

            // Update the layout RectTransform size
            layoutRect.sizeDelta = new Vector2(newWidth, newHeight);
        }
    }
}
    


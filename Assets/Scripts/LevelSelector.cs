
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
    /*
     * Creates a button depending the number of levels 
     */

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

    }
    
}
    


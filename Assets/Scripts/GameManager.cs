
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] TextMeshProUGUI result;
    Animator animator;
    private int numberOfOptions;
    private GameObject layout;
    private Button buttonObject;
    LevelData levelData;
    private Dictionary<int, bool> buttonStates;
    [SerializeField] Animator panelAnimator;


    private void Start()
    {
        levelData = LevelManager.Instance.SelectedLevelData;
        if (levelData.overrideController != null)
        {
            panelAnimator.runtimeAnimatorController = levelData.overrideController;
            Debug.Log("Override Controller Assigned!");
        }
        else
        {
            Debug.LogError("Override Controller not assigned in LevelData!");
        }
        buttonStates = new Dictionary<int, bool>();
        numberOfOptions = LevelManager.Instance.SelectedLevelData.words.Count;
        buttonObject = GameObject.Find("ButtonHolder").GetComponentInChildren<Button>();
        question.text = LevelManager.Instance.SelectedLevelData.question;
        question.color = Color.yellow;
        layout = GameObject.Find("Layout");

        PopulateGrid();

        
        
    }


   

    void PopulateGrid()
    {
        /*
         * Populating the buttons based on the number of options
         */

        for(int i = 0; i < numberOfOptions; i++)
        {
            Button button = Instantiate(buttonObject,layout.transform);
            button.name = "Button " + i;
            button.GetComponentInChildren<TextMeshProUGUI>().text = LevelManager.Instance.SelectedLevelData.words[i];
            buttonStates[i] = false;
            int selectedIndex = i;
            button.onClick.AddListener(() =>
            {
                buttonStates[selectedIndex] = !buttonStates[selectedIndex];
                button.GetComponent<Image>().color = buttonStates[selectedIndex]? Color.grey:Color.white; // if selected change the button color from white to grey or vise versa
            });
        }
    }
    /*
     * when player presses check button it basically checks if the selected buttons and answers match if yes different animation is played 
     */
    public void CheckAnswer()
    {
        int matchedCount = 0;
        int selectedCount = 0;

        for(int i = 0; i < numberOfOptions;i++)
        {
            if (buttonStates[i])
            {
                selectedCount++;
                if(LevelManager.Instance.SelectedLevelData.answers[i])
                    matchedCount++;
            }
        }
        int correctAnswersCount = LevelManager.Instance.SelectedLevelData.answers.Count(a => a);
        if (matchedCount == correctAnswersCount && matchedCount == selectedCount)
        {
            result.text = "Correct...";
            result.color = Color.green;
            panelAnimator.SetBool("pass", true);
        }
        else
        {
            result.text = "Wrong...";
            result.color = Color.red;
            panelAnimator.SetBool("pass", false) ;

        }
            
    }
}

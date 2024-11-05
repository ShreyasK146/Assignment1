
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


    private void Start()
    {
        animator = GameObject.Find("Panel").GetComponent<Animator>();    
        buttonStates = new Dictionary<int, bool>();
        numberOfOptions = LevelManager.Instance.SelectedLevelData.words.Count;
        buttonObject = GameObject.Find("ButtonHolder").GetComponentInChildren<Button>();
        question.text = LevelManager.Instance.SelectedLevelData.question;
        layout = GameObject.Find("Layout");
   
        PopulateGrid();
    }
   
    void PopulateGrid()
    {
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
                button.GetComponent<Image>().color = buttonStates[selectedIndex]? Color.grey:Color.white;
            });
        }
    }
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
            animator.SetBool("pass", true);
        }
        else
        {
            animator.SetBool("pass", false);
        }
            
    }
}

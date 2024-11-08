
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEngine;



public class CreateObject : EditorWindow
{

    private LevelData newLevelData = null;
    private LevelData levelData;
    private bool toggleValue;
    private int selectedIndex = 0;
    private int selectedIndexToRemove = 0;
    string path = "Assets/Prefabs/ScriptableObj/";
    bool isCreatingNewLevel;

    /*
     * 
     * 
     * 
     * WINDOW -> SCRIPTABLE OBJECT CREATOR IS THE TOOL TO CREATE LEVELS   
     * 
     * 
     * 
     */

    [MenuItem("Window/ScriptableObjectCreator")]
    public static void ShowWindow()
    {
        CreateObject obj = (CreateObject)GetWindow(typeof(CreateObject));
        obj.minSize = new Vector2(500, 1200);
        obj.maxSize = new Vector2(500, 1200);
        
    }
    void OnGUI()
    {
        List<string> guIds = AssetDatabase.FindAssets("", new[] { path }).ToList(); // to basically gets all guids of files in that path 
        List<string> options1 = guIds.Select(guId => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guId))) // gets the file name without extension of all guids
        .ToList();

        /*
         * THIS CODE CONTAINS THE LOGIC TO CREATE A NEW LEVLES ON EDITOR WINDOW
         * QUESTION, ANIMATION OVERRIDE CONTROLLER , ANSWERS , OPTIONS AND SAVING OF A NEW LEVEL IS DONE HERE
         * ALSO HANDLES THE LEVEL REMOVING PART AS WELL
         */

        GUILayout.Label("Create New Level", EditorStyles.boldLabel);
        if (GUILayout.Button("Create"))
        {
            isCreatingNewLevel = true;

            if (newLevelData == null)
            {
                newLevelData = ScriptableObject.CreateInstance<LevelData>();
            }
        }
        if (isCreatingNewLevel && newLevelData != null)
        {

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Question");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            newLevelData.question = EditorGUILayout.TextField(newLevelData.question);
            EditorGUILayout.EndHorizontal();

            newLevelData.overrideController = (AnimatorOverrideController)EditorGUILayout.ObjectField(
   "Animator Controller", newLevelData.overrideController, typeof(AnimatorOverrideController), false);


            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Options To Choose");
            GUILayout.Space(57);
            GUILayout.Label("Answers");
            EditorGUILayout.EndHorizontal();

    

            EditorGUILayout.Space(20);
            HandleWordEdit(newLevelData);
            EditorGUILayout.Space(20);
            newLevelData.ID = options1.Count + 1;
            EditorGUILayout.BeginHorizontal();
            if (newLevelData.words.Count >= 5)
            {
                if (GUILayout.Button("Save"))
                {

                    string path = $"Assets/Prefabs/ScriptableObj/Level {options1.Count + 1}.asset";
                    AssetDatabase.CreateAsset(newLevelData, path);
                    AssetDatabase.SaveAssets();
                    isCreatingNewLevel = false;
                    newLevelData = null;
                }

            }

            if (GUILayout.Button("Cancel"))
            {
                isCreatingNewLevel = false;
                newLevelData = null;
            }
            EditorGUILayout.EndHorizontal();


        }



        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("EditLevel", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        int newIndex = EditorGUILayout.Popup("Levels", selectedIndex, options1.ToArray());
        EditorGUILayout.EndHorizontal();

        if (newIndex != selectedIndex)
            selectedIndex = newIndex;
        if (selectedIndex >= 0 && selectedIndex < options1.Count)
        {
            ShowCurrentLevelDetails(options1[selectedIndex]);
        }
        else
        {
            GUILayout.Label("No level selected or level does not exist.");
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Select a level to remove");

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (options1.Count > 0)
        {
            int newIndexToRemove = EditorGUILayout.Popup("", selectedIndexToRemove, options1.ToArray());
            if (newIndexToRemove != selectedIndexToRemove)
            {
                selectedIndexToRemove = newIndexToRemove;
                Repaint();
            }
            bool newToggleValue = GUILayout.Toggle(toggleValue, "are you sure?");
            if (newToggleValue != toggleValue)
                toggleValue = newToggleValue;

            if (toggleValue)
            {
                if (selectedIndexToRemove >= 0 && selectedIndexToRemove < options1.Count)
                {
                    CheckForDeletion(options1[selectedIndexToRemove], options1, selectedIndexToRemove);
                }
                else
                {
                    Debug.LogWarning("Selected index for removal is out of bounds.");
                }
            }
        }
        else
        {
            GUILayout.Label("No levels available to remove.");
        }
        EditorGUILayout.EndHorizontal();

    }
    /*
     * THIS CODE IS RESPONSIBLE TO SHOW THE CURRENT LEVEL DETAILS BASED ON THE OPTIONS YOU SELECT FROM DROPDOWN
     */
    void ShowCurrentLevelDetails(string assetName)
    {
        string assetPath = $"Assets/Prefabs/ScriptableObj/{assetName}.asset";
        LevelData level = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Question",EditorStyles.boldLabel);
        level.question = EditorGUILayout.TextField(level.question);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Options to Choose");
        GUILayout.Space(57);
        GUILayout.Label("Answers");
        EditorGUILayout.EndHorizontal();
        if (level.words == null) level.words = new List<string>();
        if (level.answers == null) level.answers = new List<bool>();


        HandleWordEdit(level);
        level.overrideController = (AnimatorOverrideController)EditorGUILayout.ObjectField(
    "Animator Controller", level.overrideController, typeof(AnimatorOverrideController), false);

        if (GUI.changed)
        {
            AssetDatabase.SaveAssets(); 
            Debug.Log("Level data saved.");
        }
    }
    /*
     * HOW THE LEVEL DELETION IS HANDLED IS DONE BY THIS PART 
     */
    void CheckForDeletion(string assetName, List<string> options1 , int selectedIndexToRemove)
    {
        if(toggleValue)
        {
            if(GUILayout.Button("delete")) 
            {
                string assetPath = $"Assets/Prefabs/ScriptableObj/{assetName}.asset";
                LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
                if (AssetDatabase.DeleteAsset(assetPath))
                {
                    options1.Remove(assetName);
                    selectedIndexToRemove = Mathf.Clamp(selectedIndexToRemove, 0, options1.Count - 1);
                    Repaint();
                }
                toggleValue = false;
            }
            
        }
    }
    /*
     * SEPERATED THIS PART OF THE CODE WHERE IT HANDLES THE OPTIONS ADDING , REMOVING ALONG WITH ANSWERS 
     */
    void HandleWordEdit(LevelData level)
    {

        for (int i = 0; i < level.words.Count; i++)
        {

            EditorGUILayout.BeginHorizontal();
            level.words[i] = EditorGUILayout.TextField(level.words[i]);
            GUILayout.Space(75);
            level.answers[i] = EditorGUILayout.Toggle(level.answers[i]);
            EditorGUILayout.EndHorizontal();

        }
      
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            level.words.Add(string.Empty);
            level.answers.Add(false);
        }
        // because lets keep minimum 5 questions right to save a level
        if (level.words.Count > 5)
        {
            if (GUILayout.Button("-"))
            {
                level.words.RemoveAt(level.words.Count - 1);
                level.answers.RemoveAt(level.answers.Count - 1);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

}

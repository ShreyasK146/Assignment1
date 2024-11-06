using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class is responsible for holding the level info which player clicks on so that same level data is loaded in the next scene 
 */

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public LevelData SelectedLevelData { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void SetSelectedLevel(LevelData level)
    {
        SelectedLevelData = level;
    }
}

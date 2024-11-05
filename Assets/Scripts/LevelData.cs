using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "CreateLevel/Level")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int id;
    public int ID { set { id = value; } get { return id; } }


    public string question;
    public List<string> words = new List<string>();
    public List<bool> answers = new List<bool>();

}

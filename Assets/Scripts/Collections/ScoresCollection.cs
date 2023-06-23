using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scores Collection")]
public class ScoresCollection : ScriptableObject, ISerializationCallbackReceiver
{
    //all of this could've been a switch statement
    //but noooo, I HAD to make it "future proof"
    public List<Scores> _scoresList = new List<Scores>();

    [NonSerialized] private Dictionary<ItemType, int> scoresDict = new Dictionary<ItemType, int>();


    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        scoresDict.Clear();
        foreach (var score in _scoresList)
        {
            scoresDict.Add(score.type, score.scoreValue);
        }
    }

    public int GetScoreValue(ItemType type)
    {
        if (scoresDict.TryGetValue(type, out int value))
        {
            return value;
        }
        
        Debug.LogError("Unknown item type.");
        return -1;
    }
}

[Serializable]
public class Scores
{
    public ItemType type;
    public int scoreValue;
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string PERSISTENT_DATA_NAME = "HighScores";
    
    public static DataManager Instance { get; private set; }

    private HighScoreList _highScoreData;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(this);
        } 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        LoadHighScoreData();
        if (_highScoreData == null)
        {
            _highScoreData = new HighScoreList();
            _highScoreData.highestLevel = 1;
            _highScoreData.highScoreDatas = new List<HighScoreData>();
            SaveHighScoreData();
        }
    }

    public int GetHighestLevel()
    {
        return _highScoreData.highestLevel;
    }
    
    public int GetHighestLevelSeen()
    {
        return _highScoreData.highestLevelSeen;
    }
    
    public bool SetHighScore(int level, int value)
    {
        if (_highScoreData.scoreDict.TryGetValue(level, out int oldScore))
        {
            if (value <= oldScore) return false;
            _highScoreData.scoreDict[level] = value;
        }
        else
        {
            _highScoreData.scoreDict.Add(level, value);
        }
        SaveHighScoreData();
        return true;
    }
    

    public bool SetHighestLevel(int level)
    {
        if (level < _highScoreData.highestLevel) return false;
        _highScoreData.highestLevel = level;
        SaveHighScoreData();

        return true;
    }
    
    public bool SetHighestLevelSeen(int level)
    {
        if (level < _highScoreData.highestLevelSeen) return false;
        _highScoreData.highestLevelSeen = level;
        SaveHighScoreData();

        return true;
    }
    
    public int GetHighScoreForLevel(int level_number)
    {
        if (_highScoreData.scoreDict.TryGetValue(level_number, out int score))
        {
            return score;
        }

        return 0;
    }

    void SaveHighScoreData()
    {
        _highScoreData.DictToList();
        
        var filePath = Path.Combine(Application.persistentDataPath, ("data/" + PERSISTENT_DATA_NAME));
        
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        string jsonData = JsonUtility.ToJson(_highScoreData, true);
        byte[] byteData;
        
        byteData = Encoding.ASCII.GetBytes(jsonData);
        
        try
        {
            // save data here
            File.WriteAllBytes(filePath, byteData);
            Debug.Log("Save data to: " + filePath);
        }
        catch (Exception e)
        {
            // write out error here
            Debug.LogError("Failed to save data to: " + filePath);
            Debug.LogError("Error " + e.Message);
        }
    }

    // Update is called once per frame
    void LoadHighScoreData()
    {
        var filePath = Path.Combine(Application.persistentDataPath, ("data/" + PERSISTENT_DATA_NAME));
        
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Debug.LogWarning("No save data found.");
            return;
        }

        // load in the save data as byte array
        byte[] jsonDataAsBytes = null;

        try
        {
            jsonDataAsBytes = File.ReadAllBytes(filePath);
            Debug.Log("<color=green>Loaded all data from: </color>" + filePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to load data from: " + filePath);
            Debug.LogWarning("Error: " + e.Message);
            return;
        }

        // convert the byte array to json
        string jsonData = Encoding.ASCII.GetString(jsonDataAsBytes);

        // convert to the specified object type
        HighScoreList returnedData = JsonUtility.FromJson<HighScoreList>(jsonData);
        
        _highScoreData = returnedData;
        _highScoreData.ListToDict();
    }
}

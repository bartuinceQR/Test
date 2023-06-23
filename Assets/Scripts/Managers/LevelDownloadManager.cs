using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.Networking;

public class LevelDownloadManager : MonoBehaviour
{

    private const string URL_BASE_STRING = "https://row-match.s3.amazonaws.com/levels/";
    private const string LEVEL_PREFIX = "RM_";
    private const string PERSISTENT_DATA_NAME = "DownloadedLevels";

    public List<LevelData> downloadedLevels = new List<LevelData>();

    public static LevelDownloadManager Instance { get; private set; }

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

    private IEnumerator Start()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, ("data/" + PERSISTENT_DATA_NAME))))
        {
            StartCoroutine(WaitAndDownload());
        }
        else
        {
            LoadDownloadedLevels();
            yield return new WaitUntil(() => LevelManager.Instance.IsReady);
            LevelManager.Instance.AddDownloadedLevels();
        }
    }

    private IEnumerator WaitAndDownload()
    {
        bool isConnected = false;

        while (!isConnected)
        {
            UnityWebRequest request = new UnityWebRequest("http://google.com");
            yield return request.SendWebRequest();
            if (request.error != null)
            {
                Debug.Log("Error");
                yield return new WaitForSeconds(5);
            }
            else
            {
                Debug.Log("Success");
                isConnected = true;
            }
        }

        DownloadMissingLevels();
        
        yield return new WaitUntil(() => LevelManager.Instance.IsReady);
        LevelManager.Instance.AddDownloadedLevels();
    }
    

    public void DownloadMissingLevels()
    {
        List<LevelData> missingLevels = new List<LevelData>();

        for (int i = 11; i <= 15; i++)
        {
            var downloadURL = URL_BASE_STRING + LEVEL_PREFIX + "A" + i;

            using (UnityWebRequest request = UnityWebRequest.Get(downloadURL))
            {
                var op = request.SendWebRequest();

                while (!op.isDone)
                {
                    Task.Yield();
                }

                LevelData levelData = LevelJSONConverter.ConvertToLevelData(request.downloadHandler.text);
                missingLevels.Add(levelData);
            }
        }
        
        for (int i = 1; i <= 10; i++)
        {
            var downloadURL = URL_BASE_STRING + LEVEL_PREFIX + "B" + i;

            using (UnityWebRequest request = UnityWebRequest.Get(downloadURL))
            {
                var op = request.SendWebRequest();

                while (!op.isDone) { Task.Yield(); }

                LevelData levelData = LevelJSONConverter.ConvertToLevelData(request.downloadHandler.text);
                missingLevels.Add(levelData);
            }
        }
        
        var filePath = Path.Combine(Application.persistentDataPath, ("data/" + PERSISTENT_DATA_NAME));
        
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        ListOfLevelData list = new ListOfLevelData(){levels = missingLevels};

        downloadedLevels = missingLevels;
        
        string jsonData = JsonUtility.ToJson(list, true);
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


    public void LoadDownloadedLevels()
    {
        var filePath = Path.Combine(Application.persistentDataPath, ("data/" + PERSISTENT_DATA_NAME));
        
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Debug.LogWarning("Path does not exist! " + filePath);
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
        ListOfLevelData returnedData = JsonUtility.FromJson<ListOfLevelData>(jsonData);
        
        //DO NOT do this it's VERY persistent
        //_levelDataCollection.LevelDatas.AddRange(returnedData.levels);

        downloadedLevels = returnedData.levels;
    }

    //lol JSON limitations
    [Serializable]
    public class ListOfLevelData
    {
        public List<LevelData> levels;
    }
}

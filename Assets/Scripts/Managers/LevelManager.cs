using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDataCollection levelDataCollection;
        private List<LevelData> levels = new List<LevelData>();

        private Dictionary<int, LevelData> levelDict = new Dictionary<int, LevelData>();
        private int _levelToLoad;

        public bool IsReady;

        public bool GotHighScore { get; set; }

        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
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
            levels.AddRange(levelDataCollection.LevelDatas);
            ConvertToDict();
            
            SceneChangeHandler.Instance.SceneChangedEvent += OnSceneLoaded;

            IsReady = true;
        }

        public void AddDownloadedLevels()
        {
            levels.AddRange(LevelDownloadManager.Instance.downloadedLevels);
            ConvertToDict();
        }

        void ConvertToDict()
        {
            levelDict.Clear();
            foreach (var level in levels)
            {
                levelDict.Add(level.level_number, level);
            }
        }

        public List<LevelData> GetLevels()
        {
            return levels;
        }

        public LevelData GetLevelByNumber(int number)
        {
            return levelDict[number];
        }

        public int GetLevelToLoad()
        {
            return _levelToLoad;
        }

        public void Load(int level)
        {
            _levelToLoad = level;
            SceneManager.LoadScene("Scenes/MatchRowScene");
        }

        void OnSceneLoaded(string newName, string oldName)
        {
            if (newName == Constants.MAIN_SCENE_NAME && oldName == Constants.MATCH_SCENE_NAME)
            {
                if (GotHighScore)
                {
                    Debug.Log("Yahoo");
                    GotHighScore = false;
                }
                else
                {
                    
                }
            }
        }

    }
}

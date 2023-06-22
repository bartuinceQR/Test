using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDataCollection levelDataCollection;
        private List<LevelData> levels = new List<LevelData>();

        private Dictionary<int, LevelData> levelDict = new Dictionary<int, LevelData>();

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
        
        
    }
}

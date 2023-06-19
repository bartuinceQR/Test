using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDataCollection levelDataCollection;
        private List<LevelData> levels = new List<LevelData>();

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
        }

        public void AddDownloadedLevels()
        {
            levels.AddRange(LevelDownloadManager.Instance.downloadedLevels);
        }

        public List<LevelData> GetLevels()
        {
            return levels;
        }
    }
}

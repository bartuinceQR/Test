using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDataCollection levelDataCollection;

        [SerializeField] private GameObject _levelsPopupPrefab;
        [SerializeField] private GameObject _highScoreGuyPrefab;
        
        [SerializeField] private Fade fade;
        [SerializeField] private AudioClip menuMusic;

        private List<LevelData> levels = new List<LevelData>();

        private Dictionary<int, LevelData> levelDict = new Dictionary<int, LevelData>();
        private int _levelToLoad;
        
        
        public bool IsReady;
        public bool GotHighScore { get; set; }
        public bool IsLocked { get; set; }
        public bool IsLoading { get; private set; }
        
        public Action LevelsPopupOpened;

        public static LevelManager Instance { get; private set; }

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
            levels.AddRange(levelDataCollection.LevelDatas);
            ConvertToDict();

            SceneChangeHandler.Instance.SceneChangedEvent += OnSceneLoaded;

            fade.FadeOut(1f).OnComplete(() =>
            {
                AudioManager.Instance.PlayClip(menuMusic);
            });
            

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
            IsLoading = true;
            
            FadeIn(() =>
            {
                IsLoading = false;
                SceneManager.LoadScene("Scenes/MatchRowScene");
                fade.FadeOut(1.5f);
            }, 1.5f);
        }

        public void FadeOut(Action callback, float duration = 1f)
        {
            fade.FadeOut(duration).OnComplete(() =>
            {
                callback();
            });
        }
        
        public void FadeIn(Action callback, float duration = 1f)
        {
            fade.FadeIn(duration).OnComplete(() =>
            {
                callback();
            });
        }

        void OnSceneLoaded(string newName, string oldName)
        {
            if (newName == Constants.MAIN_SCENE_NAME && oldName == Constants.MATCH_SCENE_NAME)
            {
                FadeOut( () =>
                {
                    StartCoroutine(HighScoreAnims());
                }, 1f);
            }
        }

        IEnumerator HighScoreAnims()
        {
            IsLocked = true;

            if (GotHighScore)
            {
                var hs = Instantiate(_highScoreGuyPrefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(4.5f);
                Destroy(hs);
            }
            
            AudioManager.Instance.PlayClip(menuMusic);

            yield return new WaitForSeconds(1f);

            ShowLevelsPopup();
        }

        //this script is starting to become a swiss army knife, oh well
        public void ShowLevelsPopup()
        {
            Instantiate(_levelsPopupPrefab, transform.position, Quaternion.identity);
            LevelsPopupOpened?.Invoke();

            IsLocked = false;
        }

    }
}

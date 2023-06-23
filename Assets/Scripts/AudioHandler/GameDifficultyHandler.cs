using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class GameDifficultyHandler : MonoBehaviour
{
    
    [SerializeField] private AudioClip easyModeMusic;
    [SerializeField] private AudioClip hardModeMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        bool isHard = LevelManager.Instance.GetLevelToLoad() > LevelManager.Instance.GetLevels().Count / 2;
        
        if (isHard)
            AudioManager.Instance.PlayClip(hardModeMusic);
        else
            AudioManager.Instance.PlayClip(easyModeMusic);
        
        HUDManager.Instance.SetBGSprite(isHard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

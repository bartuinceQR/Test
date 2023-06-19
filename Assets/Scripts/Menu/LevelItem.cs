using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelItem : MonoBehaviour
{
    private int level_number = -1;
    
    
    [SerializeField] private TextMeshPro LevelText;
    [SerializeField] private TextMeshPro MoveCountText;
    [SerializeField] private TextMeshPro HighScoreText;

    public void Init(LevelData levelData)
    {
        level_number = levelData.level_number;
        LevelText.SetText("Level " + levelData.level_number);
        MoveCountText.SetText(levelData.move_count + " Moves");
        HighScoreText.SetText("Highscore : 0");
    }
}

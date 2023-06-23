using TMPro;
using UnityEngine;

public class LevelItem : MonoBehaviour
{
    [SerializeField] private LevelLoadButton button;
    
    [SerializeField] private TextMeshPro LevelText;
    [SerializeField] private TextMeshPro MoveCountText;
    [SerializeField] private TextMeshPro HighScoreText;

    public void Init(LevelData levelData)
    {
        button.Init(levelData.level_number);
        LevelText.SetText("Level " + levelData.level_number);
        MoveCountText.SetText(levelData.move_count + " Moves");

        HighScoreText.SetText("Highscore : " + DataManager.Instance.GetHighScoreForLevel(levelData.level_number));
    }
}

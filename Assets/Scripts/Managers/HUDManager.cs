using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro movesLeftText;
    [SerializeField] private List<Sprite> backgroundSprites;

    [SerializeField] private SpriteRenderer levelBGSprite;
    
    public static HUDManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this;
        } 
    }

    public void SetBGSprite(bool isHard)
    {
        if (isHard)
        {
            levelBGSprite.sprite = backgroundSprites[1];
        }
        else
        {
            levelBGSprite.sprite = backgroundSprites[0];
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.ScoreChangedEvent += UpdateScoreText;
        GameplayManager.MoveSpentEvent += UpdateMovesText;
    }

    // Update is called once per frame
    void UpdateScoreText(int score)
    {
        scoreText.SetText(score.ToString());
    }

    void UpdateMovesText(int moves)
    {
        movesLeftText.SetText(moves.ToString());
    }

    private void OnDestroy()
    {
        GameplayManager.ScoreChangedEvent -= UpdateScoreText;
        GameplayManager.MoveSpentEvent -= UpdateMovesText;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private Transform boardCenter;
    [SerializeField] private Transform bottomLeft;
    
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private ScoresCollection scoresCollection;


    private Square[][] _squares;

    private LevelData _currentLevelData;

    private int _score;
    private int _movesLeft;

    private List<int> _clearedRows;

    public static event Action<int> ScoreChangedEvent;
    public static event Action<int> MoveSpentEvent;

    public static GameplayManager Instance { get; private set; }

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

    private void Start()
    {
        int levelToLoad = LevelManager.Instance.GetLevelToLoad();
        LoadLevel(LevelManager.Instance.GetLevelByNumber(levelToLoad));
    }

    public void LoadLevel(LevelData levelData)
    {
        int width = levelData.grid_width;
        int height = levelData.grid_height;

        float squareWidth = squarePrefab.GetComponent<Square>().GetSquareSize().x;
        float squareHeight = squarePrefab.GetComponent<Square>().GetSquareSize().y;

        Vector3 offset = new Vector3(width / 2f * squareWidth, height / 2f * squareHeight, 0f);
        bottomLeft.position = boardCenter.position - offset;
        Vector3 startPos = bottomLeft.position + new Vector3(squareWidth / 2, squareHeight / 2, 0);


        _squares = new Square[height][];

        for (int row = 0; row < height; row++)
        {
            _squares[row] = new Square[width];
            
            for (int col = 0; col < width; col++)
            {
                Vector3 squareOffset = new Vector3(col * squareWidth, row * squareHeight, 0);

                var square = Instantiate(squarePrefab, startPos + squareOffset, Quaternion.identity, bottomLeft).GetComponent<Square>();
                square.Init(col, row, levelData.grid[row * width + col]);
                _squares[row][col] = square;
            }
        }

        _currentLevelData = levelData;

        _score = 0;
        _movesLeft = _currentLevelData.move_count;
        
        ScoreChangedEvent?.Invoke(_score);
        MoveSpentEvent?.Invoke(_movesLeft);
    }
    
    void AddScore(int score)
    {
        _score += score;
        ScoreChangedEvent?.Invoke(_score);
    }

    public void SpendMove()
    {
        _movesLeft--;
        MoveSpentEvent?.Invoke(_movesLeft);
    }

    public void CheckStatus()
    {
        if (!CheckResumable())
        {
            MatchManager.Instance.boardState = BoardState.Completed;
            StartCoroutine(WaitAndFinishLevel());
        }
    }
    
    IEnumerator WaitAndFinishLevel()
    {
        yield return new WaitForSeconds(1f);

        bool newHighScore = DataManager.Instance.SetHighScore(_currentLevelData.level_number, _score);
        DataManager.Instance.SetHighestLevel(_currentLevelData.level_number);
        LevelManager.Instance.GotHighScore = true;
    }

    
    public void CheckRows(Square startSquare, Square endSquare)
    {
        int row1 = startSquare.GetCoordinates().row;
        int row2 = endSquare.GetCoordinates().row;

        int width = _currentLevelData.grid_width;

        List<int> rows = new List<int>() { row1, row2 };

        foreach (var row in rows)
        {
            ItemType rowItemType = ItemType.None;
            bool allSame = true;
        
            for (int i = 0; i < width; i++)
            {
                Square square = _squares[row][i];

                if (rowItemType == ItemType.None)
                {
                    rowItemType = square.GetItemType();
                }
                else if (square.GetItemType() != rowItemType)
                {
                    allSame = false;
                    break;
                }
            }

            if (allSame)
            {
                ClearRow(row, rowItemType);
            }
        }
    }

    void ClearRow(int row, ItemType rowItemType)
    {
        int width = _currentLevelData.grid_width;

        for (int i = 0; i < width; i++)
        {
            Square square = _squares[row][i];

            square.SetItemType(ItemType.Completed);
            
            AddScore(scoresCollection.GetScoreValue(rowItemType));
        }
        
        _clearedRows.Add(row);
    }

    bool CheckResumable()
    {
        if (_clearedRows.Count == 0) return true;

        Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();

        for (int row = 0; row < _currentLevelData.grid_height; row++)
        {
            for (int col = 0; col < _currentLevelData.grid_width; col++)
            {

                Square sq = _squares[row][col];
                ItemType sqItem = sq.GetItemType();

                if (sqItem == ItemType.Completed)
                {
                    row++;

                    if (CheckItemCounts(itemCounts)) return true;
                    itemCounts.Clear();
                    
                    break;
                }
                else
                {
                    if (!itemCounts.TryAdd(sqItem, 1))
                    {
                        itemCounts[sqItem]++;
                    }
                }
            }
        }
        return CheckItemCounts(itemCounts);
    }


    private bool CheckItemCounts(Dictionary<ItemType, int> items)
    {
        foreach (var item in items.Keys)
        {
            if (items[item] > _currentLevelData.grid_width) return true;
        }

        return false;
    }
    
}


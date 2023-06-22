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


    private Square[][] _squares;

    private LevelData currentLevelData;
    
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
        LoadLevel(LevelManager.Instance.GetLevelByNumber(1));
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

        currentLevelData = levelData;
    }
    
    public void CheckRows(Square startSquare, Square endSquare)
    {
        int row1 = startSquare.GetCoordinates().row;
        int row2 = endSquare.GetCoordinates().row;

        int width = currentLevelData.grid_width;

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
                ClearRow(row);
            }
        }
    }


    void ClearRow(int row)
    {
        int width = currentLevelData.grid_width;

        for (int i = 0; i < width; i++)
        {
            Square square = _squares[row][i];

            square.SetItemType(ItemType.Completed);
        }
    }
    
}

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


    private Dictionary<SquarePosition, Square> _squares = new Dictionary<SquarePosition, Square>();


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

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Vector3 squareOffset = new Vector3(col * squareWidth, row * squareHeight, 0);

                var square = Instantiate(squarePrefab, startPos + squareOffset, Quaternion.identity, bottomLeft).GetComponent<Square>();
                square.Init(col, row, levelData.grid[row * width + col]);
                _squares.Add(square.GetCoordinates(), square);
            }
        }


    }
}

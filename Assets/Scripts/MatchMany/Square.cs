using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Square : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler,IPointerUpHandler
{
    [SerializeField] private Transform itemCenter;
    [SerializeField] private Transform itemObject;
    [SerializeField] private SpriteRenderer itemSprite;

    private SquarePosition coordinates;
    private ItemType m_ItemType;

    [HideInInspector]
    public bool IsCompleted;

    [Header("Item Type Sprites")]
    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite colorlessSprite;
    [SerializeField] private Sprite completedSprite;


    public void Init(int col, int row, ItemType itemType)
    {
        coordinates = new SquarePosition(col, row);
        SetItemType(itemType);
    }
    
    public Transform GetItemObject()
    {
        return itemObject;
    }

    public void SetItemObject(Transform itemObj, ItemType itemType)
    {
        itemObject = itemObj;
        itemObject.SetParent(itemCenter);
        itemSprite = itemObject.GetComponentInChildren<SpriteRenderer>();
        m_ItemType = itemType;
    }

    public SquarePosition GetCoordinates()
    {
        return coordinates;
    }

    public ItemType GetItemType()
    {
        return m_ItemType;
    }

    public void SetItemType(ItemType type)
    {
        m_ItemType = type;
        itemSprite.sprite = GetTypeSprite(m_ItemType);
    }

    private Sprite GetTypeSprite(ItemType type)
    {
        if (IsCompleted)
            return completedSprite;
        
        switch (type)
        {
            case ItemType.Red:
                return redSprite;
            case ItemType.Blue:
                return blueSprite;
            case ItemType.Green:
                return greenSprite;
            case ItemType.Colorless:
                return colorlessSprite;
            case ItemType.Completed:
                return completedSprite;
            default:
                Debug.LogError("How'd you make a TYPELESS square?!");
                return completedSprite;
        }
    }

    public Vector2 GetSizeInPixels()
    {
        Camera cam = Camera.main;
        
        Vector3 min = GetComponent<SpriteRenderer>().bounds.min;
        Vector3 max = GetComponent<SpriteRenderer>().bounds.max;

        Vector3 minScreen = cam.WorldToScreenPoint(min);
        Vector3 maxScreen = cam.WorldToScreenPoint(max);

        return maxScreen - minScreen;
    }

    public Vector2 GetSquareSize()
    {
        return GetComponent<SpriteRenderer>().bounds.size;
    }

    public Tweener SwapTo(Square to, float duration = 1f)
    {
        itemSprite.GetComponent<Animator>().Play("ItemSwap");
        return itemObject.DOMove(to.itemCenter.position, duration).SetEase(Ease.InCubic);
    }

    public void PlayClearAnimation()
    {
        itemSprite.GetComponent<Animator>().Play("RowMatch");
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (MatchManager.Instance.boardState != BoardState.Unlocked) return;

        MatchManager.Instance.holdingSwap = true;
        MatchManager.Instance.currentlySelectedSquare = this;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (MatchManager.Instance.holdingSwap && MatchManager.Instance.currentlySelectedSquare != this)
        {
            MatchManager.Instance.holdingSwap = false;
            if (!MatchManager.Instance.TrySwap(this))
            {
                MatchManager.Instance.holdingSwap = false;
                MatchManager.Instance.currentlySelectedSquare = null;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MatchManager.Instance.holdingSwap = false;
        MatchManager.Instance.currentlySelectedSquare = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       Debug.Log(coordinates.ToString());
    }
}


public class SquarePosition
{
    public int col;
    public int row;

    public SquarePosition(int col, int row)
    {
        this.col = col;
        this.row = row;
    }

    //definitely me when I forget how to override Equals and GetHashCode
    public string CoordsInString()
    {
        return col + "," + row;
    }
}

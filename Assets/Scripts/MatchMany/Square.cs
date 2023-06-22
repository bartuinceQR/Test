using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private Vector2 itemCenter;
    [SerializeField] private Transform itemObject;
    [SerializeField] private SpriteRenderer itemSprite;

    public SquarePosition coordinates;
    public ItemType m_ItemType;

    public bool IsCompleted;

    [Header("Item Type Sprites")]
    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite colorlessSprite;
    [SerializeField] private Sprite completedSprite;
    


    public void SetItemType(ItemType type)
    {
        m_ItemType = type;
    }


    // Start is called before the first frame update
    void Start()
    {
        SetItemSprite();
    }

    public void SetItemSprite()
    {
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
            default:
                Debug.LogError("How'd you make a TYPELESS square?!");
                return completedSprite;
        }
    }
}


public class SquarePosition
{
    public int col;
    public int row;
}

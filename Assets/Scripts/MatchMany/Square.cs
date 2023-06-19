using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private Vector2 itemCenter;
    [SerializeField] private Transform itemObject;

    public SquarePosition coordinates;
    
    public ItemType m_ItemType;


    public void SetItemType(ItemType type)
    {
        m_ItemType = type;
    }


    public void SwapTo(Vector2 targetPos)
    {
        
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class SquarePosition
{
    public int col;
    public int row;
}

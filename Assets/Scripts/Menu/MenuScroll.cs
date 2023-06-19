using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScroll : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField] private Transform moveRoot;
    
    
    private bool isClicked = false;

    private Vector2 startPos;
    private float moveLimit = 1000f;

    private float moveVelocity = 0f;
    
    private List<SpriteRenderer> levelObjects;

    private void Start()
    {
        levelObjects = new List<SpriteRenderer>();
        for (int i = 0; i < moveRoot.childCount; i++)
        {
            levelObjects.Add(moveRoot.GetChild(i).GetComponent<SpriteRenderer>());
        }
        
        startPos = moveRoot.localPosition;
        float itemsY = 0;

        foreach (var renderer in levelObjects)
        {
            itemsY += renderer.bounds.size.y;
        }
        
        moveLimit = itemsY - GetComponent<SpriteMask>().bounds.size.y;
    }

    private void Update()
    {
        if (Mathf.Abs(moveVelocity) < Single.Epsilon) return;
        Vector2 pos = moveRoot.position;
        pos += new Vector2(0,moveVelocity * Time.deltaTime * 0.25f);

        if (pos.y < startPos.y)
        {
            pos.y = startPos.y;
            moveVelocity = 0f;
        }
        
        if (pos.y - startPos.y > moveLimit)
        {
            pos.y = startPos.y + moveLimit;
            moveVelocity = 0f;
        }

        moveVelocity *= 0.95f;

        moveRoot.position = pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        Debug.Log("oi");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        Debug.Log("io");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!isClicked) return;
        moveVelocity = eventData.delta.y;
    }
}

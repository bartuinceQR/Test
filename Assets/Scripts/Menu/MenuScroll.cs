using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScroll : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField] private Transform moveRoot;
    
    
    private bool _isClicked = false;

    private Vector2 _startPos;
    private float _moveLimit = 1000f;

    private float _moveVelocity = 0f;
    
    private List<SpriteRenderer> _levelObjects;

    private void Start()
    {
        _levelObjects = new List<SpriteRenderer>();
        for (int i = 0; i < moveRoot.childCount; i++)
        {
            _levelObjects.Add(moveRoot.GetChild(i).GetComponent<SpriteRenderer>());
        }
        
        _startPos = moveRoot.position;
        float itemsY = 0;

        foreach (var renderer in _levelObjects)
        {
            itemsY += renderer.bounds.size.y;
        }
        
        _moveLimit = itemsY - GetComponent<SpriteMask>().bounds.size.y;
    }

    private void Update()
    {
        if (Mathf.Abs(_moveVelocity) < Single.Epsilon) return;
        Vector2 pos = moveRoot.position;
        pos += new Vector2(0,_moveVelocity * Time.deltaTime * 0.25f);

        if (pos.y < _startPos.y)
        {
            pos.y = _startPos.y;
            _moveVelocity = 0f;
        }
        
        if (pos.y - _startPos.y > _moveLimit)
        {
            pos.y = _startPos.y + _moveLimit;
            _moveVelocity = 0f;
        }

        _moveVelocity *= 0.95f;

        moveRoot.position = pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isClicked = true;
        Debug.Log("oi");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isClicked = false;
        Debug.Log("io");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!_isClicked) return;
        _moveVelocity = eventData.delta.y;
    }
}

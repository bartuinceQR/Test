using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScroll : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField] private Transform moveRoot;

    private bool _isReady = false;
    private bool _isClicked = false;

    private Vector2 _startPos;
    private float _moveLimit = 1000f;

    private float _moveVelocity = 0f;
    
    private List<SpriteRenderer> _levelObjects;

    public void Init()
    {
        _levelObjects = new List<SpriteRenderer>();
        for (int i = 0; i < moveRoot.childCount; i++)
        {
            _levelObjects.Add(moveRoot.GetChild(i).GetComponent<SpriteRenderer>());
        }
        
        _startPos = moveRoot.position;
        float itemsY = 0;

        float minY = (float)Double.MaxValue;
        float maxY = (float)Double.MinValue;

        foreach (var renderer in _levelObjects)
        {
            var itemMaxY = renderer.transform.position.y + renderer.bounds.size.y;
            var itemMinY = renderer.transform.position.y - renderer.bounds.size.y;

            minY = Mathf.Min(itemMinY, minY);
            maxY = Mathf.Max(itemMaxY, maxY);
            
            itemsY += renderer.bounds.size.y;
        }

        _moveLimit = maxY - minY;

        _isReady = true;

        //_moveLimit = itemsY - GetComponent<SpriteMask>().bounds.size.y;
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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isClicked = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!_isClicked) return;
        if (!_isReady) return;
        _moveVelocity = eventData.delta.y;
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    [SerializeField] private AudioClip matchSound;
    
    public static MatchManager Instance { get; private set; }

    [HideInInspector]
    public bool holdingSwap = false;
    [HideInInspector]
    public Square currentlySelectedSquare;
    [HideInInspector]
    public BoardState boardState;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
        } 
    }

    public bool TrySwap(Square other)
    {
        //not even holding anything
        if (currentlySelectedSquare == null) return false;

        //not adjacent, you really think you're being sneaky?
        if (Mathf.Abs(currentlySelectedSquare.GetCoordinates().col - other.GetCoordinates().col)
            + Mathf.Abs(currentlySelectedSquare.GetCoordinates().row - other.GetCoordinates().row) != 1) return false;

        //it's joever
        if (currentlySelectedSquare.GetItemType() == ItemType.Completed ||
            other.GetItemType() == ItemType.Completed) return false;
        
        DoSwap(currentlySelectedSquare, other);
        
        return true;
    }

    public void DoSwap(Square from, Square to)
    {
        boardState = BoardState.Locked;
        
        GameplayManager.Instance.SpendMove();
        
        float duration = 0.33f;

        Sequence tweenSeq = DOTween.Sequence();
        
        tweenSeq.Insert(0,from.SwapTo(to, duration));
        tweenSeq.Insert(0,to.SwapTo(from, duration));
        
        AudioManager.Instance.PlaySFX(matchSound);

        //should I let both squares handle it on their own?.....probably not
        tweenSeq.OnComplete(() =>
        {
            Transform fromItemObj = from.GetItemObject();
            ItemType fromItemType = from.GetItemType();
            from.SetItemObject(to.GetItemObject(), to.GetItemType());
            to.SetItemObject(fromItemObj, fromItemType);
            
            GameplayManager.Instance.CheckRows(from, to);
            GameplayManager.Instance.CheckStatus();
            
            holdingSwap = false;
            currentlySelectedSquare = null;
        });

    }
}
public enum BoardState
{
    Unlocked,
    Locked,
    Completed
}
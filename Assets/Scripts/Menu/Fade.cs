using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private SpriteRenderer _fadeSprite;
    
    private static Fade Instance { get;  set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(this);
        } 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _fadeSprite = GetComponent<SpriteRenderer>();
    }
    
    public Tweener FadeOut(float duration = 1f)
    {
        return _fadeSprite.DOFade(0, duration);
    }
    
    public Tweener FadeIn(float duration = 1f)
    {
        return _fadeSprite.DOFade(1, duration);
    }
}

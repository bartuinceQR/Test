using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class LevelsPopup : MonoBehaviour
{
    [SerializeField] private Transform LevelsArea;
    [SerializeField] private GameObject LevelItemPrefab;
    [SerializeField] private MenuScroll MenuScroll;

    [SerializeField] private AudioClip openSound;
    
    public float spacingMultiplier = 1.1f;
    
    // Start is called before the first frame update
    void OnAnimationComplete()
    {
        AudioManager.Instance.PlaySFX(openSound);
        
        var distance = LevelItemPrefab.GetComponent<SpriteRenderer>().bounds.size.y 
                       / LevelItemPrefab.GetComponent<SpriteRenderer>().transform.localScale.y;
        
        Vector2 positionOffset = Vector2.zero;
        
        var levels = LevelManager.Instance.GetLevels();
        foreach (var leveldata in levels)
        {
            var levelItem = Instantiate(LevelItemPrefab, (Vector2)LevelsArea.position + positionOffset,
                Quaternion.identity, LevelsArea);
            levelItem.GetComponent<LevelItem>().Init(leveldata);
            positionOffset -= new Vector2(0, distance * spacingMultiplier);
        }

        MenuScroll.Init();
    }
}

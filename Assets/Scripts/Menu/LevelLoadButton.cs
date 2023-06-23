using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoadButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer buttonSprite;
    [SerializeField] private TextMeshPro playText;
    
    private int _level_number;
    private bool disabled = false;

    public void Init(int level_number)
    {
        _level_number = level_number;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        int highestLevelSeen = DataManager.Instance.GetHighestLevelSeen();
        int highestLevel = DataManager.Instance.GetHighestLevel();
        if (highestLevelSeen < _level_number)
        {
            Deactivate();
            disabled = true;
        }

        if (highestLevel >= _level_number)
        {
            Activate();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (disabled) return;

        LevelManager.Instance.Load(_level_number);
    }
    
    void Activate()
    {
        GetComponent<Animator>().Play("Activate");
        playText.text = "Play";
    }

    void OnAnimationComplete()
    {
        DataManager.Instance.SetHighestLevelSeen(_level_number);
        disabled = false;
    }

    void Deactivate()
    {
        buttonSprite.color = Color.black;
        playText.text = "";
    }
}

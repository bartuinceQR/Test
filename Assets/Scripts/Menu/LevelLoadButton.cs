using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelLoadButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer buttonSprite;
    [SerializeField] private TextMeshPro playText;

    [SerializeField] private AudioClip clickSFX;
    
    private int _level_number;
    private bool disabled = false;

    private Animator _animator;

    public void Init(int level_number)
    {
        _level_number = level_number;
    }
    
    // Start is called before the first frame update
    void Start()
    {

        _animator = GetComponent<Animator>();
        
        
        int highestLevelSeen = DataManager.Instance.GetHighestLevelSeen();
        int highestLevel = DataManager.Instance.GetHighestLevel();

        if (highestLevel >= _level_number)
        {
            if (highestLevelSeen < _level_number)
            {
                disabled = true;
                Activate();
            }
        }
        else
        {
            Deactivate();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (disabled) return;
        if (LevelManager.Instance.IsLoading) return;
        
        AudioManager.Instance.PlaySFX(clickSFX);

        LevelManager.Instance.Load(_level_number);
    }
    
    void Activate()
    {
        _animator.enabled = true;
        _animator.Play("Activate");
        playText.text = "Play";
    }

    void OnAnimationComplete()
    {
        DataManager.Instance.SetHighestLevelSeen(_level_number);
        disabled = false;
    }

    void Deactivate()
    {
        _animator.enabled = false;
        buttonSprite.color = Color.black;
        playText.text = "";
        disabled = true;
    }
}

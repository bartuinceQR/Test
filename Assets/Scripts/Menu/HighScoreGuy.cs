using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreGuy : MonoBehaviour
{
    [SerializeField] private AudioClip highScoreSFX;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySFX(highScoreSFX);       
    }
}

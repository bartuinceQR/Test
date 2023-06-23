using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHandler : MonoBehaviour
{

    private Scene _lastLoadedScene;
    public Action<string, string> SceneChangedEvent;
    
    public static SceneChangeHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(this);
        } 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneChangedEvent?.Invoke(scene.name, _lastLoadedScene.name);
        _lastLoadedScene = scene;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data Collection")]
public class LevelDataCollection : ScriptableObject, ISerializationCallbackReceiver
{

    public List<LevelData> LevelDatas;

    public void OnBeforeSerialize()
    {
       
    }

    public void OnAfterDeserialize()
    {
        
    }
}

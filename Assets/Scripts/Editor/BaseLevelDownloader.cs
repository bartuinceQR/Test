using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Editor
{
    public class BaseLevelDownloader : MonoBehaviour
    {
        private const string URL_BASE_STRING = "https://row-match.s3.amazonaws.com/levels/";
        private const string LEVEL_PREFIX = "RM_";

        [MenuItem("Levels/Get Starter Levels")]
        public static void DownloadStarterLevels()
        {
            string[] result = AssetDatabase.FindAssets("LevelDataCollection", new[]{@"Assets\Collections"});
            LevelDataCollection collection = null;
    
            if (result.Length > 1)
            {
                Debug.LogError("More than 1 asset found.");
                return;
            }
    
            if(result.Length == 0)
            {
                Debug.Log("Creating new asset.");
                collection = ScriptableObject.CreateInstance<LevelDataCollection>();
                AssetDatabase.CreateAsset(collection, @"Assets\LevelDataCollection.asset");
            }
            else
            {
                string path = AssetDatabase.GUIDToAssetPath(result[0]);
                collection= (LevelDataCollection)AssetDatabase.LoadAssetAtPath(path, typeof(LevelDataCollection));
                Debug.Log("Found asset.");
            }

            if (collection == null) return;
            
            collection.LevelDatas.Clear();


            for (int i = 1; i <= 10; i++)
            {
                var downloadURL = URL_BASE_STRING + LEVEL_PREFIX + "A" + i;

                using (UnityWebRequest request = UnityWebRequest.Get(downloadURL))
                {
                    var op = request.SendWebRequest();

                    while (!op.isDone)
                    {
                        Task.Yield();
                    }

                    LevelData levelData = LevelJSONConverter.ConvertToLevelData(request.downloadHandler.text);
                
                    collection.LevelDatas.Add(levelData);
                }
            }
        
            collection.LevelDatas.Sort();
        
            
            EditorUtility.SetDirty(collection);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

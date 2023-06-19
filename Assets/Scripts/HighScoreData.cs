using System;
using System.Collections.Generic;

[Serializable]
public class HighScoreData
{
    public int level_number;
    public int score;
}

[Serializable]
public class HighScoreList
{
    public List<HighScoreData> highScoreDatas;

    [NonSerialized] public Dictionary<int, int> scoreDict;

    public void ListToDict()
    {  
        scoreDict.Clear();
        foreach (var highscore in highScoreDatas)
        {
            scoreDict.TryAdd(highscore.level_number, highscore.score);
        }
    }

    public void DictToList()
    {
        highScoreDatas.Clear();
        foreach (var scoreData in scoreDict)
        {
            highScoreDatas.Add(new HighScoreData{level_number = scoreData.Key, score = scoreData.Value});
        }
    }
}

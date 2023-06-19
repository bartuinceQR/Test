using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelJSONConverter
{

    public static LevelData ConvertToLevelData(string text)
    {
        LevelData levelData = new LevelData();

        var lines = text.Split('\n');
        levelData.level_number = Convert.ToInt32(lines[0].Split(':')[1]);
        levelData.grid_width = Convert.ToInt32(lines[1].Split(':')[1]);
        levelData.grid_height = Convert.ToInt32(lines[2].Split(':')[1]);
        levelData.move_count = Convert.ToInt32(lines[3].Split(':')[1]);

        var gridItems = lines[4].Split(':')[1].Split(',');

        levelData.grid = new List<ItemType>();
        foreach (var item in gridItems)
        {
            levelData.grid.Add(GetItemTypeFromChar(item));
        }

        return levelData;

    }

    static ItemType GetItemTypeFromChar(string c)
    {
        switch (c.Trim())
        {
            case "r":
                return ItemType.Red;
            case "b":
                return ItemType.Blue;
            case "g":
                return ItemType.Green;
            case "y": //creative liberties
                return ItemType.Colorless;
            default:
                throw new NotImplementedException("Bad colour code!");
        }
    }
    
}

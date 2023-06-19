using System;
using System.Collections.Generic;

[Serializable]
public class LevelData : IComparable<LevelData>
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public List<ItemType> grid;

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(LevelData other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var levelNumberComparison = level_number.CompareTo(other.level_number);
        return levelNumberComparison;
    }
}

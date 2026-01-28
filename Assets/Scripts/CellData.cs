using JetBrains.Annotations;

public record CellData
{
    public int CurrentGrowthDay;
    [CanBeNull] public Plant Planted;

    public bool IsEmpty()
    {
        return !Planted;
    }

    [CanBeNull]
    public Plant Harvest()
    {
        if (IsEmpty()) return null;
        CurrentGrowthDay = 0;
        var harvested = Planted;
        Planted = null;
        return harvested;
    }
}
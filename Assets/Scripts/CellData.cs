using JetBrains.Annotations;

public record CellData
{
    [CanBeNull] public Plant Planted;
    public int CurrentGrowthDay;

    public bool IsEmpty() => Planted == null;

    public Plant Harvest()
    {
        if (IsEmpty()) return null;
        CurrentGrowthDay = 0;
        var harvested = Planted;
        Planted = null;
        return harvested;
    }
}
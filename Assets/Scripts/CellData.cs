using JetBrains.Annotations;

public record CellData
{
    [CanBeNull] public Plant Planted;
    public int CurrentGrowthDay;

    public bool IsEmpty() => Planted == null;
}
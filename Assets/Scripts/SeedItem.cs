using IMS;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedItem", menuName = "HungryBunnies/Seed Item")]
public class SeedItem : ScriptableObject, IItem
{
    public Plant plant;

    [NotNull]
    public string GetName()
    {
        return $"{plant.Label} Seeds";
    }

    public int GetMaxQuantity()
    {
        return 3; // TODO: Dynamic max quantity
    }

    public Sprite GetSprite()
    {
        return plant.SeedSprite;
    }
}
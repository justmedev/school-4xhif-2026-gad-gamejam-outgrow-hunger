using IMS;
using UnityEngine;

public class SeedItem : IItem
{
    public Plant Plant;

    public string GetName()
    {
        return $"{Plant.Label} Seeds";
    }

    public int GetMaxQuantity()
    {
        return 3; // TODO: Dynamic max quantity
    }

    public Sprite GetSprite()
    {
        return Plant.SeedSprite;
    }
}
using IMS;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedItem", menuName = "HungryBunnies/Seed Item")]
public class SeedItem : ScriptableObject, IItem
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
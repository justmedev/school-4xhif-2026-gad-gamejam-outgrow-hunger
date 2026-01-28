using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantStage", menuName = "HungryBunnies/plant Stage", order = 0)]
public class PlantStage : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private float doubleSeedChance;
    [SerializeField] private int saturation;

    public Sprite Sprite => sprite;
    public float DoubleSeedChance => doubleSeedChance;
    public int Saturation => saturation;
}
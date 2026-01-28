using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantStage", menuName = "HungryBunnies/Plant Stage", order = 0)]
public class PlantStage : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;
    [SerializeField] private float doubleSeedChance;
    [SerializeField] private int saturation;

    [SerializeField, Tooltip("saturation will be rounded.")]
    private float saturationMultiplier;

    public float DoubleSeedChance => doubleSeedChance;
    public int Saturation => (int)Math.Round(saturation * saturationMultiplier);
}
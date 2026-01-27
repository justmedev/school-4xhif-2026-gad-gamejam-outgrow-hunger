using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "HungryBunnies/Plant", order = 0)]
public class Plant : ScriptableObject
{
    [SerializeField] private string label;
    [SerializeField] private List<PlantStage> stages;
    [SerializeField] private Sprite seedSprite;

    public string Label => label;
    public List<PlantStage> Stages => stages;
    public Sprite SeedSprite => seedSprite;
}
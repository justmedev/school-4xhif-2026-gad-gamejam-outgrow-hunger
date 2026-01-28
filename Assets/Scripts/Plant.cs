using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "HungryBunnies/Plant", order = 0)]
public class Plant : ScriptableObject
{
    [SerializeField] private string label;
    [SerializeField] private List<PlantStage> stages;
    [SerializeField] private Sprite resourceSprite;
    [SerializeField] private Sprite seedSprite;
    [SerializeField] private SeedItem seedItem;

    public string Label => label;
    public List<PlantStage> Stages => stages;
    public Sprite ResourceSprite => resourceSprite;
    public Sprite SeedSprite => seedSprite;
    public SeedItem SeedItem => seedItem;
}
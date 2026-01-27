using UnityEngine;

[CreateAssetMenu(fileName = "PlantStage", menuName = "HungryBunnies/Plant Stage", order = 0)]
public class PlantStage : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;
    [SerializeField] private float doubleSeedChance;
    public float DoubleSeedChance => doubleSeedChance;
}
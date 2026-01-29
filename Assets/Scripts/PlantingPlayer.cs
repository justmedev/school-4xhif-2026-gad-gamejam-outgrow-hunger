using System;
using System.Collections.Generic;
using IMS;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ItemCollectAnimationPlayer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlantingPlayer : MonoBehaviour
{
    private static readonly int AnimPropHarvest = Animator.StringToHash("Harvest");
    [SerializeField] private Tilemap highlightTilemap;
    [SerializeField] private Tilemap cropTilemap;
    [SerializeField] private Tilemap fieldTilemap;
    [SerializeField] private Tile highlightTile;
    private readonly Dictionary<Vector3Int, CellData> _fieldData = new();
    private Animator _anim;
    private AudioManager _audioMan;
    private AudioSource _audioSource;
    private ItemCollectAnimationPlayer _collectItemAnimPlayer;
    private GameStateManager _gsm;
    private InventoryHolder _hotbarHolder;
    private InputAction _interactAction;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _collectItemAnimPlayer = GetComponent<ItemCollectAnimationPlayer>();
        _audioSource = GetComponent<AudioSource>();
        _audioMan = FindFirstObjectByType<AudioManager>();
        _gsm = FindFirstObjectByType<GameStateManager>();

        EventBus.Instance.OnDayChanged += _ =>
        {
            cropTilemap.ClearAllTiles();
            foreach (var (cellPos, cell) in _fieldData)
            {
                if (cell.IsEmpty()) continue;
                cell.CurrentGrowthDay = Math.Min(cell.CurrentGrowthDay + 1, cell.Planted!.Stages.Count - 1);

                var stage = cell.Planted?.Stages[Math.Min(cell.CurrentGrowthDay, cell.Planted.Stages.Count - 1)];
                if (!stage) continue;

                SetPlantingTileFromStage(cellPos, stage);
            }
        };

        cropTilemap.ClearAllTiles();
        _hotbarHolder = FindFirstObjectByType<InventoryHolder>();

        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += _ => HandleCropFieldInteraction();

        AddMissingTilesToFieldData();
    }

    private void Update()
    {
        highlightTilemap.ClearAllTiles();
        var cellPos = fieldTilemap.WorldToCell(transform.position);
        if (!_fieldData.ContainsKey(cellPos)) return;
        highlightTilemap.SetTile(cellPos, highlightTile);
    }

    private void SetPlantingTileFromStage(Vector3Int cellPos, [NotNull] PlantStage stage)
    {
        TileBase tile;
        if (stage.AnimatedSprites.Length == 0)
        {
            tile = ScriptableObject.CreateInstance<Tile>();
            ((Tile)tile).sprite = stage.Sprite;
        }
        else
        {
            tile = ScriptableObject.CreateInstance<AnimatedTile>();
            var animated = (AnimatedTile)tile;
            animated.m_AnimatedSprites = stage.AnimatedSprites;
            animated.m_MinSpeed = 1;
            animated.m_MaxSpeed = 1;
        }

        cropTilemap.SetTile(cellPos, tile);
    }

    private void HandleCropFieldInteraction()
    {
        var cellPos = fieldTilemap.WorldToCell(transform.position);
        try
        {
            var cell = _fieldData[cellPos];
            if (!cell.IsEmpty())
            {
                if (cell.CurrentGrowthDay == 0) return;
                HarvestAtPos(cellPos);
                return;
            }

            var slotIndex = _hotbarHolder.SelectedInventorySlotIndex;
            if (_hotbarHolder.Hotbar.Slots[slotIndex].IsEmpty) return;
            _hotbarHolder.Hotbar.ModifySlotItemStack(slotIndex, (ref ItemStack stack) =>
            {
                var item = stack.TakeItem() as SeedItem;
                if (item == null) return;

                _audioSource.PlayOneShot(_audioMan.GetRandomPlantClip(), .5f);

                if (stack.IsEmpty) _hotbarHolder.Hotbar.Slots[slotIndex].RemoveItemStack();

                SetPlantingTileFromStage(cellPos, item.plant.Stages[0]);
                _fieldData[cellPos].Planted = item.plant;
            });
        }
        catch (KeyNotFoundException)
        {
        }
    }

    private void HarvestAtPos(Vector3Int cellPos)
    {
        var growthDay = _fieldData[cellPos].CurrentGrowthDay;
        var harvested = _fieldData[cellPos].Harvest();
        if (harvested == null) return;
        var stage = harvested.Stages[growthDay];

        _audioSource.PlayOneShot(_audioMan.GetRandomHarvestClip());
        _anim.SetTrigger(AnimPropHarvest);
        cropTilemap.SetTile(cellPos, null);

        var seedQty = 1;
        if (growthDay <= 0) return; // No seeds at all on day 1
        if (Random.value < stage.DoubleSeedChance) seedQty = 2;

        _gsm.AddSaturationLevel(stage.Saturation);
        _hotbarHolder.FillFirstAvailableSpace(new ItemStack(harvested.SeedItem, seedQty));

        _collectItemAnimPlayer.AddToQueue(
            seedQty,
            new Vector2(transform.position.x, transform.position.y + .5f),
            harvested.SeedSprite
        );

        _collectItemAnimPlayer.AddToQueue(
            stage.Saturation,
            new Vector2(transform.position.x, transform.position.y + .5f),
            harvested.ResourceSprite
        );
    }

    private void AddMissingTilesToFieldData()
    {
        var bounds = fieldTilemap.cellBounds;
        var allTiles = fieldTilemap.GetTilesBlock(bounds);

        for (var x = 0; x < bounds.size.x; x++)
        for (var y = 0; y < bounds.size.y; y++)
        {
            var tile = allTiles[x + y * bounds.size.x];
            var cellPos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

            if (tile != null && !_fieldData.ContainsKey(cellPos)) _fieldData.Add(cellPos, new CellData());
        }
    }
}
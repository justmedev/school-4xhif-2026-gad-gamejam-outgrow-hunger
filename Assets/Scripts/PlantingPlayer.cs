using System;
using System.Collections.Generic;
using IMS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PlantingPlayer : MonoBehaviour
{
    [SerializeField] private Tilemap cropTilemap;
    [SerializeField] private Tilemap fieldTilemap;
    private InputAction _interactAction;
    private readonly Dictionary<Vector3Int, CellData> _fieldData = new();
    private InventoryHolder _hotbarHolder;

    private void Start()
    {
        EventBus.Instance.OnDayChanged += day =>
        {
            cropTilemap.ClearAllTiles();
            foreach (var (cellPos, cell) in _fieldData)
            {
                if (cell.IsEmpty()) continue;
                cell.CurrentGrowthDay = Math.Min(cell.CurrentGrowthDay + 1, cell.Planted.Stages.Count - 1);
                var stage = cell.Planted?.Stages[Math.Min(cell.CurrentGrowthDay, cell.Planted.Stages.Count - 1)];
                Debug.Log($"Stage: {stage}");
                if (stage == null) continue; // TODO: ??? what
                SetPlantingTileFromStage(cellPos, stage);
            }
        };

        cropTilemap.ClearAllTiles();
        _hotbarHolder = FindFirstObjectByType<InventoryHolder>();

        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += _ => HandleCropFieldInteraction();

        AddMissingTilesToFieldData();
    }

    private void SetPlantingTileFromStage(Vector3Int cellPos, PlantStage stage)
    {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = stage.Sprite;
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
                HarvestAtPos(cellPos);
                return;
            }

            var slotIndex = _hotbarHolder.SelectedInventorySlotIndex;
            if (_hotbarHolder.Hotbar.Slots[slotIndex].IsEmpty) return;
            _hotbarHolder.Hotbar.ModifySlotItemStack(slotIndex, (ref ItemStack stack) =>
            {
                var item = stack.TakeItem() as SeedItem;
                if (item == null) return;

                if (stack.IsEmpty)
                {
                    _hotbarHolder.Hotbar.Slots[slotIndex].RemoveItemStack();
                }

                SetPlantingTileFromStage(cellPos, item.Plant.Stages[0]);
                _fieldData[cellPos].Planted = item.Plant;
            });
        }
        catch (KeyNotFoundException)
        {
        }
    }

    private void HarvestAtPos(Vector3Int cellPos)
    {
        var growthStage = _fieldData[cellPos].CurrentGrowthDay;
        var harvested = _fieldData[cellPos].Harvest();
        if (harvested == null) return;
        Debug.Log($"Harvested: {harvested}");

        cropTilemap.SetTile(cellPos, null);

        var qty = 1;
        // _hotbarHolder.Hotbar
        if (growthStage <= 0) return; // No seeds at all on day 1
        if (Random.value < harvested.Stages[growthStage].DoubleSeedChance)
        {
            qty = 2;
        }

        HotbarFillFirstAvailableSpace(new ItemStack(harvested.SeedItem, qty));
    }

    private void HotbarFillFirstAvailableSpace(ItemStack stack)
    {
        var inv = _hotbarHolder.Hotbar;
        foreach (var slot in inv.Slots)
        {
            if (slot.IsEmpty || slot.ItemStack == null)
            {
                slot.PlaceItemStack(stack);
                inv.PropagateChange(slot.Index);
                return;
            }

            if (!slot.ItemStack.Equals(stack)) continue;

            stack = slot.ItemStack.AddStack(stack);
            inv.PropagateChange(slot.Index);
            if (stack.IsEmpty) return;
        }
    }

    private void AddMissingTilesToFieldData()
    {
        var bounds = fieldTilemap.cellBounds;
        var allTiles = fieldTilemap.GetTilesBlock(bounds);

        for (var x = 0; x < bounds.size.x; x++)
        {
            for (var y = 0; y < bounds.size.y; y++)
            {
                var tile = allTiles[x + y * bounds.size.x];
                var cellPos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                if (tile != null && !_fieldData.ContainsKey(cellPos))
                {
                    _fieldData.Add(cellPos, new CellData());
                }
            }
        }
    }
}
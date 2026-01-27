using System.Collections.Generic;
using IMS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlantingPlayer : MonoBehaviour
{
    [SerializeField] private BoundsInt scanRange;
    [SerializeField] private Tilemap cropTilemap;
    [SerializeField] private Tilemap fieldTilemap;
    private InputAction _interactAction;
    private readonly Dictionary<Vector3Int, CellData> _fieldData = new();
    private InventoryHolder _hotbarHolder;

    private void Start()
    {
        cropTilemap.ClearAllTiles();
        _hotbarHolder = FindFirstObjectByType<InventoryHolder>();

        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += _ => HandleCropFieldInteraction();

        AddMissingTilesToFieldData();
    }

    private void HandleCropFieldInteraction()
    {
        var cellPos = fieldTilemap.WorldToCell(transform.position);
        try
        {
            var cell = _fieldData[cellPos];
            if (!cell.IsEmpty()) return;

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

                var tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = item.Plant.Stages[0].Sprite;
                cropTilemap.SetTile(cellPos, tile);

                _fieldData[cellPos].Planted = item.Plant;
            });
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogWarning(e);
            Debug.LogWarning($"Unable to find fieldData for position {cellPos}");
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
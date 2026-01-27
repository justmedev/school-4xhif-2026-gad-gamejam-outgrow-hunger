using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlantingPlayer : MonoBehaviour
{
    [SerializeField] private BoundsInt scanRange;
    [SerializeField] private Tilemap fieldTilemap;
    private InputAction _interactAction;
    private readonly Dictionary<Vector3Int, CellData> _fieldData = new();

    private void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += _ =>
        {
            var cellPos = fieldTilemap.WorldToCell(transform.position);
            try
            {
                var cell = _fieldData[cellPos];
                if (!cell.IsEmpty()) return;
                Debug.Log("Plant here!");
            }
            catch (KeyNotFoundException)
            {
            }
        };

        AddMissingTilesToFieldData();
    }

    private void AddMissingTilesToFieldData()
    {
        var bounds = fieldTilemap.cellBounds;
        var allTiles = fieldTilemap.GetTilesBlock(bounds);

        for (var x = 0; x < bounds.size.x; x++) {
            for (var y = 0; y < bounds.size.y; y++) {
                var tile = allTiles[x + y * bounds.size.x];
                var cellPos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                if (tile != null && !_fieldData.ContainsKey(cellPos)) {
                    _fieldData.Add(cellPos, new CellData());
                }
            }
        }
    }
}
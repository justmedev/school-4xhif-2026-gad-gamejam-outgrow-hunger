using System;
using IMS;
using IMS.UI;
using IMS.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryHolder : MonoBehaviour
{
    // TODO: REMOVE
    [SerializeField] private SeedItem[] startItems;
    [SerializeField] private UIDocument doc;
    private Inventory _hotbar;
    private readonly OnDropHandler _onDropHandler = new();
    private int _selectedInventorySlotIndex = 0;
    private const int InventoryCols = 4;
    private UQueryBuilder<VisualElement> _renderedSlots;
    private StyleColor _initialBgColor;

    private void Start()
    {
        var inventoryRoot = doc.rootVisualElement.Q("HotbarRoot");
        _hotbar = new Inventory(
            "Hotbar",
            InventoryCols,
            1,
            new InventoryUIOptions(inventoryRoot)
            {
                SlotSize = 90,
                Spacing = 4,
                ItemRoot = doc.rootVisualElement.Q("HotbarItems"),
            }
        );
        _hotbar.SetUIManagerItemVisualElementModifier((ref VisualElement ve) =>
            CreateItemVisualElementModifier(_hotbar, doc.rootVisualElement, ref ve));


        if (startItems.Length > InventoryCols) throw new ArgumentException($"startItems maxlength = {InventoryCols}");
        for (var i = 0; i < startItems.Length; i++)
        {
            _hotbar.PlaceItemStack(i, new ItemStack(startItems[i], 2));
        }

        _renderedSlots = doc.rootVisualElement.Query(className: InventoryUIClasses.Slot);
        _initialBgColor = _renderedSlots.First().style.backgroundColor;
        MarkSelectedSlot();
        // Change selected item
        InputSystem.actions.FindAction("Previous").performed += _ =>
        {
            ResetSelectedSlotMarking();
            if (_selectedInventorySlotIndex == 0) _selectedInventorySlotIndex = InventoryCols - 1;
            else _selectedInventorySlotIndex--;
            MarkSelectedSlot();
        };

        InputSystem.actions.FindAction("Next").performed += _ =>
        {
            ResetSelectedSlotMarking();
            if (_selectedInventorySlotIndex == InventoryCols - 1) _selectedInventorySlotIndex = 0;
            else _selectedInventorySlotIndex++;
            MarkSelectedSlot();
        };

        InputSystem.actions.FindAction("SelectHotbarSlot").performed += ctx =>
        {
            ResetSelectedSlotMarking();
            _selectedInventorySlotIndex = (int)ctx.ReadValue<float>() - 1;
            MarkSelectedSlot();
        };
    }

    private void ResetSelectedSlotMarking()
    {
        _renderedSlots.AtIndex(_selectedInventorySlotIndex).style.backgroundColor = _initialBgColor;
    }

    private void MarkSelectedSlot()
    {
        _renderedSlots.AtIndex(_selectedInventorySlotIndex).style.backgroundColor =
            new StyleColor(new Color(95, 80, 62));
    }

    private void CreateItemVisualElementModifier(Inventory inventory, VisualElement inventoryRoot,
        ref VisualElement ve)
    {
        var dm = new DragManipulator();
        dm.OnDrop += (pointerEvent, target) => _onDropHandler.HandleOnDrop(new ExternalDropData(
            inventoryRoot,
            target,
            pointerEvent,
            inventory
        ));
        ve.AddManipulator(dm);
    }
}
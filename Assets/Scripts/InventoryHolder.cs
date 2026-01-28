using System;
using IMS;
using IMS.UI;
using IMS.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryHolder : MonoBehaviour
{
    private const int InventoryCols = 4;

    [SerializeField] private SeedItem[] startItems;
    [SerializeField] private UIDocument doc;
    private readonly OnDropHandler _onDropHandler = new();
    private StyleColor _initialBgColor;
    private UQueryBuilder<VisualElement> _renderedSlots;
    public int SelectedInventorySlotIndex { get; private set; }
    public Inventory Hotbar { get; private set; }

    private void Start()
    {
        var inventoryRoot = doc.rootVisualElement.Q("HotbarRoot");
        Hotbar = new Inventory(
            "Hotbar",
            InventoryCols,
            1,
            new InventoryUIOptions(inventoryRoot)
            {
                SlotSize = 90,
                Spacing = 4,
                ItemRoot = doc.rootVisualElement.Q("HotbarItems")
            }
        );
        Hotbar.SetUIManagerItemVisualElementModifier((ref VisualElement ve) =>
            CreateItemVisualElementModifier(Hotbar, doc.rootVisualElement, ref ve));


        if (startItems.Length > InventoryCols) throw new ArgumentException($"startItems maxlength = {InventoryCols}");
        for (var i = 0; i < startItems.Length; i++) Hotbar.PlaceItemStack(i, new ItemStack(startItems[i], 2));

        _renderedSlots = doc.rootVisualElement.Query(className: InventoryUIClasses.Slot);
        _initialBgColor = _renderedSlots.First().style.backgroundColor;
        MarkSelectedSlot();
        // Change selected item
        InputSystem.actions.FindAction("Previous").performed += _ =>
        {
            ResetSelectedSlotMarking();
            if (SelectedInventorySlotIndex == 0) SelectedInventorySlotIndex = InventoryCols - 1;
            else SelectedInventorySlotIndex--;
            MarkSelectedSlot();
        };

        InputSystem.actions.FindAction("Next").performed += _ =>
        {
            ResetSelectedSlotMarking();
            if (SelectedInventorySlotIndex == InventoryCols - 1) SelectedInventorySlotIndex = 0;
            else SelectedInventorySlotIndex++;
            MarkSelectedSlot();
        };

        InputSystem.actions.FindAction("SelectHotbarSlot").performed += ctx =>
        {
            ResetSelectedSlotMarking();
            SelectedInventorySlotIndex = (int)ctx.ReadValue<float>() - 1;
            MarkSelectedSlot();
        };
    }

    private void ResetSelectedSlotMarking()
    {
        _renderedSlots.AtIndex(SelectedInventorySlotIndex).style.backgroundColor = _initialBgColor;
    }

    private void MarkSelectedSlot()
    {
        _renderedSlots.AtIndex(SelectedInventorySlotIndex).style.backgroundColor =
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
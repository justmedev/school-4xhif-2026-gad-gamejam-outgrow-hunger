using System;
using IMS;
using IMS.UI;
using IMS.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryHolder : MonoBehaviour
{
    // TODO: REMOVE
    [SerializeField] private SeedItem[] startItems;
    [SerializeField] private UIDocument doc;
    private Inventory _hotbar;
    private readonly OnDropHandler _onDropHandler = new();

    private void Start()
    {
        var inventoryRoot = doc.rootVisualElement.Q("HotbarRoot");
        _hotbar = new Inventory(
            "Hotbar",
            4,
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


        if (startItems.Length > 4) throw new ArgumentException("startItems maxlength = 4");
        for (var i = 0; i < startItems.Length; i++)
        {
            _hotbar.PlaceItemStack(i, new ItemStack(startItems[i], 2));
        }
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
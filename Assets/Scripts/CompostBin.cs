using System.Collections.Generic;
using IMS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class CompostBin : MonoBehaviour
{
    [SerializeField] private UIDocument worldDoc;
    [SerializeField] private float interactRange;
    [SerializeField] private ContactFilter2D castFilter;
    [SerializeField] private int currentContents;
    [SerializeField] private int requiredContents = 5;
    private AudioManager _audioMan;
    private AudioSource _audioSource;
    private InventoryHolder _hb;
    private ItemCollectAnimationPlayer _popupPlayer;
    private ProgressBar _progressBar;

    public void Start()
    {
        _hb = FindFirstObjectByType<InventoryHolder>();
        _audioMan = FindFirstObjectByType<AudioManager>();
        _popupPlayer = FindFirstObjectByType<ItemCollectAnimationPlayer>();

        _audioSource = GetComponent<AudioSource>();

        _progressBar = worldDoc.rootVisualElement.Q<ProgressBar>("ComposterProgress");
        _progressBar.value = currentContents;
        _progressBar.highValue = requiredContents;
        _progressBar.title = "Composter";

        InputSystem.actions.FindAction("Interact").performed += _ =>
        {
            var results = new List<RaycastHit2D>();
            Physics2D.CircleCast(transform.position, 1f, Vector2.zero, castFilter, results);
            if (results.Count <= 0) return;

            var player = results.Find(r => r.transform.CompareTag("Player"));
            if (!player) return;

            var slot = _hb.Hotbar.Slots[_hb.SelectedInventorySlotIndex];
            if (slot.IsEmpty) return;
            _hb.Hotbar.ModifySlotItemStack(slot.Index, (ref ItemStack stack) =>
            {
                var item = stack.TakeItem();
                if (item == null) return;
                if (stack.IsEmpty) _hb.Hotbar.Slots[slot.Index].RemoveItemStack();

                _audioSource.PlayOneShot(_audioMan.GetRandomPlantClip(), .5f);
                _popupPlayer.AddToQueue(
                    -1,
                    new Vector2(transform.position.x, transform.position.y + 1),
                    item.GetSprite()
                );

                currentContents++;
                _progressBar.value = currentContents;
                if (currentContents >= requiredContents)
                {
                    EmptyComposter();
                }
            });
        };
    }

    private void EmptyComposter()
    {
        var seed = _hb.GetRandomLockedItem();
        _hb.UnlockItem(seed);

        _hb.FillFirstAvailableSpace(new ItemStack(seed, 2));
        currentContents = 0;
        _progressBar.value = currentContents;
    }
}
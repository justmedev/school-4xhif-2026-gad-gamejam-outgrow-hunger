using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCollectAnimationPlayer : MonoBehaviour
{
    [SerializeField] private UIDocument prefab;
    private bool _isExecutorRunning;
    private readonly Queue<Tuple<int, Vector3, Sprite>> _popupQueue = new();

    public void AddToQueue(int amount, Vector3 worldPosition, Sprite collectedResourceImg)
    {
        _popupQueue.Enqueue(Tuple.Create(amount, worldPosition, collectedResourceImg));
        if (!_isExecutorRunning) StartCoroutine(QueueExecutor());
    }

    private IEnumerator QueueExecutor()
    {
        _isExecutorRunning = true;
        while (_popupQueue.Count > 0)
        {
            TriggerPopup(_popupQueue.Dequeue());
            yield return new WaitForSeconds(0.35f);
        }

        _isExecutorRunning = false;
    }

    private void TriggerPopup([NotNull] Tuple<int, Vector3, Sprite> data)
    {
        var (amount, worldPosition, collectedResourceImg) = data;
        var doc = Instantiate(prefab, worldPosition, Quaternion.identity);

        var container = new VisualElement();
        container.AddToClassList("collect-popup");

        var label = new Label { text = $"+{amount}" };
        if (amount < 0) label.text = $"-{amount}";
        container.Add(label);

        var image = new Image { sprite = collectedResourceImg };
        container.Add(image);

        doc.rootVisualElement.Add(container);
        container.RegisterCallback<GeometryChangedEvent>(_ =>
        {
            container.AddToClassList("collect-popup-active");
            container.schedule.Execute(() =>
            {
                container.RemoveFromClassList("collect-popup-active");
                container.AddToClassList("collect-popup-fadeout");
            }).StartingIn(300);
            container.schedule.Execute(() => container.RemoveFromHierarchy()).StartingIn(500);
        });
        Destroy(doc.gameObject, 0.5f);
    }
}
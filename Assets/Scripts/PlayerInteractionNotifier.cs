using UnityEngine;

public class PlayerInteractionNotifier : MonoBehaviour
{
    [SerializeField] private GameObject pressEMessage;

    public void ShowPressEMessage()
    {
        pressEMessage.SetActive(true);
    }

    public void HidePressEMessage()
    {
        pressEMessage.SetActive(false);
    }
}
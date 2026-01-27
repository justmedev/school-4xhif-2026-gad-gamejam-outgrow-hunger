using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlantingScript : MonoBehaviour
{
    private InputAction _interactAction;
    [SerializeField] private GameObject plantStage1Prefab;
    private bool _nearOtherPlant;
    private GameObject _currentPlant;


    private void OnEnable()
    {
        Debug.Log("OnEnable");
        _interactAction = InputSystem.actions.FindAction("Interact");
        Debug.Log(_interactAction);
    }

    private void Update()
    {
        if (_interactAction.WasPressedThisFrame() && !_nearOtherPlant)
        {
            Debug.Log("Interact");
            Vector2 spawnPos = transform.position;
            Instantiate(plantStage1Prefab, spawnPos, Quaternion.identity);
        }

        if (_interactAction.WasPressedThisFrame() && _nearOtherPlant)
        {
            if (!_currentPlant || !_currentPlant.GetComponent<Plant>().harvestable) return;
            StartCoroutine(Harvest());
        }
    }

    IEnumerator Harvest()
    {
        GetComponent<PlayerMovement>().canMove = false;
        yield return StartCoroutine(WaitAndHarvest(1.0f));
        Destroy(_currentPlant);
        GetComponent<PlayerMovement>().canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_nearOtherPlant)
        {
            _nearOtherPlant = true;
            _currentPlant = other.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_nearOtherPlant)
        {
            _nearOtherPlant = true;
            _currentPlant = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_nearOtherPlant)
        {
            _nearOtherPlant = false;
        }
    }

    IEnumerator WaitAndHarvest(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
using System;
using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour
{
    [SerializeField] private GameObject nextPlantStage;
    public bool harvestable;

    private IEnumerator _coroutine;

    private IEnumerator Start()
    {
        if (nextPlantStage != null)
        {
            yield return StartCoroutine(WaitAndGrow(3.3f));
            print("Done " + Time.time);
            Instantiate(nextPlantStage, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("PLANT FINISHED!!!");
            harvestable = true;
        }
    }

    private IEnumerator WaitAndGrow(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
using System;
using UnityEngine;
using System.Collections;

public class PlantScript : MonoBehaviour
{
    [SerializeField] private GameObject nextPlantStage;
    public bool harvestable = false;

    private IEnumerator coroutine;

    IEnumerator Start()
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

    IEnumerator WaitAndGrow(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}

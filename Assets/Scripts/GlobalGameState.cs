using Unity.Properties;
using UnityEngine;

public class GlobalGameState : MonoBehaviour
{
    [DontCreateProperty] public int FinalDay;

    public void Start()
    {
        DontDestroyOnLoad(this);
    }
}
using UnityEngine;

public class PigCleaner : MonoBehaviour
{
    public GameObject dirtyPig;
    public GameObject cleanPig;

    public void Clean()
    {
        dirtyPig.SetActive(false);
        cleanPig.SetActive(true);
    }
}
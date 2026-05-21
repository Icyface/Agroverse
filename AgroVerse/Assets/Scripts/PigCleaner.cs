using UnityEngine;

public class PigCleaner : MonoBehaviour
{
    public GameObject dirtyPig;
    public GameObject cleanPig;

    [Header("Número de pases necesarios")]
    public int hitsToClean = 3;

    private int currentHits = 0;

    public void CleanStep()
    {
        currentHits++;

        Debug.Log("Pasadas: " + currentHits);

        if (currentHits >= hitsToClean)
        {
            CleanComplete();
        }
    }

    void CleanComplete()
    {
        dirtyPig.SetActive(false);
        cleanPig.SetActive(true);

        Debug.Log("Cerdo limpio");
    }
}
using UnityEngine;

public class PigCleaner : MonoBehaviour
{
    public GameObject dirtyPig;
    public GameObject cleanPig;

    public int hitsToClean = 3;
    private int currentHits = 0;

    // 0 = sucio, 1 = mojado, 2 = enjabonado, 3 = limpio
    private int cleanState = 0;

    public void UseWater()
    {
        if (cleanState == 0)
        {
            cleanState = 1;
            Debug.Log("Cerdo mojado");
        }
        else if (cleanState == 2)
        {
            cleanState = 3;
            CleanComplete();
        }
    }

    public void UseSponge()
    {
        if (cleanState != 1)
        {
            Debug.Log("Primero moja el cerdo");
            return;
        }

        currentHits++;
        Debug.Log("Frotando: " + currentHits);

        if (currentHits >= hitsToClean)
        {
            cleanState = 2;
            Debug.Log("Cerdo enjabonado");
        }
    }

    void CleanComplete()
    {
        Debug.Log("Cerdo limpio");

        dirtyPig.SetActive(false);
        cleanPig.SetActive(true);

        // ── LÍNEA AÑADIDA ──
        TaskManager.Instance?.CompleteTask("limpiar_cerdo");
    }
}
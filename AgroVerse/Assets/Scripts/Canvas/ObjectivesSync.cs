using UnityEngine;

public class ObjectivesSync : MonoBehaviour
{
    public ObjectivesUI objectivesUI;

    private void Start()
    {
        if (objectivesUI == null)
        {
            objectivesUI = GetComponent<ObjectivesUI>();
        }

        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onTaskCompleted.AddListener(OnGlobalTaskCompleted);
        }

        SincronizarEstadoActual();
    }

    private void OnDestroy()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onTaskCompleted.RemoveListener(OnGlobalTaskCompleted);
        }
    }

    private void OnGlobalTaskCompleted(string taskId)
    {
        SincronizarEstadoActual();
    }

    private void SincronizarEstadoActual()
    {
        if (TaskManager.Instance == null || objectivesUI == null) return;

        bool eggDone = TaskManager.Instance.IsTaskCompleted("recoger_huevo");
        bool feedDone = TaskManager.Instance.IsTaskCompleted("alimentar_chicken");
        bool pigDone = TaskManager.Instance.IsTaskCompleted("limpiar_cerdo");
        bool cowDone = TaskManager.Instance.IsTaskCompleted("munyir_vaca");

        int currentEggs = eggDone ? 3 : objectivesUI.eggs;
        int currentFeed = feedDone ? 2 : objectivesUI.feed;
        bool currentPig = pigDone ? true : objectivesUI.pigCleaned;
        bool currentCow = cowDone ? true : objectivesUI.cowMilked;

        objectivesUI.SetForcedValues(currentEggs, currentFeed, currentPig, currentCow);
    }
}
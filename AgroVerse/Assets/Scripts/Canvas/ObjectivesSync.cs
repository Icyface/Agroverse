using UnityEngine;

public class ObjectivesSync : MonoBehaviour
{
    public ObjectivesUI objectivesUI;

    void Start()
    {
        if (objectivesUI == null)
        {
            objectivesUI = GetComponent<ObjectivesUI>();
        }

        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onTaskCompleted.AddListener(OnGlobalTaskCompleted);
        }

        SincronizarEstadoInicial();
    }

    void OnDestroy()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onTaskCompleted.RemoveListener(OnGlobalTaskCompleted);
        }
    }

    private void OnGlobalTaskCompleted(string taskId)
    {
        if (objectivesUI == null) return;

        switch (taskId)
        {
            case "recoger_huevo":
                ForceCompleteEgg();
                break;
            case "alimentar_chicken":
                ForceCompleteFeed();
                break;
            case "limpiar_cerdo":
                objectivesUI.CleanPig();
                break;
            case "munyir_vaca":
                objectivesUI.MilkCow();
                break;
        }
    }

    private void SincronizarEstadoInicial()
    {
        if (TaskManager.Instance == null || objectivesUI == null) return;

        if (TaskManager.Instance.IsTaskCompleted("recoger_huevo")) ForceCompleteEgg();
        if (TaskManager.Instance.IsTaskCompleted("alimentar_chicken")) ForceCompleteFeed();
        if (TaskManager.Instance.IsTaskCompleted("limpiar_cerdo")) objectivesUI.CleanPig();
        if (TaskManager.Instance.IsTaskCompleted("munyir_vaca")) objectivesUI.MilkCow();
    }

    private void ForceCompleteEgg()
    {
        for (int i = 0; i < 3; i++) objectivesUI.AddEgg();
    }

    private void ForceCompleteFeed()
    {
        for (int i = 0; i < 3; i++) objectivesUI.FeedAnimal();
    }
}
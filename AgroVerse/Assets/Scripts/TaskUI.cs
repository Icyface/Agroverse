using UnityEngine;
using TMPro;

public class TaskUI : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshProUGUI taskListText;

    void Start()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onTaskCompleted.AddListener(OnTaskCompleted);
            TaskManager.Instance.onAllTasksCompleted.AddListener(OnAllDone);
        }

        UpdateUI();
    }

    void OnTaskCompleted(string taskId)
    {
        UpdateUI();
    }

    void OnAllDone()
    {
        taskListText.text = "✓ ¡Todas las tareas completadas!";
    }

    void UpdateUI()
    {
        if (TaskManager.Instance == null || taskListText == null) return;

        string text = "TAREAS\n\n";

        foreach (string id in TaskManager.Instance.taskIds)
        {
            bool done = TaskManager.Instance.IsTaskCompleted(id);
            string icon = done ? "✓" : "○";
            text += $"{icon} {id}\n";
        }

        taskListText.text = text;
    }
}
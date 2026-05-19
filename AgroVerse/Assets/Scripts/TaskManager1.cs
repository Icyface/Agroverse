using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : MonoBehaviour
{
    // Singleton para que cualquier script pueda acceder
    public static TaskManager Instance { get; private set; }

    [Header("Tareas disponibles")]
    public List<string> taskIds = new List<string>
    {
        "recoger_huevo",
        "alimentar_chicken",
        "limpiar_cerdo",
        "munyir_vaca"
    };

    [Header("Eventos")]
    public UnityEvent<string> onTaskCompleted;
    public UnityEvent onAllTasksCompleted;

    private HashSet<string> _completedTasks = new HashSet<string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CompleteTask(string taskId)
    {
        if (_completedTasks.Contains(taskId))
        {
            Debug.Log($"[TaskManager] Tarea ya completada: {taskId}");
            return;
        }

        _completedTasks.Add(taskId);
        Debug.Log($"[TaskManager] ? Tarea completada: {taskId}");

        onTaskCompleted?.Invoke(taskId);

        if (_completedTasks.Count >= taskIds.Count)
        {
            Debug.Log("[TaskManager] ¡Todas las tareas completadas!");
            onAllTasksCompleted?.Invoke();
        }
    }

    public bool IsTaskCompleted(string taskId)
    {
        return _completedTasks.Contains(taskId);
    }

    public List<string> GetPendingTasks()
    {
        List<string> pending = new List<string>();
        foreach (string id in taskIds)
        {
            if (!_completedTasks.Contains(id))
                pending.Add(id);
        }
        return pending;
    }

    public List<string> GetCompletedTasks()
    {
        return new List<string>(_completedTasks);
    }
}
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TaskFlow : MonoBehaviour
{
    [System.Serializable]
    public class Task
    {
        public string taskName;
        public AnimalType targetAnimal;
        public bool isCompleted = false;
    }

    [Header("Lista de tareas en orden")]
    public List<Task> tasks = new List<Task>();

    [Header("Conectar con CompleteTask() de Adrián")]
    public UnityEvent<string> OnTaskCompleted;

    private int _currentTaskIndex = 0;

    // ── Métodos arrastrables desde el Inspector ──────────────

    public void OnPigTaskDone()
    {
        HandleTaskCompleted(AnimalType.Pig);
    }

    public void OnCowTaskDone()
    {
        HandleTaskCompleted(AnimalType.Cow);
    }

    public void OnChickenTaskDone()
    {
        HandleTaskCompleted(AnimalType.Chicken);
    }

    // ── Lógica central ───────────────────────────────────────

    void HandleTaskCompleted(AnimalType type)
    {
        if (_currentTaskIndex >= tasks.Count) return;

        Task current = tasks[_currentTaskIndex];

        if (current.isCompleted) return;
        if (current.targetAnimal != type) return;

        current.isCompleted = true;
        Debug.Log($"[TaskFlow] Tarea completada: {current.taskName}");

        OnTaskCompleted?.Invoke(current.taskName);

        _currentTaskIndex++;

        if (_currentTaskIndex < tasks.Count)
            Debug.Log($"[TaskFlow] Siguiente tarea: {tasks[_currentTaskIndex].taskName}");
        else
            Debug.Log("[TaskFlow] ¡Todas las tareas completadas!");
    }

    // Mantenemos este método por si Adrián lo llama directamente por código
    public void OnAnimalStateChanged(AnimalType type, string newState)
    {
        HandleTaskCompleted(type);
    }
}
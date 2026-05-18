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

    // Esto lo llaman los OnStateChanged de cada AnimalReaction
    // Arrastra este método en el Inspector de cada animal
    public void OnAnimalStateChanged(AnimalType type, string newState)
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
}
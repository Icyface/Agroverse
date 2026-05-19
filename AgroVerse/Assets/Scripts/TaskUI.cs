using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskUI : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshProUGUI taskListText;
    public Transform playerCamera; // La cámara del XR Origin

    [Header("Posición flotante")]
    public Vector3 offsetFromCamera = new Vector3(0f, -0.3f, 1.2f);
    public float followSpeed = 3f;

    void Start()
    {
        // Si no hay referencia manual, buscar la cámara
        if (playerCamera == null)
            playerCamera = Camera.main?.transform;

        // Suscribirse al TaskManager
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onTaskCompleted.AddListener(OnTaskCompleted);
            TaskManager.Instance.onAllTasksCompleted.AddListener(OnAllDone);
        }

        UpdateUI();
    }

    void LateUpdate()
    {
        // La UI sigue a la cámara suavemente
        if (playerCamera != null)
        {
            Vector3 targetPos = playerCamera.TransformPoint(offsetFromCamera);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
            transform.LookAt(playerCamera);
            transform.Rotate(0, 180, 0); // Girar para que mire al jugador
        }
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
using UnityEngine;
using UnityEngine.Events;

public enum AnimalType { Pig, Cow, Chicken }
public enum AnimalState { Initial, Active, Final } 
// Explicación de estados unificados:
// Pig:     Initial = Sucio,   Active = Mojado/Enjabonado, Final = Limpio
// Cow:     Initial = Con Leche (Full),                     Final = Ordeñada (Empty)
// Chicken: Initial = Hambrienta (Hungry),                  Final = Alimentada (Fed)

public class AnimalReaction : MonoBehaviour
{
    [Header("Configuración del Animal")]
    public AnimalType animalType;
    
    [Header("Estado Actual")]
    [SerializeField] private AnimalState currentState = AnimalState.Initial;

    [Header("Eventos de Animación y Sonido")]
    [Tooltip("Se activa cada vez que el animal cambia de estado (útil para sonido/animación)")]
    public UnityEvent<AnimalState> OnAnimalStateChanged;

    [Tooltip("Se activa ÚNICAMENTE cuando el animal completa su ciclo/tarea final")]
    public UnityEvent OnAnimalTaskComplete;

    // Propiedad pública para leer el estado actual desde otros scripts sin poder modificarlo directamente
    public AnimalState CurrentState => currentState;

    /// <summary>
    /// Cambia el estado del animal y dispara los eventos correspondientes.
    /// </summary>
    public void ChangeState(AnimalState newState)
    {
        if (currentState == newState) return; // Si ya está en ese estado, no hacemos nada

        currentState = newState;
        Debug.Log($"[{gameObject.name} ({animalType})] Cambió de estado a: {currentState}");

        // Invocar evento general con el nuevo estado
        OnAnimalStateChanged?.Invoke(currentState);

        // Si llega al estado final, avisamos que la tarea del animal ha terminado
        if (currentState == AnimalState.Final)
        {
            OnAnimalTaskComplete?.Invoke();
            Debug.Log($"[{gameObject.name}] ¡Ciclo de interacciones completado!");
        }
    }

    // ── Métodos de compatibilidad (para no romper los scripts que ya tenéis) ──
    
    public void Clean() => ChangeState(AnimalState.Final);
    public void Milk() => ChangeState(AnimalState.Final);
    public void Feed() => ChangeState(AnimalState.Final);

    public bool IsTaskComplete()
    {
        return currentState == AnimalState.Final;
    }
}
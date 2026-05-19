using UnityEngine;
using UnityEngine.Events;

public enum AnimalType { Pig, Cow, Chicken }

public enum PigState    { Dirty, Clean }
public enum CowState    { Full, Empty }
public enum ChickenState { Hungry, Fed }

[System.Serializable]
public class AnimalStateEvent : UnityEvent<AnimalType, string> { }

public class AnimalReaction : MonoBehaviour
{
    [Header("Tipo de animal")]
    public AnimalType animalType;

    [Header("Estados iniciales")]
    public PigState     pigState     = PigState.Dirty;
    public CowState     cowState     = CowState.Full;
    public ChickenState chickenState = ChickenState.Hungry;

    [Header("Eventos — conectar en Inspector")]
    public AnimalStateEvent OnStateChanged;

    // Específicos opcionales (más fácil de conectar a animaciones concretas)
    public UnityEvent OnPigCleaned;
    public UnityEvent OnCowMilked;
    public UnityEvent OnChickenFed;

    // ── API pública ──────────────────────────────────────────

    public void Clean()
    {
        if (animalType != AnimalType.Pig) return;
        if (pigState == PigState.Clean) return;

        pigState = PigState.Clean;
        OnPigCleaned?.Invoke();
        OnStateChanged?.Invoke(AnimalType.Pig, "Clean");
        Debug.Log("[AnimalReaction] Cerdo limpio");
    }

    public void Milk()
    {
        if (animalType != AnimalType.Cow) return;
        if (cowState == CowState.Empty) return;

        cowState = CowState.Empty;
        OnCowMilked?.Invoke();
        OnStateChanged?.Invoke(AnimalType.Cow, "Empty");
        Debug.Log("[AnimalReaction] Vaca ordeñada");
    }

    public void Feed()
    {
        if (animalType != AnimalType.Chicken) return;
        if (chickenState == ChickenState.Fed) return;

        chickenState = ChickenState.Fed;
        OnChickenFed?.Invoke();
        OnStateChanged?.Invoke(AnimalType.Chicken, "Fed");
        Debug.Log("[AnimalReaction] Gallina alimentada");
    }

    // ── Consulta de estado ────────────────────────────────────

    public bool IsTaskComplete()
    {
        return animalType switch
        {
            AnimalType.Pig     => pigState     == PigState.Clean,
            AnimalType.Cow     => cowState     == CowState.Empty,
            AnimalType.Chicken => chickenState == ChickenState.Fed,
            _                  => false
        };
    }

    public string GetCurrentStateLabel()
    {
        return animalType switch
        {
            AnimalType.Pig     => pigState.ToString(),
            AnimalType.Cow     => cowState.ToString(),
            AnimalType.Chicken => chickenState.ToString(),
            _                  => "Unknown"
        };
    }
}

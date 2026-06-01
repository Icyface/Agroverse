using UnityEngine;

public class PigNPC : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    public Transform pigSpawn;
    public Transform foodPoint;
    public Transform restPoint;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float eatingTime = 5f;

    private enum PigState
    {
        Dirty,
        Victory,
        GoingToFood,
        Eating,
        GoingToRest,
        Resting
    }

    private PigState currentState;

    private float timer;

    void Start()
    {
        transform.position = pigSpawn.position;

        currentState = PigState.Dirty;

        animator.SetBool("IsDirty", true);
    }

    void Update()
    {
        switch (currentState)
        {
            case PigState.GoingToFood:
                MoveTo(foodPoint.position);

                if (Vector3.Distance(transform.position, foodPoint.position) < 0.2f)
                {
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsEating", true);

                    currentState = PigState.Eating;
                    timer = eatingTime;
                }
                break;

            case PigState.Eating:
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    animator.SetBool("IsEating", false);
                    animator.SetBool("IsWalking", true);

                    currentState = PigState.GoingToRest;
                }
                break;

            case PigState.GoingToRest:
                MoveTo(restPoint.position);

                if (Vector3.Distance(transform.position, restPoint.position) < 0.2f)
                {
                    animator.SetBool("IsWalking", false);

                    currentState = PigState.Resting;
                }
                break;
        }
    }

    void MoveTo(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    [ContextMenu("Clean Pig")]
    public void CleanPig()
    {
        animator.SetBool("IsDirty", false);
        animator.SetTrigger("Cleaned");

        Invoke(nameof(StartWalking), 2f);
    }

    void StartWalking()
    {
        animator.SetBool("IsWalking", true);

        currentState = PigState.GoingToFood;
    }
}
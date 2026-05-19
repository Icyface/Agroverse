// TestBrushMover.cs — BORRAR cuando tengas las gafas
using UnityEngine;

public class TestBrushMover : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime);
    }
}
using UnityEngine;

public class Car : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake() {
         rb = GetComponent<Rigidbody2D>();
    }
    void OnMouseDown()
    {
        Move();
    }

    private void Move()
    {
        rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }
}

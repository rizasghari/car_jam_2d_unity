using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;

    private Vector3 initialPosition;

    [SerializeField]
    private List<Vector2> path = new List<Vector2> {
        Vector2.up,
        Vector2.left,
        Vector2.down,
    };

    private List<Vector2> backwardPath = new List<Vector2>();
    private int currentDirectionIndex = 0;
    private int currenBackwardIndex = 0;
    private bool isMoving = false;
    private bool isBackward = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    void OnMouseDown()
    {
        StartMoving();
    }

    private void Update() {
        if (isMoving && isBackward) {
            if (transform.position == initialPosition) {
                StopAndReset();
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            if (isBackward)
            {
                rb.linearVelocity = backwardPath[currenBackwardIndex] * speed;
            }
            else
            {
                rb.linearVelocity = path[currentDirectionIndex] * speed;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Intersection"))
        {
            Debug.Log("OnTriggerEnter2D");
            TurnAtIntersection();
        }
    }

    void StartMoving()
    {
        isMoving = true;
    }

    void TurnAtIntersection()
    {
        if (currentDirectionIndex == path.Count - 1) return;
        if (isBackward && currenBackwardIndex == backwardPath.Count - 1) return;

        if (isBackward)
        {
            currentDirectionIndex = (currentDirectionIndex - 1) % 0;
            currenBackwardIndex = (currenBackwardIndex + 1) % backwardPath.Count;
        } else {
            currentDirectionIndex = (currentDirectionIndex + 1) % path.Count;
        }

        var direction = path[currentDirectionIndex];
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle - 90); // -90 because the default facing is up
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car") && isMoving)
        {
            Debug.Log("OnCollisionEnter2D Current direction index: " + currentDirectionIndex);
            for (int i = currentDirectionIndex; i >= 0; i--)
            {
                Debug.Log("Adding to backward path: " + -path[i]);
                backwardPath.Add(-path[i]);
            }
            Debug.Log("Backward path: " + backwardPath.Count);
            isBackward = true;
        }
    }

    private void StopAndReset()
    {
        isMoving = false;
        currentDirectionIndex = 0;
        isBackward = false;
    }
}

using UnityEngine;

public abstract class Player:Entity
{
    private Rigidbody2D rb;
    protected virtual void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {
        HandleMovement();
    }
    protected virtual void HandleMovement()
    {
        Vector2 input = InputHandler.Instance.Movement;
        rb.velocity = input * moveSpeed;
    }
}
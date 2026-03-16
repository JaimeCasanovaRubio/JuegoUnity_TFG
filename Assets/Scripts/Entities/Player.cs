using UnityEngine;

public abstract class Player:Entity
{
    protected Rigidbody2D rb;
    protected virtual void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {
        HandleMovement();
        ExecAbility();
    }
    protected virtual void HandleMovement()
    {
        Vector2 input = InputHandler.Instance.Movement;
        rb.linearVelocity = input * moveSpeed;
    }
    protected virtual void ExecAbility(){}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Teleport tp = collision.GetComponent<Teleport>();
        if(tp!=null)
        {
            GameManager.Instance.ChangeScene(tp.sceneName);
        }
    }
}
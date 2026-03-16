using UnityEngine;

public class Hurtadilla:Player
{
    private float abilitySpeed = 100;
    protected override void ExecAbility()
    {
        if(InputHandler.Instance != null && InputHandler.Instance.AbilityPressed)
        {
            Vector2 dashDir = InputHandler.Instance.LastDirection switch
            {
                "left" => Vector2.left,
                "right" => Vector2.right,
                "down" => Vector2.down,
                "up" => Vector2.up,
                _ => Vector2.right
            };
            rb.linearVelocity = dashDir * abilitySpeed;
        }
    }

    protected override void OnDeath(){}
}
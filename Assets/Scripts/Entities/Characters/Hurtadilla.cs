using UnityEngine;

public class Hurtadilla:Player
{
    [SerializeField] private float abilitySpeed = 100;
    [SerializeField] private float dashDuration = 0.15f;
    private float dashTimer = 0f;
    
    protected override void ExecAbility()
    {
        if (CanUseAbility() && InputHandler.Instance != null && InputHandler.Instance.AbilityPressed)
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
            dashTimer = dashDuration;
            StartAbilityCooldown();
            if (SoundPlayer != null) SoundPlayer.PlayAbility();
        }
    }

    protected override void HandleMovement()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            return;
        }
        base.HandleMovement();
    }

}
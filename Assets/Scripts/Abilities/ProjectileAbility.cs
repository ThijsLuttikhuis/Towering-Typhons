using UnityEngine;

public class ProjectileAbility : Ability {
    [SerializeField] private Projectile projectile;

    protected bool isAttack = false;

    private bool UpdateAttackMove(float dt, Transform rayTarget, Vector3 rayPosition) {
        bool stillAttacking = true;

        Vector3 targetPos = rayTarget.transform.position;
        float distanceToTarget = (transform.position - targetPos).magnitude;

        if (timeSinceStart < windUpTime) {
            if (timeSinceStart > 0.0f) {
                if (distanceToTarget > range) {
                    stillAttacking = false;

                    Vector3 eulerAngles;
                    Vector3 position;
                    (eulerAngles, position) =
                        UpdatePlayerPosition(dt, targetPos, transform.position, player.getMoveSpeed);

                    player.Move(eulerAngles, position);
                }
            }
            else if (distanceToTarget > range * 0.9f) {
                stillAttacking = false;

                Vector3 eulerAngles;
                Vector3 position;
                (eulerAngles, position) =
                    UpdatePlayerPosition(dt, targetPos, transform.position, player.getMoveSpeed);

                player.Move(eulerAngles, position);
            }
        }

        return stillAttacking;
    }

    public override (bool, bool) UpdateCast(float dt, Transform rayTarget, Vector3 rayPosition) {
        if (!rayTarget.CompareTag("Enemy")) return (false, false);

        bool performedAction = true;
        bool returnToMoveAbility = false;
        
        // Check if you are in range, if not, first get in range
        bool stillAttacking = UpdateAttackMove(dt, rayTarget, rayPosition);

        if (cooldownRemaining > 0.0f) {
            performedAction = !stillAttacking;
            // Waiting for cooldown
        }
        else if (!stillAttacking) {
            // Not in range or not attacking
            timeSinceStart = 0.0f;
        }
        else if (timeSinceStart >= windUpTime) {
            // Attempt to cast
            performedAction = Cast(rayTarget, rayPosition);
            returnToMoveAbility = performedAction;
        }
        else {
            // Casting Windup
            timeSinceStart += dt;
        }

        return (performedAction, returnToMoveAbility);
    }

    protected override bool Cast(Transform targetTransform, Vector3 targetPosition) {
        bool success = false;

        // Check if you can cast abilities
        if (isAttack ? player.CanAttack() : player.CanCastAbilities()) {
            // Check if you have enough mana
            if (player.RemoveMana(manaCost)) {
                cooldownRemaining = cooldownTime - windUpTime;

                Projectile projectile_ = Instantiate(projectile);

                projectile_.SetTarget(targetTransform);
                projectile_.SetDamage(damage);
                projectile_.transform.position = transform.position;

                success = true;
            }
        }

        timeSinceStart = 0.0f;
        return success;
    }
}
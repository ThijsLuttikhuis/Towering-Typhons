using UnityEngine;

public class ProjectileAbility : Ability {
    [SerializeField] private Projectile projectile;

    protected bool isAttack = false;
    protected bool stopCastingOnCast = false;
    
    private bool UpdateAttackMove(float dt, Transform rayTarget, Vector3 rayPosition) {

        Vector3 targetPos = rayTarget.transform.position;
        float distanceToTarget = (transform.position - targetPos).magnitude;

        if (timeSinceStart < lockInTime) {
            if (timeSinceStart > 0.0f) {
                if (distanceToTarget > range) {
                    Vector3 eulerAngles;
                    Vector3 position;
                    (eulerAngles, position) =
                        UpdatePlayerPosition(dt, targetPos, transform.position, player.GetMoveSpeed);

                    return player.Move(eulerAngles, position);
                }
            }
            else if (distanceToTarget > range * 0.9f) {
                Vector3 eulerAngles;
                Vector3 position;
                (eulerAngles, position) =
                    UpdatePlayerPosition(dt, targetPos, transform.position, player.GetMoveSpeed);

                return player.Move(eulerAngles, position);
            }
        }

        return false;
    }

    protected override (bool, bool) OnUpdateCast(float dt, Transform rayTarget, Vector3 rayPosition) {
        if (!rayTarget) {
            return (false, true);
        }
        
        if (!rayTarget.CompareTag("Enemy")) {
            return (false, true);
        }

        bool performedAction;
        
        if (!isLocked) {
            // Check if you are in range, if not, first get in range
            performedAction = UpdateAttackMove(dt, rayTarget, rayPosition);
            if (performedAction) {
                timeSinceStart = 0.0f;
                isLocked = false;
                return (true, true);
            }
        }

        if (timeSinceStart >= windUpTime) {
            // Attempt to cast
            performedAction = Cast(rayTarget, rayPosition);        
            timeSinceStart = 0.0f;
            isLocked = false;
            return (performedAction, true);
        }
        
        // Casting windup and lock
        timeSinceStart += dt;
        isLocked = timeSinceStart >= lockInTime;
        
        return (false, true);
    }

    protected override bool Cast(Transform targetTransform, Vector3 targetPosition) {
        bool success = false;

        // Check if you can cast abilities
        if (isAttack ? player.CanAttack() : player.CanCastAbilities()) {
            // Check if you have enough mana
            if (player.RemoveMana(manaCost)) {
                SetCooldown();

                Projectile projectile_ = Instantiate(projectile);

                projectile_.SetTarget(targetTransform);
                projectile_.SetDamage(damage);
                projectile_.transform.position = transform.position;

                success = true;
            }
        }
        if (stopCastingOnCast) {
            StopCasting();
        }
        
        return success;
    }
}

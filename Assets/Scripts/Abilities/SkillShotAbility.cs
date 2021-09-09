using UnityEngine;

public class SkillShotAbility : Ability {
    [SerializeField] private SkillShot skillShot;
    
    protected bool stopCastingOnCast = true;
    
    private bool UpdateAttackMove(float dt, Transform rayTarget, Vector3 rayPosition) {
        
        Vector3 targetPos = rayPosition;
        float distanceToTarget = (transform.position - targetPos).magnitude;

        if (timeSinceStart < windUpTime) {
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
        
        if (rayTarget.CompareTag("Enemy")) {
            rayPosition = rayTarget.position;
            rayPosition.y = 0.0f;
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
            return (performedAction, false);
        }

        // Casting windup and lock
        timeSinceStart += dt;
        isLocked = timeSinceStart >= lockInTime;
        
        return (false, false);
    }

    protected override bool Cast(Transform targetTransform, Vector3 targetPosition) {
        bool success = false;

        // Check if you can cast abilities
        if (player.CanCastAbilities()) {
            // Check if you have enough mana
            if (player.RemoveMana(manaCost)) {
                SetCooldown();

                SkillShot skillShot_ = Instantiate(skillShot);

                skillShot_.SetDamage(damage);
                skillShot_.transform.position = player.transform.position;
                skillShot_.SetTargetPos(targetPosition);
                success = true;
            }
        }
        if (stopCastingOnCast) {
            StopCasting();
        }

        return success;
    }
}

using UnityEngine;

public class SkillShotAbility : Ability {
    [SerializeField] private SkillShot skillShot;

    private bool UpdateAttackMove(float dt, Transform rayTarget, Vector3 rayPosition) {
        bool stillAttacking = true;

        Vector3 targetPos = rayPosition;
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
        if (player.CanCastAbilities()) {
            // Check if you have enough mana
            if (player.RemoveMana(manaCost)) {
                cooldownRemaining = cooldownTime - windUpTime;

                SkillShot skillShot_ = Instantiate(skillShot);

                skillShot_.SetDamage(damage);
                skillShot_.transform.position = targetPosition;

                success = true;
            }
        }

        timeSinceStart = 0.0f;
        return success;
    }
}
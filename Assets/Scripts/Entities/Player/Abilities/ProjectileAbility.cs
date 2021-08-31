using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability {
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected Player player;

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

    public override bool UpdateCast(float dt, Transform rayTarget, Vector3 rayPosition) {
        bool stillAttacking = UpdateAttackMove(dt, rayTarget, rayPosition);
        
        if (cooldownRemaining > 0.0f) {
            cooldownRemaining -= dt;
        }
        else if (!stillAttacking) {
            timeSinceStart = 0.0f;
        }
        else if (timeSinceStart >= windUpTime) {

            if (player.RemoveMana(manaCost)) {
                cooldownRemaining = cooldownTime - windUpTime;
                Cast(rayTarget);

                timeSinceStart = 0.0f;
            }
            else {
                Debug.Log("No Mana!");

                timeSinceStart = 0.0f;
            }
        }
        else {
            timeSinceStart += dt;
        }

        return true;
    }

    protected override void Cast(Transform targetTransform) {
        Projectile projectile_ = Instantiate(projectile);

        projectile_.SetTarget(targetTransform);
        projectile_.SetDamage(damage);
        projectile_.transform.position = transform.position;
    }
}
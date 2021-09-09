using UnityEngine;

public class DashAbility : Ability {
    [SerializeField] private Dash dash;

    private Dash castedDash = default;

    protected override (bool, bool) OnUpdateCast(float dt, Transform rayTarget, Vector3 rayPosition) {
        
        // Block casting other abilities until the dash is complete
        if (isLocked) {
            SetCooldown();

            if (!castedDash) {
                StopCasting();
            }

            return (true, false);
        }
        
        if (rayTarget.CompareTag("Enemy")) {
            rayPosition = rayTarget.position;
            rayPosition.y = 0.0f;
        }

        bool performedAction = Cast(rayTarget, rayPosition);
        return (performedAction, false);
    }

    protected override bool Cast(Transform targetTransform, Vector3 targetPosition) {
        bool success = false;
        
        // Check if you can cast abilities
        if (player.CanCastAbilities()) {
            
            // Check if you have enough mana
            if (player.RemoveMana(manaCost)) {
                isLocked = true;

                SetCooldown();
                
                Dash dash_ = Instantiate(dash);

                dash_.SetPlayer(player);
                dash_.SetTargetPos(targetPosition);
                dash_.transform.position = player.transform.position;

                castedDash = dash_;
                success = true;
            }
        }
        
        return success;
    }
}
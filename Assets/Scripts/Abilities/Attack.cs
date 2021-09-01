using UnityEngine;

public class Attack : ProjectileAbility {
    private void Awake() {
        isAttack = true;
        
        cooldownTime = 0.6f;
        lockInTime = 0.1f;
        windUpTime = 0.15f;

        manaCost = 0.0f;
        damage = 50.0f;
        range = 8.0f;
    }
}
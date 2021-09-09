using UnityEngine;

public class Attack : ProjectileAbility {
    private void Awake() {
        isAttack = true;
        
        cooldownTime = 0.5f;
        lockInTime = 0.2f;
        windUpTime = 0.3f;

        manaCost = 0.0f;
        damage = 50.0f;
        range = 8.0f;
    }
    
    public override string GetName() {
        return "Attack";
    }
}
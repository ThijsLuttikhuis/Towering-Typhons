using System;
using UnityEngine;

public class Ability1 : ProjectileAbility {
    private void Awake() {
        stopCastingOnCast = true;
        
        cooldownTime = 4.0f;
        lockInTime = 1.2f;
        windUpTime = 1.2f;
        
        manaCost = 50.0f;
        damage = 10.0f;
        range = 15.0f;
    }
    
    public override string GetName() {
        return "Ability 1";
    }
}

using System;
using UnityEngine;

public class Ability1 : ProjectileAbility {
    private void Awake() {
        cooldownTime = 4.0f;
        lockInTime = 0.2f;
        windUpTime = 0.2f;
        
        manaCost = 50.0f;
        damage = 10.0f;
        range = 15.0f;
    }
}

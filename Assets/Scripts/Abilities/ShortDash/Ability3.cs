using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability3 : DashAbility {
    private void Awake() {
        cooldownTime = 4.0f;
        lockInTime = 0.0f;
        windUpTime = 0.0f;

        manaCost = 10.0f;
        damage = 0.0f;
        range = 0.0f;
    }

    public override string GetName() {
        return "Ability 3";
    }
}
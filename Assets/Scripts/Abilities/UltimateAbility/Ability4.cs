using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability4 : SkillShotAbility {
    private void Awake() {
        cooldownTime = 20.0f;
        lockInTime = 0.05f;
        windUpTime = 0.1f;

        manaCost = 100.0f;
        damage = 120.0f;
        range = 20.0f;
    }

    public override string GetName() {
        return "Ability 4";
    }
}

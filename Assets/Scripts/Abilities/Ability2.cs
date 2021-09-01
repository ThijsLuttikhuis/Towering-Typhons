using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability2 : SkillShotAbility {
    private void Awake() {
        cooldownTime = 1.0f;
        lockInTime = 0.1f;
        windUpTime = 0.1f;

        manaCost = 20.0f;
        damage = 120.0f;
        range = 20.0f;
    }
}
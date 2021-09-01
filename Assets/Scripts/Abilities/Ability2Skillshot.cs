using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability2Skillshot : SkillShot {
    protected override void OnCollisionWithTarget(Entity entity) {
        entity.TakeDamage(damage);
    }
}

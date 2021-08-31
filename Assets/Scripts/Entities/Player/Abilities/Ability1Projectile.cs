using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability1Projectile : Projectile {
    private float stunDuration;
    
    protected override void OnCollisionWithTarget(Entity entity) {
        entity.Stun(1.5f);
        entity.TakeDamage(damage);
    }
}

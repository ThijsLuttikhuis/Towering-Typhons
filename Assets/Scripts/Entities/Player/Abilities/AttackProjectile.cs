using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AttackProjectile : Projectile {
    
    protected override void OnCollisionWithTarget(Entity entity) {
        entity.TakeDamage(damage);
    }
}


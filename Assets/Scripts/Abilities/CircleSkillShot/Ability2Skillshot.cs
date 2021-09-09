using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability2Skillshot : SkillShot {
    private List<Entity> entitiesInCollider = new List<Entity>();
    
    protected override void OnCollision(Entity entity) {
        entitiesInCollider.Add(entity);
    }

    protected override void OnLeaveCollision(Entity entity) {
        if (entitiesInCollider.Contains(entity)) {
            entitiesInCollider.Remove(entity);
        }
        else {
            Debug.Log("Ability2Skillshot::OnLeaveCollision Entity not found!");
        }
    }

    protected override void OnFixedUpdate(float dt) {
        if (timeSinceStart <= duration) return;
        
        foreach (Entity entity in entitiesInCollider) {
            if (entity) {
                entity.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}

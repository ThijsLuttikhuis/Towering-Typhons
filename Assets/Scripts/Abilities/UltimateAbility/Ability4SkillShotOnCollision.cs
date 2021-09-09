using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability4SkillShotOnCollision : SkillShot {
    private List<Entity> entitiesInCollider = new List<Entity>();

    private float slowPercent = 60.0f;
    private float slowDuration = 0.1f;
    
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
        
        foreach (Entity entity in entitiesInCollider) {
            if (entity) {
                entity.Slow(slowPercent, slowDuration);
            }
        }

        if (timeSinceStart <= duration) return;
        
        foreach (Entity entity in entitiesInCollider) {
            if (entity) {
                entity.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

}

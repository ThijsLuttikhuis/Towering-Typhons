using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillShot : CollidableMonoBehaviour {
    
    protected float speed = 15.0f;
    protected float damage;
    protected float duration = 1.0f;
    private float timeSinceStart = 0.0f;

    public void SetDamage(float damage_) {
        damage = damage_;
    }
    
    public override void Collide(Collider other) {
        var entity = other.transform.parent.transform.GetComponent(typeof(Entity)) as Entity;
        if (!entity) {
            Debug.Log("Projectile::Collide Entity not found!");
            return;
        }

        if (entity.CompareTag("Enemy")) {
            OnCollisionWithTarget(entity);
        }
    }

    protected virtual void OnCollisionWithTarget(Entity entity) {
        entity.TakeDamage(damage);
    }

    private void FixedUpdate() {
        float dt = Time.deltaTime;

        timeSinceStart += dt;
        if (timeSinceStart > duration) {
            Destroy(gameObject);
        }
    }

}
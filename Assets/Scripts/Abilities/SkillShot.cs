using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillShot : CollidableMonoBehaviour {
    protected float damage;
    protected float duration = 1.0f;
    protected float timeSinceStart = 0.0f;
    protected Vector3 targetPos;
    
    public void SetDamage(float damage_) {
        damage = damage_;
    }
    
    public virtual void SetTargetPos(Vector3 targetPos_) {
        targetPos = targetPos_;
    }
    
    public override void Collide(Collider other) {
        var entity = other.transform.parent.transform.GetComponent(typeof(Entity)) as Entity;
        if (!entity) {
            Debug.Log("Projectile::Collide Entity not found!");
            return;
        }

        if (entity.CompareTag("Enemy")) {
            OnCollision(entity);
        }
    }

    public override void LeaveCollide(Collider other) {
        var entity = other.transform.parent.transform.GetComponent(typeof(Entity)) as Entity;
        if (!entity) {
            Debug.Log("Projectile::Collide Entity not found!");
            return;
        }

        if (entity.CompareTag("Enemy")) {
            OnLeaveCollision(entity);
        }
    }

    protected virtual void OnCollision(Entity entity) {
        entity.TakeDamage(damage);
    }

    protected virtual void OnLeaveCollision(Entity entity) {
        // do nothing
    }

    
    private void Start() {
        OnStart();
    }

    protected virtual void OnStart() {
        transform.position = targetPos;
    }

    private void FixedUpdate() {
        float dt = Time.deltaTime;
        timeSinceStart += dt;

        OnFixedUpdate(dt);
    }

    protected virtual void OnFixedUpdate(float dt) {
        
        if (timeSinceStart > duration) {
            Destroy(gameObject);
        }
    }
}
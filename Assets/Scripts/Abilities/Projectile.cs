using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : CollidableMonoBehaviour {
    
    protected float speed = 15.0f;
    protected float damage;

    protected Transform target;
    
    public void SetTarget(Transform target_) {
        target = target_;
    }

    public void SetDamage(float damage_) {
        damage = damage_;
    }
    
    public override void Collide(Collider other) {
        if (other.transform.parent.transform == target) {
            var entity = target.GetComponent(typeof(Entity)) as Entity;
            if (!entity) {
                Debug.Log("Projectile::Collide Entity not found!");
                return;
            }

            OnCollisionWithTarget(entity);
            Destroy(gameObject);
        }
    }

    
    protected virtual void OnCollisionWithTarget(Entity entity) {
        entity.TakeDamage(damage);
    }


    private void FixedUpdate() {
        float dt = Time.deltaTime;

        if (!target) {
            Destroy(gameObject);
            return;
        }
        
        Vector3 dPos = target.position - transform.position;
        transform.position += dPos.normalized * (dt * speed);
        
    }

}
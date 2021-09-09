using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : CollidableMonoBehaviour {
    private Player player;

    protected float speed = 25.0f;
    protected float distance = 10.0f;
    private Vector3 targetPos;
    private float lastYRotation;

    private float timeSinceStart;

    private float maxDuration() => distance / speed;

    public void SetPlayer(Player player_) {
        player = player_;
    }
    
    public void SetTargetPos(Vector3 targetPos_) {
        if (!player) {
            Debug.Log("Dash::SetTarget please set player!");
            return;
        }

        Vector3 pos = player.transform.position;

        targetPos_.y = pos.y;
        Vector3 dPos = targetPos_ - pos;

        if (dPos.magnitude <= distance) {
            targetPos = targetPos_;
        }
        else {
            targetPos = pos + dPos.normalized * distance;
        }
    }

    public override void Collide(Collider other) {
        var entity = other.transform.parent.transform.GetComponent(typeof(Entity)) as Entity;
        if (!entity) {
            Debug.Log("Dash::Collide Entity not found!");
            return;
        }
    
        if (entity.CompareTag("Enemy")) {
            OnCollisionWithTarget(entity);
        }
    }

    protected virtual void OnCollisionWithTarget(Entity entity) {
        // TODO: handle walls
        
        // Dash does not have a collision with most targets
    }

    private void FixedUpdate() {
        float dt = Time.deltaTime;

        timeSinceStart += dt;
        if (timeSinceStart > maxDuration() * 2.0f) {
            Destroy(gameObject);
            return;
        }
        
        if (!player) {
            Debug.Log("Dash::FixedUpdate please set player!");
            Destroy(gameObject);
            return;
        }
        
        Vector3 pos = player.transform.position;
        targetPos.y = pos.y;

        // if close to target, destroy dash object
        if ((pos - targetPos).magnitude < 0.05f) {
            Destroy(gameObject);
            return;
        }

        Vector3 dPos = targetPos - pos;

        float length = dPos.magnitude;
        float dxNormMax = speed * dt;
        dPos = dPos.normalized * Mathf.Min(length, dxNormMax);

        var rot = new Vector3(0, lastYRotation, 0);
        if (length > dxNormMax) {
            rot.y = 90 - Mathf.Atan2(dPos.z, dPos.x) / Mathf.Deg2Rad;
        }

        lastYRotation = rot.y;

        // if not moved, destroy dash object
        bool moved = player.Move(rot, pos + dPos);
        if (!moved) {
            Destroy(gameObject);
        }

        Debug.Log((pos - targetPos).magnitude);
    }
}
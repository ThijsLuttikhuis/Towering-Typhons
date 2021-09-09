using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability4SkillShot : SkillShot
{
    [SerializeField] private SkillShot skillShotOnCollision;

    private float speed = 15.0f;
    private float constantHeight = 0.5f;
    private Vector3 startPosition;
    
    public override void SetTargetPos(Vector3 targetPos_) {
        targetPos = targetPos_;
        constantHeight = transform.position.y;
    }
    
    protected override void OnCollision(Entity entity) {
        entity.Stun(2.0f);
        
        SkillShot skillShot_ = Instantiate(skillShotOnCollision);
        
        skillShot_.SetDamage(damage);
        
        Vector3 pos = transform.position;
        pos.y = 0.0f;
        skillShot_.SetTargetPos(pos);
        
        Destroy(gameObject);
    }
    
    protected override void OnStart() {
        targetPos.y = constantHeight;
        
        Vector3 pos = transform.position;
        pos.y = constantHeight;
        transform.position = pos;
    }
    
    protected override void OnFixedUpdate(float dt) {

        Vector3 dPos = targetPos - transform.position;
        if (dPos.magnitude < 0.05f) {
            Destroy(gameObject);
        }
        dPos = dPos.normalized * Mathf.Min(dPos.magnitude, dt * speed);
        transform.position += dPos;
        
        // if (timeSinceStart > duration) { 
        //     Destroy(gameObject);
        // }
    }
}

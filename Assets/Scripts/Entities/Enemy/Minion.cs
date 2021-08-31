using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Entity {
    
    protected override void Initialize() {
        maxHealthPoints = 260.0f;
        maxManaPoints = 0.0f;
        armorPoints = 30.0f;
        attackDamage = 20.0f;
        attackRange = 4.0f;

        moveSpeed = 4.0f;

        currentHealthPoints = maxHealthPoints;
        currentManaPoints = maxManaPoints;
    }

    protected override void OnFixedUpdate(float dt) {

        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;

        position.x += moveSpeed * dt;

        Move(eulerAngles, position);

    }
}


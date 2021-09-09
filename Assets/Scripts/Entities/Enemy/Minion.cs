using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Entity {
    
    protected override void Initialize() {
        maxHealthPoints = 260.0f;
        maxManaPoints = 0.0f;
        armor = 30.0f;
        magicResist = 30.0f;

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


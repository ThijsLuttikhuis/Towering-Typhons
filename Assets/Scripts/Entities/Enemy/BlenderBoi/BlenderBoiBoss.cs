using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderBoiBoss : Entity {
    [SerializeField] private BBBAbility bbbAbility;

    protected override void OnFixedUpdate(float dt) {

        //bbbAbility.UpdateCast(dt);

        // Vector3 position = transform.position;
        // Vector3 eulerAngles = transform.eulerAngles;
        //
        // position.x += 4.0f * Mathf.Sin(eulerAngles.y * Mathf.Deg2Rad) * dt;
        // position.z += 4.0f * Mathf.Cos(eulerAngles.y * Mathf.Deg2Rad) * dt;
        //
        // eulerAngles.y += 10.0f * dt;
        //
        // Move(eulerAngles, position);
    }
}
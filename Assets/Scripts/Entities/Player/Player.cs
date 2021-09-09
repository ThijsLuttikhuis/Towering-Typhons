using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Player : Entity {
    
    [SerializeField] private PlayerAbilities playerAbilities;
    [SerializeField] private PlayerCameraMovement playerCameraMovement;
    [SerializeField] private Camera playerCamera;

    private Transform rayTarget = default;
    private Vector3 rayPosition = default;
    
    internal (Transform, Vector3) GetRayPosition() {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Transform rayTarget_ = transform;
        Vector3 rayPosition_ = rayPosition;
        
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            rayTarget_ = hit.transform.parent.transform;
            
            if (rayTarget_.CompareTag("Terrain")) {
                rayPosition_ = hit.point;
            }
        }

        return (rayTarget_, rayPosition_);
    }

    protected override void Initialize() {
        rayPosition = transform.position;
        rayTarget = transform;
        
        maxHealthPoints = 500.0f;
        maxManaPoints = 250.0f;
        healthPerSecond = 3.0f;
        manaPerSecond = 2.0f;
        
        armor = 40.0f;
        magicResist = 40.0f;
        
        moveSpeed = 7.0f;
    }

    protected override void OnFixedUpdate(float dt) {
        playerCameraMovement.UpdatePlayerCamera(dt);
    }
}
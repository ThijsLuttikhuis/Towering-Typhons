using System;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour {
    [SerializeField] protected Player player;
    [SerializeField] private Slider cooldownSlider;

    protected PathFinder pathFinder = new PathFinder();

    protected Transform rayTarget;
    protected Vector3 rayPosition;

    private bool isStopped = false;
    private bool buttonPressed = false;
    protected bool isLocked = false;
    private float cooldownRemaining;
    protected float timeSinceStart;

    [SerializeField] protected float cooldownTime; // waiting time between consecutive casts
    [SerializeField] protected float lockInTime; // time until you cannot cancel the ability anymore
    [SerializeField] protected float windUpTime; // time before the ability first casts 

    [SerializeField] protected float manaCost; // mana cost of the ability
    [SerializeField] protected float damage; // damage the abililty deals
    [SerializeField] protected float range; // distance for which you can cast the ability

    public void ToggleButtonPressed() {
        buttonPressed = !buttonPressed;
        
        isStopped = false;
    }

    public Transform GetRayTarget() => rayTarget;
    public Vector3 GetRayPosition() => rayPosition;

    public bool GetButtonPressed() => buttonPressed;

    public float GetManaCost() => manaCost;
    public bool IsLocked() => isLocked;
    public bool IsOnCooldown() => cooldownRemaining > 0.0f;
    public float GetCooldownRemaining() => cooldownRemaining;
    public float GetCooldownTime() => cooldownTime;
    public void SetCooldown() {
        cooldownRemaining = cooldownTime - windUpTime;
    }

    public void SetCooldown(float time) {
        cooldownRemaining = time;
    }

    public virtual string GetName() {
        Debug.LogWarning("PlayerAbility::GetName needs to be overwritten");

        return "Ability";
    }

    public (bool, bool) UpdateCast(float dt) {
        if (isStopped) {
            isLocked = false;
            timeSinceStart = 0.0f;

            return (false, true);
        }
        
        if (isLocked) {
            if (!rayTarget) {
                isLocked = false;
                timeSinceStart = 0.0f;

                return (false, true);
            }
            return OnUpdateCast(dt, rayTarget, rayPosition);
        }

        // Check if the ability is on cooldown
        if (IsOnCooldown()) {
            Debug.Log(GetName() + " is still on cooldown!");
            // TODO: feedback for "still on cooldown"

            isLocked = false;
            timeSinceStart = 0.0f;
            return (false, true);
        }

        // Check if player has enough mana to cast the ability
        if (!player.HasMana(GetManaCost())) {
            Debug.Log("Not enough mana to cast " + GetName() + "!");
            // TODO: feedback for "no mana"
            
            isLocked = false;
            timeSinceStart = 0.0f;
            return (false, true);
        }

        if (buttonPressed) {
            UpdateRayTarget();
        }

        return OnUpdateCast(dt, rayTarget, rayPosition);
    }
    
    protected void StopCasting() {
        isLocked = false;
        timeSinceStart = 0.0f;

        isStopped = true;
    }

    protected virtual (bool, bool) OnUpdateCast(float dt, Transform rayTarget, Vector3 rayPosition) {
        Debug.LogWarning("PlayerAbility::UpdateCast needs to be overwritten");
        return (false, false);
    }

    protected virtual bool Cast(Transform targetTransform, Vector3 targetPosition) {
        Debug.LogWarning("PlayerAbility::Cast needs to be overwritten");
        return false;
    }

    internal (Vector3, Vector3) UpdatePlayerPosition(float dt, Vector3 targetPos, Vector3 playerPos, float maxVelocity) {
        return pathFinder.FindPath(targetPos, playerPos, dt, maxVelocity);
    }
    
    private void FixedUpdate() {
        float dt = Time.deltaTime;

        cooldownRemaining -= dt;
    }

    private void LateUpdate() {
        cooldownSlider.maxValue = cooldownTime;
        cooldownSlider.value = cooldownRemaining;
    }


    internal void UpdateRayTarget() {
        (rayTarget, rayPosition) = player.GetRayPosition();
    }

    internal void UpdateRayTarget(Transform rayTarget_, Vector3 rayPosition_) {
        rayTarget = rayTarget_;
        rayPosition = rayPosition_;
    }
    
    public void ResetRayTarget() {
        UpdateRayTarget(player.transform, player.transform.position);
    }
    
    protected class PathFinder {
        private float lastYRotation = default;

        public (Vector3, Vector3) FindPath(Vector3 targetPos, Vector3 pos, float dt, float maxVelocity) {
            targetPos.y = pos.y;
            Vector3 dPos = targetPos - pos;

            float length = dPos.magnitude;
            float dxNormMax = maxVelocity * dt;
            dPos = dPos.normalized * Mathf.Min(length, dxNormMax);

            var rot = new Vector3(0, lastYRotation, 0);
            if (length > dxNormMax) {
                rot.y = 90 - Mathf.Atan2(dPos.z, dPos.x) / Mathf.Deg2Rad;
            }

            lastYRotation = rot.y;


            return (rot, pos + dPos);
        }
    }
}
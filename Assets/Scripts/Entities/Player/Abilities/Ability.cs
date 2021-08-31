using UnityEngine;

public class Ability : MonoBehaviour {

    protected PathFinder pathFinder = new PathFinder();

    [SerializeField] protected bool isCasting = false;
    [SerializeField] protected float cooldownRemaining = 0.0f;
    [SerializeField] protected float timeSinceStart = 0.0f;
    
    [SerializeField] protected float cooldownTime = 5.0f;
    [SerializeField] protected float lockInTime = 0.1f;
    [SerializeField] protected float windUpTime = 0.2f;
    
    [SerializeField] protected float manaCost = 50.0f;
    [SerializeField] protected float damage = 10.0f;
    [SerializeField] protected float range = 15.0f;

    public virtual bool UpdateCast(float dt, Transform rayTarget, Vector3 rayPosition) {
        Debug.LogWarning("PlayerAbility::UpdateCast needs to be overwritten");

        return false;
    }

    public virtual bool AttemptCast() {
        Debug.LogWarning("PlayerAbility::AttemptCast needs to be overwritten");

        return false;
    }

    protected virtual void Cast(Transform targetTransform) {
        Debug.LogWarning("PlayerAbility::Cast needs to be overwritten");
    }

    internal (Vector3, Vector3) UpdatePlayerPosition(float dt, Vector3 targetPos, Vector3 playerPos, float maxVelocity) {
        return pathFinder.FindPath(targetPos, playerPos, dt, maxVelocity);
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

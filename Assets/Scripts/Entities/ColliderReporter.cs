using UnityEngine;

public class ColliderReporter : MonoBehaviour {
    [SerializeField] private CollidableMonoBehaviour collidableMonoBehaviour;
    
    private void OnTriggerEnter(Collider other) {
        collidableMonoBehaviour.Collide(other);
    }
    
    private void OnTriggerExit(Collider other) {
        collidableMonoBehaviour.LeaveCollide(other);
    }
}

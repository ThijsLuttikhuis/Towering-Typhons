using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableMonoBehaviour : MonoBehaviour {
    
    public virtual void Collide(Collider other) {
        Debug.Log("CollidableMonoBehaviour::Collide needs to be overwritten");
    }
}

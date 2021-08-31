using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    [SerializeField] internal Transform playerCamera;
    
    void LateUpdate()
    {
        transform.LookAt(transform.position + playerCamera.forward);

        float scale = (transform.position - playerCamera.transform.position).magnitude / 20.0f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}

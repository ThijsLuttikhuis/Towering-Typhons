using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform parentCanvas;

    public void SetPlayerCamera(Transform playerCamera_) {
        playerCamera = playerCamera_;
    }

    void LateUpdate() {
        if (!parentCanvas) return;
        
        parentCanvas.transform.LookAt(parentCanvas.transform.position + playerCamera.forward);

        float scale = (parentCanvas.transform.position - playerCamera.transform.position).magnitude / 20.0f;
        parentCanvas.transform.localScale = new Vector3(scale, scale, scale);
    }
}
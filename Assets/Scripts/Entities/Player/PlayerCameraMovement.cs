using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.LowLevel;

public class PlayerCameraMovement : MonoBehaviour {
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Player player;

    private CameraController cameraController = new CameraController();

    private bool buttonRotateCameraLeft = false;
    private bool buttonRotateCameraRight = false;
    private bool buttonRotateCameraUp = false;
    private bool buttonRotateCameraDown = false;
    private bool keepCameraCentered = false;

    private void OnCenterCamera() {
        keepCameraCentered = !keepCameraCentered;
    }

    private void OnToggleLockedCamera() {
        cameraController.ToggleLockedCamera();
    }

    private void OnRotateCameraLeft() {
        buttonRotateCameraLeft = !buttonRotateCameraLeft;
    }

    private void OnRotateCameraRight() {
        buttonRotateCameraRight = !buttonRotateCameraRight;
    }

    private void OnRotateCameraUp() {
        buttonRotateCameraUp = !buttonRotateCameraUp;
    }

    private void OnRotateCameraDown() {
        buttonRotateCameraDown = !buttonRotateCameraDown;
    }

    private void OnZoomCamera(InputValue value) {
        float dZoomCamera = Mathf.Clamp(value.Get<float>(), -1.0f, 1.0f);
        cameraController.ControlCamera(dZoomCamera, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    }

    private void Start() {
        Vector3 cameraEulerAngles = playerCamera.transform.eulerAngles;

        cameraController = new CameraController();
        cameraController.SetPlayer(player);
        cameraController.ControlCamera(0.0f, 0.0f, 0.0f,
            cameraEulerAngles.y, cameraEulerAngles.x, 1.0f / 150.0f);
        cameraController.CenterCamera();
    }

    private void Update() {
        Transform playerCameraTransform = playerCamera.transform;

        playerCameraTransform.position = cameraController.GetCameraPosition() + player.transform.position;
        playerCameraTransform.eulerAngles = cameraController.GetCameraEulerAngles();
    }

    public void UpdatePlayerCamera(float dt) {
        // Camera rotation
        float dRotCameraH = buttonRotateCameraLeft ? 1.0f : buttonRotateCameraRight ? -1.0f : 0.0f;
        float dRotCameraV = buttonRotateCameraUp ? 1.0f : buttonRotateCameraDown ? -1.0f : 0.0f;

        // Camera translation (unlocked mode)
        float dPosX = 0.0f;
        float dPosZ = 0.0f;
        if (dRotCameraH != 0.0f || dRotCameraV != 0.0f || keepCameraCentered) {
            cameraController.CenterCamera();
        }
        else {
            const float screenEdgeSize = 2.0f;
            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (mousePos.x >= Screen.width - screenEdgeSize) {
                dPosX = 1.0f;
            }
            else if (mousePos.x <= screenEdgeSize) {
                dPosX = -1.0f;
            }

            if (mousePos.y >= Screen.height - screenEdgeSize) {
                dPosZ = 1.0f;
            }
            else if (mousePos.y <= screenEdgeSize) {
                dPosZ = -1.0f;
            }
        }

        cameraController.ControlCamera(0.0f, dPosX, dPosZ, dRotCameraH, dRotCameraV, dt);
    }
}

public class CameraController {
    private Player player;
    
    private bool lockedCameraMode;

    private const float dPosVelocity = 60.0f;
    private const float rotVelocity = 150.0f;
    private float cameraDistanceToPlayer = 20.0f;

    private Vector3 cameraPos;
    private Vector3 cameraHorizontalDeltaPos;
    private Vector3 cameraRot;
    private Vector3 lastKnownPlayerPos;

    public void SetPlayer(Player player_) {
        player = player_;
    }
    
    public Vector3 GetCameraPosition() {
        Vector3 pos = cameraPos;

        pos.x += cameraHorizontalDeltaPos.x * Mathf.Cos((-cameraRot.y) * Mathf.Deg2Rad) -
                 cameraHorizontalDeltaPos.z * Mathf.Sin((-cameraRot.y) * Mathf.Deg2Rad);

        pos.z += cameraHorizontalDeltaPos.x * Mathf.Sin((-cameraRot.y) * Mathf.Deg2Rad) +
                 cameraHorizontalDeltaPos.z * Mathf.Cos((-cameraRot.y) * Mathf.Deg2Rad);

        pos += lastKnownPlayerPos - player.transform.position;
        
        return pos;
    }

    public Vector3 GetCameraEulerAngles() {
        return cameraRot;
    }

    private static float Angle(float a) {
        a = a % 360.0f;
        return a < 0.0f ? a + 360.0f : a;
    }

    public void LockCamera() {
        lockedCameraMode = true;

        CenterCamera();
    }

    public void UnlockCamera() {
        lockedCameraMode = false;
    }

    public void ControlCamera(float zoom, float dPosX, float dPosZ,
        float dRotH, float dRotV, float dt) {
        cameraDistanceToPlayer = Mathf.Clamp(cameraDistanceToPlayer * (1.0f - zoom / 10.0f), 8.0f, 40.0f);

        cameraRot.x = Mathf.Clamp(Angle(cameraRot.x + dRotV * rotVelocity * dt), 5.0f, 89.0f);
        cameraRot.y = Angle(cameraRot.y + dRotH * rotVelocity * dt);
        cameraRot.z = 0.0f;

        cameraPos.x = -cameraDistanceToPlayer * Mathf.Cos(cameraRot.x * Mathf.Deg2Rad) *
                      Mathf.Sin(cameraRot.y * Mathf.Deg2Rad);
        cameraPos.y = cameraDistanceToPlayer * Mathf.Sin(cameraRot.x * Mathf.Deg2Rad);
        cameraPos.z = -cameraDistanceToPlayer * Mathf.Cos(cameraRot.x * Mathf.Deg2Rad) *
                      Mathf.Cos(cameraRot.y * Mathf.Deg2Rad);

        if (lockedCameraMode) {
            CenterCamera();
        }
        else {
            cameraHorizontalDeltaPos.x += dPosX * dPosVelocity * dt;
            cameraHorizontalDeltaPos.z += dPosZ * dPosVelocity * dt;
        }
    }

    public void CenterCamera() {
        cameraHorizontalDeltaPos = new Vector3();
        lastKnownPlayerPos = player.transform.position;
    }

    public bool IsLockedCameraMode() {
        return lockedCameraMode;
    }

    public void ToggleLockedCamera() {
        if (lockedCameraMode) {
            UnlockCamera();
        }
        else {
            LockCamera();
        }
    }
}
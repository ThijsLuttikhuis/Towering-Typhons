using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour {
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Player player;

    private CameraRotator cameraRotator = new CameraRotator();
    
    private Vector3 cameraPos = default;
    private Vector3 cameraRot = default;

    private bool buttonRotateCameraLeft = false;
    private bool buttonRotateCameraRight = false;
    private bool buttonRotateCameraUp = false;
    private bool buttonRotateCameraDown = false;

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
        (cameraRot, cameraPos) =
            cameraRotator.RotateCamera(dZoomCamera, 0.0f, 0.0f, cameraRot, cameraPos, 0.0f);
    }

    private void Start() {
        Transform playerCameraTransform = playerCamera.transform;

        cameraPos = playerCameraTransform.localPosition;
        cameraRot = playerCameraTransform.eulerAngles;

        cameraRotator = new CameraRotator();
    }

    private void Update() {
        Transform playerCameraTransform = playerCamera.transform;

        playerCameraTransform.position = cameraPos + player.transform.position;
        playerCameraTransform.eulerAngles = cameraRot;
    }

    public void UpdatePlayerCamera(float dt) {
        float dRotCameraH = buttonRotateCameraLeft ? 1.0f : buttonRotateCameraRight ? -1.0f : 0.0f;
        float dRotCameraV = buttonRotateCameraUp ? 1.0f : buttonRotateCameraDown ? -1.0f : 0.0f;

        (cameraRot, cameraPos) =
            cameraRotator.RotateCamera(0.0f, dRotCameraH, dRotCameraV, cameraRot, cameraPos, dt);
    }

    private class CameraRotator {
        private const float rotVelocity = 150.0f;

        private float cameraDistanceToPlayer = 20.0f;

        private static float Angle(float a) {
            a = a % 360;
            return a < 0 ? a + 360 : a;
        }

        public (Vector3, Vector3) RotateCamera(float zoom, float dRotH, float dRotV, Vector3 rot, Vector3 pos, float dt) {
            cameraDistanceToPlayer = Mathf.Clamp(cameraDistanceToPlayer * (1.0f - zoom / 10.0f), 8.0f, 40.0f);

            rot.x = Mathf.Clamp(Angle(rot.x + dRotV * rotVelocity * dt), 5, 89);
            rot.y = Angle(rot.y + dRotH * rotVelocity * dt);
            rot.z = 0;

            pos.x = -cameraDistanceToPlayer * Mathf.Cos((rot.x) * Mathf.Deg2Rad) * Mathf.Sin(rot.y * Mathf.Deg2Rad);
            pos.y = cameraDistanceToPlayer * Mathf.Sin((rot.x) * Mathf.Deg2Rad);
            pos.z = -cameraDistanceToPlayer * Mathf.Cos((rot.x) * Mathf.Deg2Rad) * Mathf.Cos(rot.y * Mathf.Deg2Rad);

            return (rot, pos);
        }
    }
}





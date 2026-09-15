using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTarget;

    [Header("Camera Position")]
    [SerializeField] private float distance = 4.5f;
    [SerializeField] private float shoulderOffset = 0.5f;
    [SerializeField] private float verticalOffset = 0.2f;

    [Header("Mouse Orbit")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Player Rotation")]
    [SerializeField] private float playerRotationSpeed = 12f;

    [Header("Camera Smoothing")]
    [SerializeField] private float positionSmoothSpeed = 15f;
    [SerializeField] private float rotationSmoothSpeed = 20f;

    [Header("Default View")]
    [SerializeField] private float defaultPitch = 10f;

    private float yaw;
    private float pitch;

    private bool isDragging;
    private bool rotatePlayerAfterDrag;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError(
                "CameraController: Player chưa được gán!",
                this
            );

            return;
        }

        if (cameraTarget == null)
        {
            Debug.LogError(
                "CameraController: CameraTarget chưa được gán!",
                this
            );

            return;
        }

        // Bắt đầu camera theo hướng Player
        yaw = player.eulerAngles.y;
        pitch = defaultPitch;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void LateUpdate()
    {
        if (player == null || cameraTarget == null)
        {
            return;
        }

        HandlePlayerRotation();

        // Khi không kéo chuột,
        // camera tiếp tục bám hướng Player
        if (!isDragging && !rotatePlayerAfterDrag)
        {
            FollowPlayerDirection();
        }

        UpdateCamera();
    }

    private void HandleMouseInput()
    {
        if (Mouse.current == null)
        {
            return;
        }

        // =========================
        // BẮT ĐẦU KÉO CAMERA
        // =========================

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
            rotatePlayerAfterDrag = false;

            Cursor.lockState =
                CursorLockMode.Locked;

            Cursor.visible = false;
        }

        // =========================
        // ĐANG KÉO CAMERA
        // =========================

        if (isDragging)
        {
            Vector2 mouseDelta =
                Mouse.current.delta.ReadValue();

            yaw +=
                mouseDelta.x *
                mouseSensitivity;

            pitch -=
                mouseDelta.y *
                mouseSensitivity;

            pitch = Mathf.Clamp(
                pitch,
                minPitch,
                maxPitch
            );
        }

        // =========================
        // THẢ CHUỘT
        // =========================

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;

            // Không trả camera về hướng cũ.
            // Thay vào đó xoay Player sang hướng camera.
            rotatePlayerAfterDrag = true;

            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible = true;
        }
    }

    private void HandlePlayerRotation()
    {
        if (!rotatePlayerAfterDrag)
        {
            return;
        }

        float currentPlayerYaw =
            player.eulerAngles.y;

        float difference =
            Mathf.DeltaAngle(
                currentPlayerYaw,
                yaw
            );

        // Player đã gần đúng hướng camera
        if (Mathf.Abs(difference) < 0.5f)
        {
            Vector3 finalEuler =
                player.eulerAngles;

            finalEuler.y = yaw;

            player.rotation =
                Quaternion.Euler(finalEuler);

            rotatePlayerAfterDrag = false;

            return;
        }

        float newYaw =
            Mathf.LerpAngle(
                currentPlayerYaw,
                yaw,
                1f -
                Mathf.Exp(
                    -playerRotationSpeed *
                    Time.deltaTime
                )
            );

        Vector3 playerEuler =
            player.eulerAngles;

        playerEuler.y = newYaw;

        player.rotation =
            Quaternion.Euler(playerEuler);
    }

    private void FollowPlayerDirection()
    {
        // Camera theo hướng Player
        // nhưng không giật/snap.
        yaw =
            Mathf.LerpAngle(
                yaw,
                player.eulerAngles.y,
                1f -
                Mathf.Exp(
                    -15f *
                    Time.deltaTime
                )
            );

        // Trả pitch nhẹ về góc mặc định
        pitch =
            Mathf.Lerp(
                pitch,
                defaultPitch,
                1f -
                Mathf.Exp(
                    -5f *
                    Time.deltaTime
                )
            );
    }

    private void UpdateCamera()
    {
        Quaternion cameraRotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f
            );

        Vector3 focusPosition =
            cameraTarget.position +
            Vector3.up *
            verticalOffset;

        Vector3 backwardOffset =
            cameraRotation *
            new Vector3(
                0f,
                0f,
                -distance
            );

        Vector3 shoulder =
            cameraRotation *
            new Vector3(
                shoulderOffset,
                0f,
                0f
            );

        Vector3 desiredPosition =
            focusPosition +
            backwardOffset +
            shoulder;

        float positionLerp =
            1f -
            Mathf.Exp(
                -positionSmoothSpeed *
                Time.deltaTime
            );

        transform.position =
            Vector3.Lerp(
                transform.position,
                desiredPosition,
                positionLerp
            );

        Vector3 lookDirection =
            focusPosition -
            transform.position;

        if (lookDirection.sqrMagnitude <
            0.001f)
        {
            return;
        }

        Quaternion desiredRotation =
            Quaternion.LookRotation(
                lookDirection,
                Vector3.up
            );

        float rotationLerp =
            1f -
            Mathf.Exp(
                -rotationSmoothSpeed *
                Time.deltaTime
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                rotationLerp
            );
    }
}
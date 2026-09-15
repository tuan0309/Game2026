using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTarget;

    [Header("Camera Position")]
    [SerializeField] private float distance = 6f;
    [SerializeField] private float height = 2.5f;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    private void LateUpdate()
    {
        if (player == null || cameraTarget == null)
        {
            return;
        }

        // Camera luôn ở phía sau Player
        Vector3 desiredPosition =
            player.position
            - player.forward * distance
            + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        // Camera nhìn về phía trước của Player
        Vector3 lookDirection =
            cameraTarget.position - transform.position;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion desiredRotation =
                Quaternion.LookRotation(
                    lookDirection,
                    Vector3.up
                );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
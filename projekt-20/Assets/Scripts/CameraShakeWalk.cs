using UnityEngine;

public class CameraShakeWalk : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Shake Settings")]
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 10f;

    [Header("Movement Detection")]
    public float movementThreshold = 0.1f;

    private Vector3 originalPosition;
    private float shakeTimer;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        originalPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(moveX) > movementThreshold || Mathf.Abs(moveY) > movementThreshold;

        if (isMoving)
        {
            Shake();
        }
        else
        {
            ResetPosition();
        }
    }

    void Shake()
    {
        shakeTimer += Time.deltaTime * shakeSpeed;

        float offsetX = Mathf.Sin(shakeTimer) * shakeAmount;
        float offsetY = Mathf.Cos(shakeTimer * 2f) * shakeAmount;

        cameraTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
    }

    void ResetPosition()
    {
        shakeTimer = 0f;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originalPosition, Time.deltaTime * 10f);
    }
}

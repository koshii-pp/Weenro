// MODIFIED VER OF SLIPKNOT'S CAMERA CONTROLLER LMAO!
using UnityEngine;

public class OverworldCam : MonoBehaviour
{
    [Header("Following Settings")]
    [SerializeField] private float horizontalOffset = 0f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float lookAheadDistance = 3f;
    
    [Header("Zoom Settings")]
    [SerializeField] private float defaultZoom = 5f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float speedBasedZoomMultiplier = 0.5f;
    [SerializeField] private float boredZoom = 2.5f;

    [Header("Boundaries")]
    [SerializeField] private bool useBoundaries = true;
    [SerializeField] private float minX = float.MinValue;
    [SerializeField] private float maxX = float.MaxValue;
    [SerializeField] private float minY = float.MinValue;
    [SerializeField] private float maxY = float.MaxValue;

    private Transform target;
    private Camera cam;
    private float currentZoom;
    private float targetZoom;

    private void Start()
    {
        cam = GetComponent<Camera>();
        currentZoom = defaultZoom;
        targetZoom = defaultZoom;
        cam.orthographicSize = currentZoom;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            var playerController = FindObjectOfType<PlayerControllerTakeUno>();
            if (playerController != null)
            {
                target = playerController.transform;
            }
            else
            {
                return;
            }
        }

        PlayerControllerTakeUno player = target.GetComponent<PlayerControllerTakeUno>();
        float speed = Mathf.Abs(player.GetGroundSpeed());
        float speedZoom = defaultZoom + (speed * speedBasedZoomMultiplier);
        targetZoom = Mathf.Clamp(speedZoom, minZoom, maxZoom);

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        cam.orthographicSize = currentZoom;

        Vector3 targetPosition = new Vector3(
            target.position.x + horizontalOffset,
            target.position.y + verticalOffset,
            transform.position.z
        );

        if (useBoundaries)
        {
            float halfHeight = currentZoom;
            float halfWidth = halfHeight * cam.aspect;

            targetPosition.x = Mathf.Clamp(targetPosition.x, minX + halfWidth, maxX - halfWidth);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY + halfHeight, maxY - halfHeight);
        }

        transform.position = targetPosition;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetBoundaries(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        useBoundaries = true;
    }
}
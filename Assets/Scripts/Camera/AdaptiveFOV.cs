using UnityEngine;

public class AdaptiveFOV : MonoBehaviour
{
    [SerializeField] private BoxCollider adaptiveFOVArea;
    [SerializeField] private float targetFov = 45f;
    [SerializeField] private float minFov = 20f;
    [SerializeField] private float maxFov = 80f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 50f;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (adaptiveFOVArea == null)
        {
            return;
        }

        if (cam == null)
        {
            cam = GetComponent<Camera>();
            if (cam == null)
            {
                return;
            }
        }

        FitBounds(adaptiveFOVArea.bounds);
    }

    private void FitBounds(Bounds bounds)
    {
        Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        for (int i = 0; i < 8; i++)
        {
            Vector3 corner = new Vector3(
                center.x + extents.x * ((i & 1) == 0 ? -1f : 1f),
                center.y + extents.y * ((i & 2) == 0 ? -1f : 1f),
                center.z + extents.z * ((i & 4) == 0 ? -1f : 1f));

            Vector3 local = cam.transform.InverseTransformPoint(corner);
            min = Vector3.Min(min, local);
            max = Vector3.Max(max, local);
        }

        Vector3 halfExtents = (max - min) * 0.5f;
        float halfDepth = Mathf.Abs(halfExtents.z);
        float maxAbsX = Mathf.Abs(halfExtents.x);
        float maxAbsY = Mathf.Abs(halfExtents.y);

        float clampedTargetFov = Mathf.Clamp(targetFov, minFov, maxFov);
        float vFovRad = clampedTargetFov * Mathf.Deg2Rad;
        float hFovRad = 2f * Mathf.Atan(Mathf.Tan(vFovRad * 0.5f) * cam.aspect);

        float requiredDistance = halfDepth + Mathf.Max(
            maxAbsY / Mathf.Tan(vFovRad * 0.5f),
            maxAbsX / Mathf.Tan(hFovRad * 0.5f));

        float distance = Mathf.Clamp(requiredDistance, minDistance, maxDistance);
        float finalFov = clampedTargetFov;

        if (!Mathf.Approximately(distance, requiredDistance))
        {
            float effectiveZ = Mathf.Max(distance - halfDepth, 0.0001f);
            float requiredV = 2f * Mathf.Atan(maxAbsY / effectiveZ);
            float requiredH = 2f * Mathf.Atan(maxAbsX / effectiveZ);
            float requiredVFromH = 2f * Mathf.Atan(Mathf.Tan(requiredH * 0.5f) / cam.aspect);
            finalFov = Mathf.Clamp(Mathf.Max(requiredV, requiredVFromH) * Mathf.Rad2Deg, minFov, maxFov);
        }

        cam.fieldOfView = finalFov;
        cam.transform.position = bounds.center - cam.transform.forward.normalized * distance;
    }
}

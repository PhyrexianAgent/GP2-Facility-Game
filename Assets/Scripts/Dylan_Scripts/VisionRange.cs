using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRange : MonoBehaviour
{
    [Header("Circle Settings")]
    public float radius = 5f;
    public float angle = 90f;
    public float startingAngle, endingAngle;
    public float directionAngle = 0f;

    private void OnDrawGizmos()
    {
        if (angle < 0f || angle > 360f)
        {
            Debug.LogWarning("Angle must be between 0 and 360 degrees.");
            return;
        }

        startingAngle = directionAngle + angle / 2f;
        endingAngle = directionAngle - angle / 2f;

        Gizmos.color = Color.green;

        int segments = 100;
        float angleStep = (endingAngle - startingAngle) / segments;
        Vector3 previousPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startingAngle + i * angleStep;
            float rad = Mathf.Deg2Rad * currentAngle;

            Vector3 point = new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius) + transform.position;

            if (i > 0) { Gizmos.DrawLine(previousPoint, point); }

            previousPoint = point;
        }

        // Draw Lines
        float startRad = Mathf.Deg2Rad * startingAngle;
        float endRad = Mathf.Deg2Rad * endingAngle;

        Vector3 startPoint = new Vector3(Mathf.Cos(startRad) * radius, 0, Mathf.Sin(startRad) * radius) + transform.position;
        Vector3 endPoint = new Vector3(Mathf.Cos(endRad) * radius, 0, Mathf.Sin(endRad) * radius) + transform.position;

        Gizmos.DrawLine(transform.position, startPoint);
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position, endPoint);
    }
}

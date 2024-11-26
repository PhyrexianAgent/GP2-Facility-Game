using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetector : MonoBehaviour
{
    [SerializeField, Min(0)] protected float lookRadius, lookDistance;
    [SerializeField] private Color coneColor = Color.blue; // Does not matter for gameplay as will only change how 'cone' is seen in editor.
    // Start is called before the first frame update

    protected void OnDrawGizmos() => DrawSpotlight(lookRadius, lookDistance, coneColor);
    protected void DrawSpotlight(float radius, float length, Color color){
        Vector3 forward = transform.position + transform.forward * length;
        Vector3 left = forward - transform.right * radius;
        Vector3 right = forward + transform.right * radius;
        Vector3 up = forward + transform.up * radius;
        Vector3 down = forward - transform.up * radius;

        Debug.DrawLine(transform.position, left, color, 0);
        Debug.DrawLine(transform.position, right, color, 0);
        Debug.DrawLine(transform.position, up, color, 0);
        Debug.DrawLine(transform.position, down, color, 0);

        Debug.DrawLine(left, up, color, 0);
        Debug.DrawLine(up, right, color, 0);
        Debug.DrawLine(right, down, color, 0);
        Debug.DrawLine(down, left, color, 0);
    }

    public bool PlayerInSpotlight(Transform playerTransform) => PlayerInSight(lookDistance, lookRadius, playerTransform);

    protected bool PlayerInSight(float lookDistance, float lookRadius, Transform playerTransform){ // Similar to what we did in class but with a cone so such a cone can be made easily visible
        Vector3 forward = transform.position + transform.forward * lookDistance;
        float coneDist = Vector3.Dot(playerTransform.transform.position - transform.position, transform.forward);

        if (coneDist < 0 || coneDist > lookDistance)
            return false;

        Vector3 coneDistPoint = transform.position + transform.forward * coneDist;
        float coneRadius = (coneDist / lookDistance) * lookRadius;

        return Vector3.Distance(coneDistPoint, playerTransform.transform.position) <= coneRadius;
    }
}

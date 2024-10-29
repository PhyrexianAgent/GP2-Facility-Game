using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPane : MonoBehaviour // Is the class used to see if player is hidden by cover, but will say not if player is only partially hidden
{
    [SerializeField] private float width, height;
    [SerializeField] private float gizmosDistanceFromObject; // Only matters for easier viewing in editor and meaningless when game runs.

    public bool PaneVisibleToPoint(Vector3 point){ // Will shoot 4 raycasts to point, with returning true if any contact nothing
        transform.LookAt(point);
        Vector3[] corners = {
            transform.position - transform.right * width / 2 + transform.up * height / 2,
            transform.position - transform.right * width / 2 - transform.up * height / 2,
            transform.position + transform.right * width / 2 + transform.up * height / 2,
            transform.position + transform.right * width / 2 - transform.up * height / 2
        };
        foreach (Vector3 corner in corners){
            if (!Physics.Linecast(point, corner)){
                Debug.DrawLine(point, corner, Color.blue, 0); // So you can see the rays still reaching the bot
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos(){
        Vector3 frontPos = transform.forward * gizmosDistanceFromObject + transform.position;
        Vector3 leftTopCorner = frontPos - transform.right * width / 2 + transform.up * height / 2;
        Vector3 leftBottomCorner = frontPos - transform.right * width / 2 - transform.up * height / 2;
        Vector3 rightTopCorner = frontPos + transform.right * width / 2 + transform.up * height / 2;
        Vector3 rightBottomCorner = frontPos + transform.right * width / 2 - transform.up * height / 2;

        Debug.DrawLine(leftTopCorner, leftBottomCorner, Color.blue, 0);
        Debug.DrawLine(rightTopCorner, rightBottomCorner, Color.blue, 0);
        Debug.DrawLine(leftTopCorner, rightTopCorner, Color.blue, 0);
        Debug.DrawLine(leftBottomCorner, rightBottomCorner, Color.blue, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//[CustomEditor(typeof(TurretCamera))]
#if UNITY_EDITOR
public class TurretCameraEditor : Editor
{
    private bool togglePointButtons;
    private bool justToggled = false;
    private Quaternion rotation;
    public override void OnInspectorGUI(){
        TurretCamera script = (TurretCamera)target;
        Transform[] points = script.GetLookPoints();
        DrawDefaultInspector();
        togglePointButtons = GUILayout.Toggle(togglePointButtons, "Show Rotate To Point Buttons");
        if (togglePointButtons){
            if (!justToggled){
                justToggled = true;
                rotation = script.transform.rotation;
            }
            for (int i=0; i<points.Length; i++){
                if (GUILayout.Button($"Look to Point {i+1}")) script.RotateToPoint(points[i].position);
            }
        }
        else{
            justToggled = false;
            script.transform.rotation = rotation;
        }
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretPatrolState : State
{
    private readonly GameObject _turret;
    private GameObject _turretBase;
    private readonly float _angleLimit = 30f;
    private float _baseVelocity = 0f;

    private readonly float _speed = 1f;

    private VisionRange _range;

    private float currentAngle = 0f;

    public TurretPatrolState(GameObject turret, VisionRange range, float speed) : base(turret)
    {
        _turret = turret;
        _turretBase = FindDescendant(_turret.transform, "Turret_BaseRotation");

        _speed = speed;

        _range = range;
        _angleLimit = _range.angle / 1.7f;
    }

    public override void Update()
    {
        //Debug.Log("Patrolling");
        currentAngle = Mathf.Sin(Time.time * _speed) * _angleLimit;
        currentAngle = Mathf.SmoothDampAngle(_turretBase.transform.rotation.eulerAngles.y, 90f + -_range.directionAngle + currentAngle, ref _baseVelocity, 1f / _speed); // To make for a smooth transition

        // Apply the rotation around the z-axis (for 2D) or y-axis (for 3D)
        if (_turretBase != null )
        {
            _turretBase.transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
        }
        // 
    }

    GameObject FindDescendant(Transform parent, string target)
    {
        // Check if current descendant matches
        if (parent.gameObject.name == target)
        {
            return parent.gameObject;
        }

        // Search through all descendants
        foreach (Transform child in parent)
        {
            GameObject found = FindDescendant(child, target);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}

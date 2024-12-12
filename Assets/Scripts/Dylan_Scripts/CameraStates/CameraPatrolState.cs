using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraPatrolState : State
{
    private readonly GameObject[] _turrets;
    private GameObject _cameraBase;
    private float _baseVelocity = 0f;
    private Vector2[] _turretBaseVelocity;

    private readonly GameObject _player;
    private readonly float _angleLimit = 30f;
    private readonly float _speed = 1f;

    private VisionRange _cameraRange;

    private float currentAngle = 0f;

    public CameraPatrolState(GameObject[] turrets, GameObject player, GameObject camera, float angle, float speed) : base(camera)
    {

        _turrets = turrets;
        _turretBaseVelocity = new Vector2[turrets.Length];
        for (int i = 0; i < _turrets.Length - 1; i++)
        {
            _turretBaseVelocity[i] = new Vector2(0f, 0f);
        }

        _cameraBase = FindDescendant(camera.transform, "Camera_BaseRotation");
        _cameraRange = camera.GetComponentInChildren<VisionRange>();

        _player = player;
        _angleLimit = angle;
        _speed = speed;

        _angleLimit = _cameraRange.angle / 1.7f;
    }

    public override void OnEnter()
    {
        //Debug.Log("...Entering Patrol Mode");
        //agent.speed = 4f;
        //animator.Play(Run);


    }

    public override void Update()
    {
        //Debug.Log("Patrolling");

        // Update Camera
        currentAngle = Mathf.Sin(Time.time * _speed) * _angleLimit;
        currentAngle = Mathf.SmoothDampAngle(_cameraBase.transform.rotation.eulerAngles.y, 90f + -_cameraRange.directionAngle + currentAngle, ref _baseVelocity, 1f / _speed); // To make for a smooth transition

        // Apply the rotation around the z-axis (for 2D) or y-axis (for 3D)
        if (_cameraBase != null)
        {
            _cameraBase.transform.rotation = Quaternion.Euler(30f, currentAngle, 0f);
        }

        // Update Turrets
        for (int i = 0; i < _turrets.Length; i++)
        {
            GameObject turret = _turrets[i];

            GameObject turretNeck = FindDescendant(turret.transform, "Turret_Neck");
            GameObject turretBase = FindDescendant(turret.transform, "Turret_BaseRotation");
            GameObject turretIdleTarget = FindDescendant(turret.transform, "Turret_IdleTarget");

            Vector3 directionToIdle = turretIdleTarget.transform.position - turretNeck.transform.position;

            float baseAngle = Mathf.SmoothDampAngle(turretBase.transform.rotation.eulerAngles.y, -90f, ref _turretBaseVelocity[i].x, 1f / _speed); // To make for a smooth transition

            float neckAngle = Mathf.Atan2(-directionToIdle.y, directionToIdle.magnitude) * Mathf.Rad2Deg;
            float neckOffset = Mathf.Sin(Time.time * Mathf.PI * _speed) * 3f;
            float neckTargetAngle = neckAngle + neckOffset;
            neckAngle = Mathf.SmoothDampAngle(turretNeck.transform.rotation.eulerAngles.z, -neckTargetAngle, ref _turretBaseVelocity[i].y, 1f / _speed); // To make for a smooth transition

            turretBase.transform.rotation = Quaternion.Euler(0f, baseAngle, 0f);
            turretNeck.transform.rotation = Quaternion.Euler(0f, baseAngle, neckAngle);
        }
        // 
    }

    public override void OnExit()
    {
        //Debug.Log("Leaving patrol...");
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

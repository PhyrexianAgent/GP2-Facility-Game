using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class CameraAlertState : State
{
    private readonly GameObject[] _turrets;
    private GameObject _cameraBase, _cameraSensor;
    private Vector2 _baseVelocity = new Vector2(0f, 0f);
    private Vector2[] _turretBaseVelocity;

    private readonly float _speed = 3f;

    private VisionRange _cameraRange;

    private readonly float _damage = 1f;

    private float _refreshRate = 5f / 60f;
    private float _refreshTimer = 0f;

    public CameraAlertState(GameObject[] turrets, GameObject camera, float speed, float damage) : base(camera)
    {
        _turrets = turrets;
        _turretBaseVelocity = new Vector2[turrets.Length];
        for (int i = 0; i < _turrets.Length - 1; i++)
        {
            _turretBaseVelocity[i] = new Vector2(0f, 0f);
        }

        _cameraBase = FindDescendant(camera.transform, "Camera_BaseRotation");
        _cameraSensor = FindDescendant(camera.transform, "CameraSensor");
        _cameraRange = camera.GetComponentInChildren<VisionRange>();

        _speed = speed;
        _damage = damage;
    }

    public override void OnEnter()
    {
        //Debug.Log("...Entering Attack Mode");
        //agent.speed = 4f;
        //animator.Play(Run);

        foreach (GameObject turret in _turrets)
        {
            GameObject particleEmitter = FindDescendant(turret.transform, "Shooting_ParticleSystem");
            particleEmitter.SetActive(true);

            AudioSource audioSource = turret.GetComponent<AudioSource>();

            if (!audioSource.isPlaying)
            {
                audioSource.time = audioSource.clip.length * 0.15f;
                audioSource.Play();
            }
        }
    }

    public override void Update()
    {
        //Debug.Log("Attacking Player");

        if (GameManager.GetPlayerTransform() == null) return; // Ignore Update if Player hasn't been found yet

        _refreshTimer += Time.deltaTime;
        if (_refreshTimer > _refreshRate)
        {
            _refreshTimer = 0f;
            CheckForPlayerHit();
        }

        TrackTarget();

    }

    public void TrackTarget()
    {
        Vector3 cameraDirectionToTarget = GameManager.GetPlayerTransform().position - _cameraBase.transform.position;

        float turnAngle = Mathf.Atan2(cameraDirectionToTarget.x, cameraDirectionToTarget.z) * Mathf.Rad2Deg;
        float turnOffset = Mathf.Sin(Time.time * Mathf.PI * _speed) * 3f;
        float turnTargetAngle = turnAngle + turnOffset;
        turnAngle = Mathf.SmoothDampAngle(_cameraBase.transform.rotation.eulerAngles.y, turnTargetAngle, ref _baseVelocity.x, 1f / _speed); // To make for a smooth transition

        float leanAngle = Mathf.Atan2(cameraDirectionToTarget.y, cameraDirectionToTarget.magnitude) * Mathf.Rad2Deg;
        float leanOffset = Mathf.Sin(Time.time * Mathf.PI * _speed) * 3f;
        float leanTargetAngle = (leanAngle + leanOffset) * 1.345f;
        leanAngle = Mathf.SmoothDampAngle(_cameraBase.transform.rotation.eulerAngles.x, -leanTargetAngle, ref _baseVelocity.y, 1f / _speed); // To make for a smooth transition

        turnAngle = 90f + CheckBounds(turnAngle, _cameraRange, -90f);

        _cameraBase.transform.rotation = Quaternion.Euler(leanAngle, turnAngle, 0f);

        for (int i = 0; i < _turrets.Length; i++)
        {
            GameObject turret = _turrets[i];

            GameObject turretBase = FindDescendant(turret.transform, "Turret_BaseRotation");
            GameObject turretNeck = FindDescendant(turret.transform, "Turret_Neck");
            GameObject turretSensor = FindDescendant(turret.transform, "Turret_Sensor");
            VisionRange turretVision = _turrets[i].GetComponentInChildren<VisionRange>();

            Vector3 directionToTarget = GameManager.GetPlayerTransform().position - turretSensor.transform.position;

            float baseAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            float baseOffset = Mathf.Sin(Time.time * Mathf.PI * _speed) * 3f;
            float baseTargetAngle = baseAngle + baseOffset - 90f;
            baseAngle = Mathf.SmoothDampAngle(turretBase.transform.rotation.eulerAngles.y, baseTargetAngle, ref _turretBaseVelocity[i].x, 1f / _speed); // To make for a smooth transition

            float neckAngle = Mathf.Atan2(-directionToTarget.y, directionToTarget.magnitude) * Mathf.Rad2Deg;
            float neckOffset = Mathf.Sin(Time.time * Mathf.PI * _speed) * 3f;
            float neckTargetAngle = neckAngle + neckOffset;
            neckAngle = Mathf.SmoothDampAngle(turretNeck.transform.rotation.eulerAngles.z, -neckTargetAngle, ref _turretBaseVelocity[i].y, 1f / _speed); // To make for a smooth transition

            baseAngle = CheckBounds(baseAngle, turretVision, 0f);

            turretBase.transform.rotation = Quaternion.Euler(0f, baseAngle, 0f);
            turretNeck.transform.rotation = Quaternion.Euler(0f, baseAngle, neckAngle);
        }
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180.1f) { angle -= 360f; }
        else if (angle < -180.1f) { angle += 360f; }
        return angle;
    }

    private float CheckBounds(float angle, VisionRange rawRange, float initialOffset)
    {
        float normalizedAngle = NormalizeAngle(initialOffset + angle);
        float startingAngle = NormalizeAngle(-rawRange.startingAngle);
        float endingAngle = NormalizeAngle(-rawRange.endingAngle);

        float range = Mathf.Abs(-rawRange.startingAngle - -rawRange.endingAngle);
        //int option = 0;

        if (startingAngle + range > 180) // If it is greater than start or less than end
        {
            //option = 1;
            if (normalizedAngle < startingAngle && normalizedAngle >= startingAngle - ((360f - range) / 2f)) { normalizedAngle = startingAngle; }
            if (normalizedAngle > endingAngle && normalizedAngle < endingAngle + ((360f - range) / 2f)) { normalizedAngle = endingAngle; }
        }
        else if (startingAngle + range < 180) // If it is less than start or greater than end
        {
            //option = 2;
            if (startingAngle > 0 && endingAngle > 0) // Both Positve
            {
                //option = 3;
                if (normalizedAngle < startingAngle && normalizedAngle >= NormalizeAngle(startingAngle - ((360f - range) / 2f))) { normalizedAngle = startingAngle; }
                if (normalizedAngle > endingAngle || normalizedAngle < NormalizeAngle(endingAngle + ((360f - range) / 2f))) { normalizedAngle = endingAngle; }
            }
            else if (startingAngle < 0 && endingAngle < 0) // Both Negative
            {
                //option = 4;
                if (normalizedAngle < startingAngle || normalizedAngle >= NormalizeAngle(startingAngle - ((360f - range) / 2f))) { normalizedAngle = startingAngle; }
                if (normalizedAngle > endingAngle && normalizedAngle < NormalizeAngle(endingAngle + ((360f - range) / 2f))) { normalizedAngle = endingAngle; }
            }
            else // start is negative and end is positive
            {
                //option = 5;
                if (normalizedAngle < startingAngle) { normalizedAngle = startingAngle; }
                if (normalizedAngle > endingAngle) { normalizedAngle = endingAngle; }
            }

        }
        //Debug.Log($"Current Angle: {normalizedAngle}, bounds: start|{startingAngle}| end|{endingAngle}| ... option: {option}, fullRange: abs({startingAngle} - {endingAngle}) = {range}");
        
        return normalizedAngle;
    }

    public void CheckForPlayerHit()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Wall") | LayerMask.GetMask("Player");

        for (int i = 0; i < _turrets.Length; i++)
        {
            GameObject turretSensor = FindDescendant(_turrets[i].transform, "Turret_Sensor");

            //Debug.DrawLine(turretSensor.transform.position, turretSensor.transform.position + turretSensor.transform.right * 10f, Color.red, 1f);
            if (Physics.Linecast(turretSensor.transform.position, turretSensor.transform.position + turretSensor.transform.right * 10f, out hit, layerMask))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall")) { return; }

                if (hit.collider.gameObject == GameManager.GetPlayerTransform().gameObject)
                {
                    Debug.Log("Deal " + _damage + " Damage!");
                }
            }
        }
    }

    public override void OnExit()
    {
        //Debug.Log("Leaving attack...");

        foreach (GameObject turret in _turrets)
        {
            GameObject particleEmitter = FindDescendant(turret.transform, "Shooting_ParticleSystem");
            particleEmitter.SetActive(false);

            AudioSource audioSource = turret.GetComponent<AudioSource>();

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
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

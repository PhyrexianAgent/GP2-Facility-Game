using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class TurretAttackState : State
{
    private readonly GameObject _turret;
    private GameObject _turretBase, _turretArm, _turretSensor, _particleEmitter;
    private Vector2 _baseVelocity = new Vector2(0f, 0f);
    private AudioSource _audioSource;

    private readonly Vector3 _angleLimit = new Vector3();
    private readonly float _speed = 3f;

    private VisionRange _range;

    private readonly float _damage = 1f;

    private float _refreshRate = 5f/60f;
    private float _refreshTimer = 0f;

    public TurretAttackState(GameObject turret, VisionRange range, float speed, AudioSource audioSource, float damage) : base(new GameObject())
    {
        _turret = turret;
        _turretBase = FindDescendant(_turret.transform, "Turret_BaseRotation");
        _turretArm = FindDescendant(_turret.transform, "Turret_Arm");
        _turretSensor = FindDescendant(_turret.transform, "Turret_Sensor");
        _particleEmitter = FindDescendant(_turret.transform, "Shooting_ParticleSystem");

        _range = range;
        _turretBase.transform.rotation = Quaternion.Euler(0f, _range.directionAngle + 90f, 0f);

        _angleLimit = new Vector3(_range.startingAngle, 0f, _range.endingAngle);
        _speed = speed;
        _audioSource = audioSource;
        _damage = damage;
    }

    public override void OnEnter()
    {
        //Debug.Log("...Entering Attack Mode");
        //agent.speed = 4f;
        //animator.Play(Run);

        _particleEmitter.SetActive(true);

        if (!_audioSource.isPlaying)
        {
            _audioSource.time = _audioSource.clip.length * 0.15f;
            _audioSource.Play();
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
        Vector3 directionToTarget = GameManager.GetPlayerTransform().position - _turretSensor.transform.position;

        float baseAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        float baseOffset = Mathf.Sin(Time.time * Mathf.PI * _speed) * 3f;
        float baseTargetAngle = baseAngle + baseOffset;
        baseAngle = Mathf.SmoothDampAngle(_turretBase.transform.rotation.eulerAngles.y, baseTargetAngle, ref _baseVelocity.x, 1f / _speed); // To make for a smooth transition

        float armAngle = Mathf.Atan2(-directionToTarget.y, directionToTarget.magnitude) * Mathf.Rad2Deg;
        float armOffset = Mathf.Sin(Time.time * Mathf.PI * _speed) * 3f;
        float armTargetAngle = armAngle + armOffset;
        armAngle = Mathf.SmoothDampAngle(_turretArm.transform.rotation.eulerAngles.x, armTargetAngle, ref _baseVelocity.y, 1f / _speed); // To make for a smooth transition

        baseAngle = 90f + CheckBounds(baseAngle);

        _turretBase.transform.rotation = Quaternion.Euler(0f, baseAngle, 0f);
        _turretArm.transform.rotation = Quaternion.Euler(armAngle, baseAngle, 0f);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180.1f) { angle -= 360f; }
        else if (angle < -180.1f) { angle += 360f; }
        return angle;
    }

    private float CheckBounds(float angle)
    {
        float normalizedAngle = NormalizeAngle(-90f + angle);
        float startingAngle = NormalizeAngle(-_range.startingAngle);
        float endingAngle = NormalizeAngle(-_range.endingAngle);

        float range = Mathf.Abs(-_range.startingAngle - -_range.endingAngle);
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

    public void CheckForPlayerHit() // You could just shoot raycast out (as you can specify distance limit) and see what tag collider has (easier way I think)
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Wall") | LayerMask.GetMask("Player");

        if (Physics.Linecast(_turretSensor.transform.position, _turretSensor.transform.position + _turretSensor.transform.forward * 10f, out hit, layerMask))
        {

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall")) { return; }

            if (hit.collider.gameObject == GameManager.GetPlayerTransform().gameObject)
            {
                Debug.Log("Deal " + _damage + " Damage!");
            }
        }
    }

    public override void OnExit()
    {
        //Debug.Log("Leaving attack...");

        _particleEmitter.SetActive(false);

        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurretShootingController : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private float speedToShoot = 0;
    [SerializeField] private GameObject turretGun;
    [SerializeField] private Transform barrelPoint;
    private TurretDamageController damageController;
    void Awake()
    {
        damageController = GetComponent<TurretDamageController>();
    }
    void Update()
    {
        turretGun.transform.Rotate(new Vector3(0, 0, 1) * spinSpeed * Time.deltaTime);
        damageController.SetDamageActive(spinSpeed >= speedToShoot && GameManager.PlayerInView(barrelPoint.position, barrelPoint.forward));
    }
}

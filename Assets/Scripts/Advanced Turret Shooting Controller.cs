using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurretShootingController : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private float speedToShoot = 1500;
    [SerializeField] private GameObject turretGun;
    private TurretDamageController damageController;
    void Awake()
    {
        damageController = GetComponent<TurretDamageController>();
    }
    void Update()
    {
        turretGun.transform.Rotate(new Vector3(0, 0, 1) * spinSpeed * Time.deltaTime);
        damageController.SetDamageActive(spinSpeed >= speedToShoot && GameManager.PlayerInView(turretGun.transform.position));
    }
}

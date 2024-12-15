using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTurretShootingController : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private GameObject turretGun;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turretGun.transform.Rotate(new Vector3(0, 0, 1) * spinSpeed * Time.deltaTime);
    }
}

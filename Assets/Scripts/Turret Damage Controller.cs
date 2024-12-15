using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDamageController : MonoBehaviour
{
    [SerializeField] private GameObject particleController;
    [SerializeField, Min(0)] private float damagePerProjectile;
    private Coroutine damageDelay;
    private ParticleSystem particleSystem;
    void Awake()
    {
        particleSystem = particleController.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleController.activeSelf && damageDelay == null) damageDelay = StartCoroutine(DelayPlayerDamage());
    }
    private IEnumerator DelayPlayerDamage(){
        while true{
            if (!particleController.activeSelf) break;
            GameManager.MakePlayerTakeDamage(damagePerProjectile);
            yield return new WaitForSeconds();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDamageController : MonoBehaviour
{
    [SerializeField] private GameObject particleController;
    [SerializeField, Min(0)] private float damagePerProjectile;
    private Coroutine damageDelay;
    private ParticleSystem particleSystem;
    private bool dealDamage = false;
    void Awake()
    {
        particleSystem = particleController.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private IEnumerator DelayPlayerDamage(){
        Debug.Log("started damage");
        while (true){
            if (dealDamage) GameManager.MakePlayerTakeDamage(damagePerProjectile);
            yield return new WaitForSeconds(particleSystem.duration);
        }
        Debug.Log("stopped damage");
    }
    private IEnumerator DelayCoroutineTest(){
        yield return null;
        if (dealDamage && damageDelay == null) damageDelay = StartCoroutine(DelayPlayerDamage());
        if (!dealDamage && damageDelay != null){
            StopCoroutine(damageDelay);
            damageDelay = null;
        } 
    }
    public void SetDamageActive(bool dealDamage){
        this.dealDamage = dealDamage;
        particleController.SetActive(dealDamage);
        StartCoroutine(DelayCoroutineTest());
    }
}

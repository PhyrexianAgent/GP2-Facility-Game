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
    }
    private IEnumerator DelayPlayerDamage(){
        Debug.Log("started damage");
        while (true){
            GameManager.MakePlayerTakeDamage(damagePerProjectile);
            yield return new WaitForSeconds(particleSystem.duration);
        }
        Debug.Log("stopped damage");
    }
    public void SetDamageActive(bool dealDamage){
        if (dealDamage != particleController.activeSelf){
            if (dealDamage && damageDelay == null) 
                damageDelay = StartCoroutine(DelayPlayerDamage());
            else if (!dealDamage && damageDelay != null)
                StopCoroutine(damageDelay);
        }
        particleController.SetActive(dealDamage);
    }
}

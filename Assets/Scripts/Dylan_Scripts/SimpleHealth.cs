using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    //public float visualHealth = 60f;
    public float regenRate = 0.1f;
    [SerializeField, Min(0)] private float regenDelay = 0.3f;
    private bool died = false;
    private Coroutine regenWait;
    private float health;
    private bool canRegenHealth = true;
    private CanvasGroup healthCanvasGroup;

    private void Start()
    {
        health = maxHealth;
        healthCanvasGroup = GetComponent<CanvasGroup>();
        GameManager.SetPlayerHealth(this);
        maxHealth = health;
    }

    private void Update()
    {
        if (died) 
            healthCanvasGroup.alpha = 1;
        else{
            if (health < maxHealth && canRegenHealth){
                health += regenRate * Time.deltaTime;
            }
            if (health > maxHealth) health = maxHealth;
            healthCanvasGroup.alpha = 1 - (health / maxHealth);
        }
    }
    private IEnumerator DelayRegen(){
        canRegenHealth = false;
        yield return new WaitForSeconds(regenDelay);
        canRegenHealth = true;
    }
    private void RefreshRegenDelay(){
        if (regenWait != null) StopCoroutine(regenWait);
        regenWait = StartCoroutine(DelayRegen());
    }

    public void DealDamage(float damage)
    {
        if (!died){
            health -= damage;
            RefreshRegenDelay();
            died = health <= 0;
            if (died) GameManager.KillPlayer();
        }
    }

}

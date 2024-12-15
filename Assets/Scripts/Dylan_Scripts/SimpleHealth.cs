using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleHealth : MonoBehaviour
{

    public float health = 100f;
    private float maxHealth;
    public float visualHealth = 60f;
    public float regenRate = 0.1f;
    [SerializeField, Min(0)] private float regenDelay = 0.3f;
    private float regenTimer = 0f;
    private bool died = false;
    private Coroutine regenWait;

    private CanvasGroup healthCanvasGroup;

    private void Start()
    {
        healthCanvasGroup = GetComponent<CanvasGroup>();
        GameManager.SetPlayerHealth(this);
        maxHealth = health;
        visualHealth = (maxHealth / 100) * visualHealth;
    }

    private void Update()
    {
        if (died) 
            healthCanvasGroup.alpha = 1;
        else{
            if (health < maxHealth && regenWait == null) health += regenRate;
            healthCanvasGroup.alpha = 1 - (health / visualHealth);
        }
    }
    private IEnumerator DelayRegen(){
        yield return new WaitForSeconds(regenDelay);
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
        }
    }

}

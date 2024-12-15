using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleHealth : MonoBehaviour
{

    public float health = 100f;
    private float maxHealth;
    public float visualHealth = 60f; // 0 - 100
    public float regenRate = 0.1f;
    private float regenTimer = 0f;

    public Image healthVisual;

    private void Start()
    {
        GameManager.SetPlayerHealth(this);
        maxHealth = health;
        visualHealth = (maxHealth / 100) * visualHealth;
        //health = 1;
    }

    private void Update()
    {
        if (regenTimer < 0.3f)
        {
            regenTimer += Time.deltaTime;
        }
        else
        {
            float alpha = ((150 / 100) * (1 - (health / visualHealth)));
            //Debug.Log(alpha);
            healthVisual.color = new Color(healthVisual.color.r, healthVisual.color.g, healthVisual.color.b, alpha);
            if (health < maxHealth)
            {
                health += regenRate;
            }
            regenTimer = 0f;
        }
    }

    public void DealDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health < visualHealth)
            {
                if (health < 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float maxHealth = 100f;
    public int money = 0;
    public float currentHealth;
    public float healthRegen = 1f;

    public Vector3 playerPos;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        currentHealth += healthRegen * Time.deltaTime;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if(currentHealth < 0)
        {
            currentHealth = 0;        
        }

        HealthBar.instance?.SetHealth(currentHealth);
    }

    public void ChangeHealth(int health)
    {
        currentHealth += health;
        HealthBar.instance.SetHealth(currentHealth);
    }

    public void ChangeMoney(int sum)
    {
        money += sum;
        MoneyUI.instance.SetMoney(money);
    }
}

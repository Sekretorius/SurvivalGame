using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float maxHealth = 100f;
    public float maxMana = 100f;
    public int money = 0;
    public float currentHealth;
    public float healthRegen = 1f;
    public float currentMana;
    public float manaRegen = 2f;

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
        currentMana = maxMana;
    }

    void Update()
    {
        currentHealth += healthRegen * Time.deltaTime;
        currentMana += manaRegen * Time.deltaTime;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if(currentHealth < 0)
        {
            currentHealth = 0;        
        }

        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        if(currentMana < 0)
        {
            currentMana = 0;        
        }

        HealthBar.instance?.SetHealth(currentHealth);
        ManaBar.instance?.SetMana(currentMana);
    }

    public void ChangeHealth(int health)
    {
        currentHealth += health;
        HealthBar.instance.SetHealth(currentHealth);
    }

    public void ChangeMana(int mana)
    {
        currentMana += mana;
        ManaBar.instance.SetMana(currentMana);
    }

    public void ChangeMoney(int sum)
    {
        money += sum;
        MoneyUI.instance.SetMoney(money);
    }
}

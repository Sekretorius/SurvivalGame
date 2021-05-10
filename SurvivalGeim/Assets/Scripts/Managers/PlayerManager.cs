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
    public float currentMana;
    public float healthRegen = 1f;
    public float manaRegen = 2f;
    public GameObject[] projectiles;
    public GameObject[] skills;

    private float currentHealthRegen;
    private float currentManaRegen;
    public bool isAlive = true;
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
        currentMana = maxMana;
        currentHealthRegen = healthRegen;
        currentManaRegen = manaRegen;
    }

    void Update()
    {
        //playerPos = PlayerController.instance.playerPos;

        currentHealth += currentHealthRegen * Time.deltaTime;
        currentMana += currentManaRegen * Time.deltaTime;

        currentHealth = currentHealth < 0 ? 0 : currentHealth > maxHealth ? maxHealth : currentHealth;
        currentMana = currentMana < 0 ? 0 : currentMana > maxMana ? maxMana : currentMana;

        HealthBar.instance?.SetHealth(currentHealth);
        ManaBar.instance?.SetMana(currentMana);
    }

    public void ChangeHealth(int health)
    {
        if (!isAlive)
            return;

        if(health < 0) StartCoroutine(HeroIcon.instance?.Injured());
        currentHealth += health;

        HealthBar.instance.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            isAlive = false;
            PauseMenu.instance.ShowDeathScreen();
        }
    }

    public void ChangeMana(int mana)
    {
        currentMana += mana;
        ManaBar.instance?.SetMana(currentMana);
    }

    public void ChangeMoney(int sum)
    {
        money += sum;
        money = money < 0 ? 0 : money;
        MoneyUI.instance.SetMoney(money);
    }

    public void SetHealthRegen(float regen)
    {
        currentHealthRegen = regen;
    }

    public void SetManaRegen(float regen)
    {
        currentManaRegen = regen;
    }
}

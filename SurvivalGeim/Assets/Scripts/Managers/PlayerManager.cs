using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int maxHealth = 100;
    public int money = 0;
    public int currentHealth;

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthBar.instance.SetHealth(currentHealth);
    }

    public void ChangeMoney(int sum)
    {
        money += sum;
        MoneyUI.instance.SetMoney(money);
    }
}

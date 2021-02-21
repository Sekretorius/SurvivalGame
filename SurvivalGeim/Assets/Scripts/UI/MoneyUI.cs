using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI instance;
    public Text moneyText;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        SetMoney(PlayerManager.instance.money);
    }

    public void SetMoney(int money) 
    {
        moneyText.text = money.ToString();
    }
}

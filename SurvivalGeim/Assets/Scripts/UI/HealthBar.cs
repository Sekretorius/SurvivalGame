using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
  public static HealthBar instance;

  public Slider slider;
  public Gradient gradient;
  public Image fill;
  public Text hpText;

  void Awake()
  {
    if (instance == null)
      instance = this;
    else
      Destroy(this);
  }

  void Start()
  {
    SetMaxHealth(PlayerManager.instance.maxHealth);
    SetHealth(PlayerManager.instance.currentHealth);
  }

  public void SetMaxHealth(float health) 
  {
    slider.maxValue = health;
    slider.value = health;
    hpText.text = health + " / " + health;
    fill.color = gradient.Evaluate(1f);
  }
  public void SetHealth(float health) 
  {
    slider.value = health;

    fill.color = gradient.Evaluate(slider.normalizedValue);
    if (hpText != null)
    {
        hpText.text = ((int)health).ToString() + " / " + slider.maxValue;
    }
  }
}

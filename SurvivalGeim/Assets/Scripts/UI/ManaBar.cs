using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManaBar : MonoBehaviour
{
  public static ManaBar instance;

  public Slider slider;
  public Image fill;
  public Text manaText;

  void Awake()
  {
    if (instance == null)
      instance = this;
    else
      Destroy(this);
  }

  void Start()
  {
    SetMaxMana(PlayerManager.instance.maxMana);
    SetMana(PlayerManager.instance.currentMana);
  }

  public void SetMaxMana(float mana) 
  {
    slider.maxValue = mana;
    slider.value = mana;
    manaText.text = mana + " / " + mana;
  }
  public void SetMana(float mana) 
  {
    slider.value = mana;

    manaText.text = ((int)mana).ToString() + " / " + slider.maxValue;
  }
}
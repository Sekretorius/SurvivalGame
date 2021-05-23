using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegen : MonoBehaviour, Skill
{
    public KeyCode keyCode;
    public float manaRegenMultiplier = 10f;
    public float duration = 10;
    public float cooldown = 10;
    public Sprite sprite;

    private float myTime = 0;
    private bool active = false;

    public bool isOnCooldown { get { return myTime > 0; } }

    public bool isActive { get { return active; } }

    Sprite Skill.barSprite 
    { 
        get { return sprite; }
        set { sprite = value; } 
    }

    KeyCode Skill.keyCode 
    { 
        get => keyCode; 
        set => keyCode = value; 
    }

    public void onInvoke()
    {
        myTime -= Time.deltaTime;
        if (myTime <= 0)
        {
            if (Input.GetKeyDown(keyCode) && !active)
            {
                StartCoroutine(SetManaRegen());                
            }
        }
    }

    private IEnumerator SetManaRegen()
    {
        active = true;
        PlayerManager.instance.SetManaRegen(PlayerManager.instance.manaRegen * manaRegenMultiplier);
        yield return new WaitForSeconds(duration);
        PlayerManager.instance.SetManaRegen(PlayerManager.instance.manaRegen / manaRegenMultiplier);
        active = false;
        myTime = cooldown;
    }
}

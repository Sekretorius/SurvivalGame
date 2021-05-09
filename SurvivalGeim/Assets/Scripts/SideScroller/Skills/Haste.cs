using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : MonoBehaviour, Skill
{
    public KeyCode keyCode;
    public float heroSpeedMultiplier = 3f;
    public float jumpSpeedMultiplier = 1.5f;
    public float duration = 5;
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
            if (Input.GetKeyDown(keyCode))
            {
                StartCoroutine(SetManaRegen());
            }
        }
    }

    private IEnumerator SetManaRegen()
    {
        active = true;
        PlayerController.instance.moveSpeed *= heroSpeedMultiplier;
        PlayerController.instance.jumpSpeed *= jumpSpeedMultiplier;
        yield return new WaitForSeconds(duration);
        PlayerController.instance.moveSpeed /= heroSpeedMultiplier;
        PlayerController.instance.jumpSpeed /= jumpSpeedMultiplier;
        active = false;
        myTime = cooldown;
    }
}

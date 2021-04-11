using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRegen : MonoBehaviour, Skill
{
    public KeyCode keyCode;
    public float manaRegenMultiplier = 10f;
    public float duration = 10;
    public float cooldown = 10;

    private float myTime = 0;

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
        PlayerManager.instance.SetManaRegen(PlayerManager.instance.manaRegen * manaRegenMultiplier);
        yield return new WaitForSeconds(duration);
        PlayerManager.instance.SetManaRegen(PlayerManager.instance.manaRegen);
        myTime = cooldown;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePosition;
    public GameObject projectile;
    public float fireRate = 0.2F;
    public float manaRegenMultiplier = 10f;
    public float maxCharge = 3f;
    public float maxScale = 4f;
    public int baseManaCost = 5;

    private float nextFire = 0.2F;
    private float myTime = 0.0F;

    // Update is called once per frame
    void Update()
    {
        myTime = myTime + Time.deltaTime;

        if (Input.GetButtonUp("Fire1") && PlayerManager.instance.currentMana >= baseManaCost)
        {
            PlayerManager.instance.ChangeMana(-baseManaCost);
            var proj = Instantiate(projectile, firePosition.position, firePosition.rotation);
            if(myTime / maxCharge * maxScale > 1)
            {
                proj.transform.localScale *= myTime > maxCharge ? maxScale : myTime / maxCharge * maxScale;
            }
            
            myTime = 0.0F;
        }

        if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && PlayerManager.instance.currentMana >= baseManaCost && myTime > nextFire)
        {
            PlayerManager.instance.SetManahRegen(PlayerManager.instance.manaRegen);
            PlayerManager.instance.ChangeMana(-baseManaCost);
            Instantiate(projectile, firePosition.position, firePosition.rotation);

            myTime = 0.0F;            
        }
        if(Input.GetButtonDown("Fire2") && !Input.GetButton("Fire1"))
        {
            PlayerManager.instance.SetManahRegen(PlayerManager.instance.manaRegen * manaRegenMultiplier);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            PlayerManager.instance.SetManahRegen(PlayerManager.instance.manaRegen);
        }

    }
}

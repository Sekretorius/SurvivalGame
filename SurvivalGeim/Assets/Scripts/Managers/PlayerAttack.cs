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

    private float myTime = 0.0F;
    private int baseManaCost;

    private void Start()
    {
        baseManaCost = projectile.GetComponent<Projectile>().manaCost;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1")) myTime = myTime + Time.deltaTime;

        if (Input.GetButtonUp("Fire1") && PlayerManager.instance.currentMana >= baseManaCost)
        {
            PlayerManager.instance.ChangeMana(-baseManaCost);
            createProjectile();           
            
            myTime = 0.0F;
        }

        if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && PlayerManager.instance.currentMana >= baseManaCost && myTime > fireRate)
        {
            PlayerManager.instance.SetManahRegen(PlayerManager.instance.manaRegen);
            PlayerManager.instance.ChangeMana(-baseManaCost);
            createProjectile();

            myTime = 0.0F;            
        }
        if(Input.GetButton("Fire2") && !Input.GetButton("Fire1"))
        {
            PlayerManager.instance.SetManahRegen(PlayerManager.instance.manaRegen * manaRegenMultiplier);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            PlayerManager.instance.SetManahRegen(PlayerManager.instance.manaRegen);
        }

    }

    private GameObject createProjectile()
    {
        var proj = Instantiate(projectile, firePosition.position, firePosition.rotation);
        var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - proj.transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (myTime > fireRate)
        {
            var multip = myTime > maxCharge ? (maxScale - 1) : myTime / maxCharge * (maxScale - 1);
            proj.transform.localScale += proj.transform.localScale * multip;
            proj.GetComponent<Projectile>().damage *= multip;
        }

        return proj;
    }
}

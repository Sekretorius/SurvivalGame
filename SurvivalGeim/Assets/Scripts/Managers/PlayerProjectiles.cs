using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    public Transform firePosition;
    public GameObject[] projectiles;
    public float fireRate = 0.2F;
    public float maxCharge = 3f;
    public float maxScale = 4f;

    private float myTime;
    private float chargeTime;
    private List<Projectile> projs = new List<Projectile>();

    private void Start()
    {
        for(int i = 0; i < projectiles.Length; i++)
            projs.Add(projectiles[i].GetComponent<Projectile>());
        myTime = 0.0f;
        chargeTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < projs.Count; i++)
        {
            checkForInput(projs[i], i);
        }
    }

    private void checkForInput(Projectile projectile, int index)
    {
        myTime += Time.deltaTime;
        if (projectile.chargeable && Input.GetKey(projectile.keyCode)) chargeTime += Time.deltaTime;

        if (Input.GetKeyUp(projectile.keyCode) && PlayerManager.instance.currentMana >= projectile.manaCost && myTime >= fireRate)
        {
            PlayerManager.instance.ChangeMana(-projectile.manaCost);
            createProjectile(index);            
        }        
    }

    private GameObject createProjectile(int index)
    {
        var position = firePosition.position;
        position.y += projectiles[index].GetComponent<SpriteRenderer>().bounds.size.y / 4;
        position.x += projectiles[index].GetComponent<SpriteRenderer>().bounds.size.x / 8;

        var proj = Instantiate(projectiles[index], position, firePosition.rotation);
        var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - proj.transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (projs[index].rotatable) proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        else if (Mathf.Abs(angle) > 90)
        {
            proj.transform.localScale *= -1;
            proj.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        }

        if (projs[index].chargeable && chargeTime > fireRate)
        {
            var multip = chargeTime > maxCharge ? (maxScale - 1) : chargeTime / maxCharge * (maxScale - 1);
            proj.transform.localScale += proj.transform.localScale * multip;
            proj.GetComponent<Projectile>().damage *= multip;
            proj.GetComponent<Projectile>().knockback *= multip;
        }

        myTime = 0.0f;
        chargeTime = 0.0f;
        return proj;
    }
}

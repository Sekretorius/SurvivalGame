using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private List<Enemy> enemies;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

    }

    private void Start()
    {
        enemies = new List<Enemy>();
        foreach(Enemy e in transform.GetComponentsInChildren<Enemy>())
        {
            enemies.Add(e);
        }
    }

    public void BlockEnemyMovement()
    {
        foreach (Enemy e in enemies)
        {
            e.block = true;
            e.animator.StopPlayback();
        }
    }

    public void UnblockEnemtMovement()
    {
        foreach (Enemy e in enemies)
        {
            e.block = false;
            e.animator.StartPlayback();
        }
    }

    


}

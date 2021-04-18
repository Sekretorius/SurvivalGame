using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private GameObject[] skills;

    private void Start()
    {
        skills = PlayerManager.instance.skills;        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].GetComponent<Skill>().onInvoke();
        }
    }
}

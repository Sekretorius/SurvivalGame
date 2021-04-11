using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public GameObject[] skills;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].GetComponent<Skill>().onInvoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public static PlayerSkills Instance { get; private set; }
    private List<GameObject> skills;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        skills = PlayerManager.instance.skills;
    }
    public void AddSkill(GameObject skill)
    {
        skills.Add(skill);
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].GetComponent<Skill>().onInvoke();
        }
    }
}

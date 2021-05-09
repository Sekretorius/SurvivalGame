using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject skill;

    private bool isBlinkinking = false;
    // Update is called once per frame
    void Update()
    {
        if(skill.GetComponent<Skill>().isOnCooldown)
            gameObject.transform.Find("Background").Find("Cover").gameObject.SetActive(true);
        if (!skill.GetComponent<Skill>().isOnCooldown)
        {
            gameObject.transform.Find("Background").Find("Cover").gameObject.SetActive(false);
            if (skill.GetComponent<Skill>().isActive && !isBlinkinking)
                StartCoroutine(Blink(0.4f, gameObject.transform.Find("Background").Find("ActiveCover").gameObject));
        }
        

    }
    private IEnumerator Blink(float time, GameObject obj)
    {
        isBlinkinking = true;
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        yield return new WaitForSeconds(time);
        isBlinkinking = false;
    }
}

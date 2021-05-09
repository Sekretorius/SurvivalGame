using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsBar : MonoBehaviour
{
    public Transform slotTemplate;

    // Start is called before the first frame update
    void Start()
    {
        var skills = PlayerManager.instance.skills;
        for (int i = 0; i < skills.Length; i++)
        {
            var skill = skills[i].GetComponent<Skill>();
            var sprite = skill.barSprite;
            var key = skill.keyCode.ToString();
            var keyText = key.Substring(key.Length - 1);

            Transform skillSlotTransform = Instantiate(slotTemplate, transform);
            skillSlotTransform.gameObject.SetActive(true);
            skillSlotTransform.GetComponent<SkillSlot>().skill = skills[i];

            var projSlotRectTransform = skillSlotTransform.GetComponent<RectTransform>();
            projSlotRectTransform.anchoredPosition = new Vector2(100f * i, 0f);
            projSlotRectTransform.Find("Background").Find("Image").GetComponent<Image>().sprite = sprite;
            projSlotRectTransform.Find("Background").Find("ButtonKey").GetComponent<TMPro.TextMeshProUGUI>().text = keyText;
        }

    }
}

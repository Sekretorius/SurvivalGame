using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectilesBar : MonoBehaviour
{
    public static ProjectilesBar Instance { get; private set; }
    public Transform slotTemplate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        Init();
    }

    public void Init()
    {
        var projectiles = PlayerManager.instance.projectiles;
        for (int i = 0; i < projectiles.Count; i++)
        {
            var proj = projectiles[i].GetComponent<Projectile>();
            var sprite = proj.barSprite;
            var keyCode = proj.keyCode;
            var keyText = keyCode.ToString().Substring(keyCode.ToString().Length - 1);

            Transform projSlotTransform = Instantiate(slotTemplate, transform);
            projSlotTransform.GetComponent<ProjectileSlot>().keyCode = keyCode;
            projSlotTransform.gameObject.SetActive(true);
            var projSlotRectTransform = projSlotTransform.GetComponent<RectTransform>();
            projSlotRectTransform.anchoredPosition = new Vector2(100f * i, 0f);
            projSlotRectTransform.Find("Background").Find("Image").GetComponent<Image>().sprite = sprite;
            projSlotRectTransform.Find("Background").Find("ButtonKey").GetComponent<TMPro.TextMeshProUGUI>().text = keyText;
        }
    }
}

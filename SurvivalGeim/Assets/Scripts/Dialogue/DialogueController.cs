using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InventorySystem;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject Inventory;

    [SerializeField]
    public CanvasGroup UI;

    [SerializeField]
    public TextMeshProUGUI dialogueName;

    [SerializeField]
    public TextMeshProUGUI dialogueTextBox;

    [SerializeField]
    public Image playerFace;

    [SerializeField]
    public Image npcFace;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        if (!Inventory && InventoryManager.Instance)
            Inventory = InventoryManager.Instance.gameObject;
    }

    public void Enable()
    {
        if (Inventory) Inventory.SetActive(false);
        if (UI) UI.alpha = 0;

        background.SetActive(true);
    }

    public void Disable()
    {
        background.SetActive(false);

        if (Inventory) Inventory.SetActive(true);
        if (UI) UI.alpha = 1;
    }

    public void SetName(string name)
    {
        dialogueName.text = name;
    }

    public void SetText(string text)
    {
        dialogueTextBox.text = text;
    }

    public void SetNpcImage(Sprite img)
    {
        npcFace.sprite = img;
    }

    public void SetImageVisibility(bool speaker)
    {
        if (speaker) 
        {
            playerFace.color = new Color(playerFace.color.r, playerFace.color.g, playerFace.color.b, 1f);
            if(npcFace.sprite)
                npcFace.color = new Color(playerFace.color.r, playerFace.color.g, playerFace.color.b, 0.2f);
        }
        else
        {
            playerFace.color = new Color(playerFace.color.r, playerFace.color.g, playerFace.color.b, 0.2f);
            if (npcFace.sprite)
                npcFace.color = new Color(playerFace.color.r, playerFace.color.g, playerFace.color.b, 1f);
        }
    }

}

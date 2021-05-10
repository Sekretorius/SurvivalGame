using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject Inventory;

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

    public void Enable()
    {
        background.SetActive(true);
        Inventory.SetActive(false);
    }

    public void Disable()
    {
        background.SetActive(false);
        Inventory.SetActive(true);
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
            npcFace.color = new Color(playerFace.color.r, playerFace.color.g, playerFace.color.b, 0.2f);
        }
        else
        {
            playerFace.color = new Color(playerFace.color.r, playerFace.color.g, playerFace.color.b, 0.2f);
            npcFace.color = new Color(playerFace.color.r, playerFace.color.g, playerFace.color.b, 1f);
        }
    }

}

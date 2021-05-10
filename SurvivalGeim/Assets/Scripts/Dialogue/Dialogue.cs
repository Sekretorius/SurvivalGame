using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private List<MessageLines> dialogueList;

    private Queue<Message> dialogue;

    private Message current;

    [SerializeField]
    public bool isPlayerFirst = false;

    [SerializeField]
    public string playerName = "Nose man";

    [SerializeField]
    public string NpcName = "???";

    [SerializeField]
    public Sprite npcFace;

    public bool isSpeaking = false;

    private bool turn;

    private void Init()
    {
        dialogue = new Queue<Message>();

        for (int i = 0; i < dialogueList.Count; i++)
        {
            Message temp = new Message();
            temp.Init(dialogueList[i].lines);
            dialogue.Enqueue(temp);
        }
    }

    public void StartDialogue()
    {
        Init();
        if (dialogue.Count > 0)
        {
            turn = isPlayerFirst;

            if (isPlayerFirst)
                SetName(playerName);
            else
                SetName(NpcName);

            if(npcFace)
                DialogueController.instance.SetNpcImage(npcFace);

            current = dialogue.Dequeue();
            isSpeaking = true;
            ShowText();
        }
        else
            Debug.Log("No dialogue");
    }

    private void SetName(string name)
    {
        DialogueController.instance.SetImageVisibility(turn);
        DialogueController.instance.SetName(name);
    }

    private void ChangeTurn()
    {
        turn = !turn;
        if (turn)
            SetName(playerName);
        else
            SetName(NpcName);

        current = dialogue.Dequeue();
        Next();
    }


    public void Next()
    {
        if (dialogue.Count <= 0 && current.isFinished)
            EndDialogue();
        else if (!current.isFinished)
            ShowText();
        else
            ChangeTurn();
    }

    public void EndDialogue()
    {
        Debug.Log("Ended");
        isSpeaking = false;
        DialogueController.instance.Disable();
    }

    private void ShowText()
    {
        DialogueController.instance.SetText(current.Next());
    }

    [System.Serializable]
    public class MessageLines
    {
        public List<string> lines;
    }
}

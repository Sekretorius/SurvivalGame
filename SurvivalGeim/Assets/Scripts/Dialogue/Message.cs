using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message
{
    public int index;

    public bool isFinished;

    public Queue<string> message;

    public void Init(List<string> lines)
    {
        index = 0;
        isFinished = false;

        message = new Queue<string>();

        foreach (string l in lines)
            message.Enqueue(l);
    }
    public string Next()
    {
        if (message.Count - 1 <= 0)
            isFinished = true;

        return message.Dequeue();
    }
}

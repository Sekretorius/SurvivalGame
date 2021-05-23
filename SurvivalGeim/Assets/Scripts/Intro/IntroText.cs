using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public float delay = 0.1f;
    public AudioSource source;

    Coroutine coroutine;

    [SerializeField]
    private List<string> introTxt;

    private Queue<string> monologue;
    private string current;

    void Start()
    {
        monologue = new Queue<string>();
        for (int i = 0; i < introTxt.Count; i++)
            monologue.Enqueue(introTxt[i]);

        //coroutine = StartCoroutine(TypeAnim(monologue.Dequeue()));
    }

    public void Next()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            textField.text = "[ " + current + " ]";
        }

        if (monologue.Count > 0)
            coroutine = StartCoroutine(TypeAnim(monologue.Dequeue()));
        else
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    IEnumerator TypeAnim(string text)
    {

        int count = 0;

        Dictionary<int,int> waiters = new Dictionary<int,int>();

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '[')
            {
                waiters.Add(i,Convert.ToInt32(text[i + 1].ToString()));
                Debug.Log(Convert.ToInt32(text[i + 1].ToString()));
                text = text.Remove(i, 3);
                continue;
            }       
        }

        current = text;

        for (int i = 0; i < text.Length; i++)
        {
            textField.text = "[ " + text.Substring(0,i) + " ]";
            if (text[i] != ' ' && count++ == 0)
            {
                count = 0;
                source.Play();
            }
            if (waiters.ContainsKey(i))
            {
                Debug.Log("Waiting for - " + waiters[i]);
                yield return new WaitForSeconds(waiters[i]);
            }
            yield return new WaitForSeconds(delay);
        }
        textField.text = "[ " + text + " ]";
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private float value;

    [SerializeField]
    [TextArea]
    private string textOnPick = "";

    [SerializeField]
    private TextMeshProUGUI textField;

    [SerializeField]
    private float textDuration = 3;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private bool isPicked = false;
    private void Awake()
    {
        textField.gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPicked)
        {
            return;
        }
        if (collision.collider.name.Equals("Player"))
        {
            isPicked = true;
            Debug.Log(textOnPick);
            textField.gameObject.SetActive(true);
            textField.text = textOnPick;
            spriteRenderer.enabled = false;
            StartCoroutine(Timmer());
        }
    }

    private IEnumerator Timmer()
    {
        float time = 0;
        while(time < textDuration)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public RectTransform circle;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        //StartCoroutine(Transition(""));
    }

    public void ChangeScene(string scene)
    {
        StartCoroutine(Transition(scene));
    }

    private IEnumerator Transition(string scene)
    {
        int transitionSpeed = 30;

        while (circle.sizeDelta.x >= 0)
        {
            circle.sizeDelta = new Vector2(circle.sizeDelta.x - transitionSpeed, circle.sizeDelta.y - transitionSpeed);
            yield return null;
        }

        // yield return new WaitForSeconds(2);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);

        //while (asyncLoad.isDone == false)
        //{
        //    float progress = Mathf.Clamp01(asyncLoad.progress / .9f);

        //    circle.sizeDelta = new Vector2(3000 * progress, 3000 * progress);

        //    yield return null;

        //}

        while (circle.sizeDelta.x < 3000)
        {
            circle.sizeDelta = new Vector2(circle.sizeDelta.x + 15, circle.sizeDelta.y + 15);
            yield return null;
        }

    }

}

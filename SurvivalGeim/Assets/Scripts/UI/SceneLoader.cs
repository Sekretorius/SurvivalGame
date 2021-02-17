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
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
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
        int transitionSpeed = 3000 / 50;

        if (TopDownPlayerController.Instance != null)
        {
            circle.transform.position = Camera.main.WorldToScreenPoint(TopDownPlayerController.Instance.transform.position);
            // Other controller movement stop
        }
        else
        {
            circle.transform.position = Camera.main.WorldToScreenPoint(PlayerController.instance.transform.position);
            PlayerController.instance.Block();
        }

        while (circle.sizeDelta.x >= 0)
        {
            circle.sizeDelta = new Vector2(circle.sizeDelta.x - transitionSpeed, circle.sizeDelta.y - transitionSpeed);
            yield return new WaitForFixedUpdate();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);

        while (asyncLoad.isDone == false)
            yield return null;

        CutoutMaskUI.instance.SetMaterial(); // Refresh mat
        CameraFollow.instance.RefreshPosition(); // camera pos
        PlayerController.instance.Block(); // block movement

        if (TopDownPlayerController.Instance != null)
            circle.transform.position = Camera.main.WorldToScreenPoint(TopDownPlayerController.Instance.transform.position);
        else
            circle.transform.position = Camera.main.WorldToScreenPoint(PlayerController.instance.transform.position);

        while (circle.sizeDelta.x < 3000)
        {
            circle.sizeDelta = new Vector2(circle.sizeDelta.x + transitionSpeed, circle.sizeDelta.y + transitionSpeed);
            yield return new WaitForFixedUpdate();
        }

        PlayerController.instance.Unblock();
        // Other controller movement unstop

    }

}

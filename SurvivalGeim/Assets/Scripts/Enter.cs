using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enter : MonoBehaviour
{
    public static List<string> sceneList;
    [ListToPopup(typeof(Enter), "sceneList")]
    public string scene;

    public bool exit;

    public bool canExit = true;

    public bool showIntro = false;

    [DraggablePoint] public Vector3 SpawnPosition;

    void Awake()
    {
        CreateNumberList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canExit && collision.tag == "Player")
        {
            if (exit)
            {
                EnterManager.instance.Exit();
                return;
            }

            SceneLoader.instance.ChangeScene(scene);

            // for some reason

            //SceneManager.LoadSceneAsync(sceneList.IndexOf(scene), LoadSceneMode.Single);
            EnterManager.instance.playerPos = SpawnPosition;
        }
    }

    [ContextMenu("CreateNumberList")]
    public void CreateNumberList()
    {
        sceneList = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            sceneList.Add(NameFromIndex(i));
    }

    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }


}

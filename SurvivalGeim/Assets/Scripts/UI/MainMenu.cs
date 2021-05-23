using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

  private void Start()
  {
      Application.targetFrameRate = 300;
  }
  public void PlayGame() 
  {
    if(PlayerManager.instance)
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    else
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }
  
  public void QuitGame() 
  {
    Debug.Log("Exit!");
    Application.Quit();
  }
}

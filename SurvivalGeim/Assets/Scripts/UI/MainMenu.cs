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
    SceneManager.LoadScene("Main", LoadSceneMode.Single);
  }
  
  public void QuitGame() 
  {
    Debug.Log("Exit!");
    Application.Quit();
  }
}

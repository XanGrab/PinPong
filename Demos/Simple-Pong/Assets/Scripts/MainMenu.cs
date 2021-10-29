using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene("Arena");
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Pretend we quit");
    }
}

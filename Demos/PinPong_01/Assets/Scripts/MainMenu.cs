using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start(){
        FindObjectOfType<AudioManager>().Play("MenuTheme");
    }

    public void PlayGame(){
        AudioManager am = FindObjectOfType<AudioManager>();
        int currTime = am.GetSoundTime("MenuTheme");
        am.Stop("MenuTheme");
        am.Play("ArenaTheme", currTime);
        SceneManager.LoadScene("Arena");
    }

    public void QuitGame(){
        Application.Quit();
    }
}

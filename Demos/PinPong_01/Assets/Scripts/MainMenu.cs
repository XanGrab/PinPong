using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Settings settings;
    private void Awake() {
        settings = FindObjectOfType<Settings>();

        settings.SetQuality(3);
        Resolution[] resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++){
            if(resolutions[i].Equals(Screen.currentResolution)){
                currentResolutionIndex = i;
            }
        }
        settings.SetResolution(currentResolutionIndex);
    }

    void Start(){
        FindObjectOfType<AudioManager>().Play("MenuTheme");
    }

    public void PlayGame(){
        AudioManager am = FindObjectOfType<AudioManager>();
        int currTime = am.GetSoundTime("MenuTheme");
        am.Stop("MenuTheme");
        am.Play("ArenaTheme");
        SceneManager.LoadScene("Arena");
    }

    public void QuitGame(){
        Application.Quit();
    }
}

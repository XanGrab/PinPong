using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Settings settings;
    AudioManager audioManager;
    void Awake() {
        settings = FindObjectOfType<Settings>();
        if(settings == null){
            Debug.LogError("null settings");
            return;
        }
        settings.GetComponent<Canvas>().enabled = false;

        audioManager = FindObjectOfType<AudioManager>();
        if(audioManager == null){
            Debug.LogError("null audio manager");
            return;
        }

        QualitySettings.SetQualityLevel(3);
        Resolution[] resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++){
            if(resolutions[i].Equals(Screen.currentResolution)){
                currentResolutionIndex = i;
            }
        }
        Resolution resolution = resolutions[currentResolutionIndex];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void Start(){
        FindObjectOfType<AudioManager>().Play("MenuTheme");
    }

    public void PlayGame(){
        AudioManager am = FindObjectOfType<AudioManager>();
        int currTime = am.GetSoundTime("MenuTheme");
        am.Stop("MenuTheme");
        am.Play("ArenaTheme");
        //test
        SceneManager.LoadScene("Arena");
    }

    public void EnableOptions(){
        settings.GetComponent<Canvas>().enabled = true;
    }
    public void DisableOptions(){
        settings.GetComponent<Canvas>().enabled = false;
    }

    public void ButtonSound(){
        audioManager.Play("ButtonPress");
    }
}

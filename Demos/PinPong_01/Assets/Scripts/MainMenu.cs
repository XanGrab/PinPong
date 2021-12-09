using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Settings settings;
    void Awake() {
        settings = FindObjectOfType<Settings>();
        if(settings == null){
            Debug.LogError("null settings");
            return;
        }
        settings.gameObject.SetActive(false);

        QualitySettings.SetQualityLevel(3);
        Resolution[] resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++){
            if(resolutions[i].Equals(Screen.currentResolution)){
                currentResolutionIndex = i;
            }
        }
        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
        settings.gameObject.SetActive(true);
        SceneManager.LoadScene("Arena");
    }

    public void EnableOptions(){
        settings.gameObject.SetActive(true);
    }
    public void DisableOptions(){
        settings.gameObject.SetActive(false);
    }

    public void QuitGame(){
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public float musicVol;
    public float SFXVol;
    public int qualityVal;

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Awake(){
        resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>(resolutions.Length);
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if(!resolutionOptions.Contains(option)){
                resolutionOptions.Add(option);
            }

            if(resolutions[i].Equals(Screen.currentResolution)){
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        SetMusicVolume(musicVol);
        SetSFXVolume(SFXVol);
        SetQuality(qualityVal);
    }

    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume(float volume){
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetResolution(int index){
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }
}

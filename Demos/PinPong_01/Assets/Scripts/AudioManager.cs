using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    void Start(){}

    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if(s == null){
            Debug.LogWarning("Sount: " + name + " not found.");
            return;
        }        
        Debug.Log("Sound: " + s.name);
        s.source.Play();
    }
    public void Play(string name, int time){
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if(s == null){
            Debug.LogWarning("Sount: " + s.name + " not found.");
            return;
        }        
        s.source.timeSamples = time;
        s.source.Play();
    }

    public void Stop(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if(s == null){
            Debug.LogWarning("Sount: " + s.name + " not found.");
            return;
        }        
        s.source.Stop();
    }

    public int GetSoundTime(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if(s == null){
            Debug.LogWarning("Sount: " + s.name + " not found.");
            return -1;
        }        

        return s.source.timeSamples;
    }

    public void Quiet(string name, bool active){
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if(s == null){
            Debug.LogWarning("Sount: " + s.name + " not found.");
            return;
        }        

        if(active){
            s.source.volume *= 0.4f;
        }else{
            s.source.volume *= 1/0.4f;
        }
    }
}

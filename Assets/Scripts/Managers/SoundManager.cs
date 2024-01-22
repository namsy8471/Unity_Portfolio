using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> soundFile;
    
    private void Awake()
    {
        soundFile = new Dictionary<string, AudioClip>();
        
        AddWalkSound();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void AddWalkSound()
    {
        for (int i = 1; i <= 10; i++)
        {
            soundFile.Add("Walk" + i, Resources.Load<AudioClip>("Sound/S_Stone_Mono_" + i));
            
            bool result = soundFile.TryGetValue("Walk"+ i, out AudioClip sound);
            if (!result)
            {
                Debug.Log("Walk" + i + " Key Sound File is not Found!");
                return;
            }
        }

        foreach (var key in soundFile.Keys)
        {
            Debug.Log(key + " is added in Audio Dictionary!");
        }
    }
    
    public void PlaySound(string keyWord)
    {
        bool result = soundFile[keyWord];
        if (!result)
        {
            Debug.Log(keyWord + " Key Sound File is not Found!");
            return;
        }
        else
        {
            audioSource.clip = soundFile[keyWord];
        }
        
        // Debug.Log(keyWord + " Sound is playing!");
        audioSource.Play();
    }
}

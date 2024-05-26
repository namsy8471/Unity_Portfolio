using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class SoundManager
{
    private Dictionary<string, AudioClip> _soundFiles;
    private Dictionary<string, int> _count;
    public void Init()
    {
        _soundFiles = new Dictionary<string, AudioClip>();
        _count = new Dictionary<string, int>();
        
        AddSound();
    }

    private void AddSound()
    {
        for (int i = 1; i <= 10; i++) AddSoundFile("Walk/Walk");
        AddSoundFile("Sword/Sword");
        AddSoundFile("Punch/Punch");
        AddSoundFile("BGM/BGM");

        #region Enemy
        AddSoundFile("Enemy/Bear/BearRoaring");
        AddSoundFile("Enemy/Bear/BearDead");
        AddSoundFile("Enemy/Bear/BearGetHit");
        #endregion
    }

    private AudioClip AddSoundFile(string path)
    {
        int index = path.LastIndexOf('/');
        string newPath = path.Substring(index + 1);
        
        if (_soundFiles.ContainsKey(newPath)) return _soundFiles[path];
        if (_count.ContainsKey(newPath) == false) _count[newPath] = 1;
        
        _soundFiles.Add(newPath + _count[newPath], Resources.Load<AudioClip>("Sound/" + path + _count[newPath]));
        _soundFiles.TryGetValue(newPath + _count[newPath], out AudioClip sound);
        
        if (sound == null)
        {
            Debug.Log(newPath + _count[newPath] + " Key Sound File is not Found!");
            return sound;
        }
        
        Debug.Log(newPath + _count[newPath] + " is added in Audio Dictionary!");
        _count[newPath]++;
        
        return sound;
    }
    
    public void PlaySound(GameObject obj,string keyWord)
    {
        int index = Random.Range(1, _count[keyWord]);
        
        _soundFiles.TryGetValue(keyWord + index, out AudioClip clip);
        
        if (clip == null)
        {
            Debug.Log(keyWord + index +" Key Sound File is not Found!");
            return;
        }

        AudioSource audioSource = obj.GetOrAddComponent<AudioSource>();
        audioSource.clip = clip;
        
        audioSource.Play();
    }
}

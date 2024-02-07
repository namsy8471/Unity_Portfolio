using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundReceiver : MonoBehaviour
{
    private void Start() => gameObject.GetOrAddComponent<AudioSource>();
    
    // 사운드 재생
    private void PlaySound(string keyWord) => Managers.Sound.PlaySound(gameObject, keyWord);
}

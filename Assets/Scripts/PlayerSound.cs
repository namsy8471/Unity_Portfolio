using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSound : MonoBehaviour
{
    private GameObject obj;

    private void Start()
    {
        obj = GameObject.Find("Player");
    }

    // 애니메이션 이벤트
    private void FootstepSound(string keyWord)
    {
        int num = Random.Range(1, 10);
        
        obj.SendMessage("PlaySound", keyWord + num, SendMessageOptions.DontRequireReceiver);
    }
    
}

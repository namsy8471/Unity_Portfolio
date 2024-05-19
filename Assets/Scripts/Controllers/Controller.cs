using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Animator Animator;
    public GameObject HpBar { get; private set;}
    public float InvincibleTimer { get; set; }
    
    protected void HpBarInit()
    {
        HpBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HPBarInWorldWorld/HPBarInWorldSpace"),
            gameObject.transform);
        HpBar.GetComponent<Canvas>().worldCamera = Camera.main;
        HpBar.transform.position = HpBar.transform.parent.position 
                                   + Vector3.up * (gameObject.GetComponent<Collider>().bounds.size.y);
        HpBar.SetActive(false);
    }

    protected virtual void Update()
    {
        if(HpBar.activeSelf) HpBar.transform.rotation = Camera.main.transform.rotation;
        if (InvincibleTimer >= 0) InvincibleTimer -= Time.deltaTime;
    }
}

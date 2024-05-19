using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public GameObject destWarp;
    private Warp _warp;

    private bool _isPlayerWarped;
    
    private void Start()
    {
        _warp = destWarp.GetComponent<Warp>();
        _isPlayerWarped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isPlayerWarped) return;

        var pos = destWarp.transform.position;
        other.transform.position = pos;
        Managers.Game.Player.GetComponent<PlayerController>().IdleState.DestPos = pos;
        _warp._isPlayerWarped = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isPlayerWarped = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventReceiver : MonoBehaviour
{
    private void Start() => gameObject.GetOrAddComponent<AudioSource>();
    
    private void AttackToEnemy()
        => Managers.Game.Player.GetComponent<PlayerController>().AttackState.Attack();
    
    private void PlaySound(string keyWord) => Managers.Sound.PlaySound(gameObject, keyWord);
}

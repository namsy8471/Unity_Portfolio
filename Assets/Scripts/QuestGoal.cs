using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoal : MonoBehaviour
{
    [SerializeField] private string questName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.Instance.SendMessage("CompleteQuest", questName, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}

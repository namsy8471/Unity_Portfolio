using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAgent : MonoBehaviour
{
    [SerializeField] private string questName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.Instance.SendMessage("AddQuestInPlayerQuestDictionary", questName, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}

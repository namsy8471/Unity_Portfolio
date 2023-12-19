using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestLetterBox : MonoBehaviour, IPointerClickHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        QuestManager.Instance.SendMessage("LetterBoxInteraction", SendMessageOptions.DontRequireReceiver);
    }
}

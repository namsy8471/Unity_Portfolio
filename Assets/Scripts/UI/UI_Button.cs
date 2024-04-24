using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Button : UI_Base, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData){ }
}

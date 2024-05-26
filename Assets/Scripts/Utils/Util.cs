using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null) 
            component = obj.AddComponent<T>();

        return component;
    }
    
    public static bool IsMousePointerOutOfScreen()
    {
        Vector2 mousePos = Input.mousePosition;
        if(mousePos.x < 0 || mousePos.y < 0 || Screen.width < mousePos.x || Screen.height < mousePos.y)
            return true;
        
        return false;
    }
}

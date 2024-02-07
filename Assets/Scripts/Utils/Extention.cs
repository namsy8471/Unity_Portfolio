using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extention
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        return Util.GetOrAddComponent<T>(obj);
    }
}

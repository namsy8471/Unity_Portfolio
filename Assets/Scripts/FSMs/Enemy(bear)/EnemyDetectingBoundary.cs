using System;
using UnityEngine;

public class EnemyDetectingBoundary : MonoBehaviour
{
    public Action<Collider> TriggerEnter;
    public Action<Collider> TriggerExit;
    
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(other);
    }
}

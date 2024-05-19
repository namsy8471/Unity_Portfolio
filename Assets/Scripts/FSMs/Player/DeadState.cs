using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IStateBase
{
    public void Init()
    {
        
    }

    public void StartState()
    {
        Managers.Graphics.UI.DieCanvasActive();
    }

    public void UpdateState()
    {
        
    }

    public void EndState()
    {
        
    }
}

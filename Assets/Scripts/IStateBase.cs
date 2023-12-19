using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateBase
{
    void StartState();
    void UpdateState();
    void EndState();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateBase
{
    void Init();
    void StartState();
    void UpdateState();
    void EndState();
}

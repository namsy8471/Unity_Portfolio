using System;
using UnityEngine;
using UnityEngine.UI;


public class TargetingSystem
{
    private Action _changeCursorForBattle;
    private Action _changeCursorForNormal;
    
    private Action _setClosestEnemyCollider;
    private Action<Vector3> _drawCircleOnEnemy;
    private Action _drawLineToEnemy;

    private Action _clearTargetingCircle;
    private Action _clearTargetingLineRenderer;
    
    public GameObject Target { get; set; }
    
    public void Init()
    {
        _changeCursorForBattle += Managers.Graphics.Cursor.ChangeCursorForBattle;
        _changeCursorForNormal += Managers.Graphics.Cursor.BackNormalCursor;
        
        Managers.Input.AddAction(Managers.Input.KeyButtonDown, Managers.Input.TargetingKey, FindEnemy);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.TargetingKey, TargetEnemy);
        Managers.Input.AddAction(Managers.Input.KeyButtonUp, Managers.Input.TargetingKey, ClearTarget);
    }

    public void Update()
    {
        
    }
    
    private void FindEnemy()
    {
        _setClosestEnemyCollider?.Invoke();
    }

    private void TargetEnemy()
    {
        _drawCircleOnEnemy?.Invoke(Target.transform.position);
        _drawLineToEnemy?.Invoke();
        
        _changeCursorForBattle?.Invoke();
    }
    
    private void ClearTarget()
    {
        Target = null;
        
        _clearTargetingLineRenderer?.Invoke();
        _clearTargetingCircle?.Invoke();
        
        _changeCursorForNormal?.Invoke();
    }
    
    public bool IsCurrentTargetExist()
    {
        return Target;
    }
    
    
    public GameObject GetCurrentTarget()
    {
        return Target;
    }

    #region Action Binding

    // This will be used in RayManager
    public void SetSetClosestEnemyCollider(Action action)
    {
        _setClosestEnemyCollider = action;
    }
    
    // This will be used in VisualManager
    public void SetDrawCircleAction(Action<Vector3> action)
    {
        _drawCircleOnEnemy = action;
    }
    
    public void SetDrawLineAction(Action action)
    {
        _drawLineToEnemy = action;
    }

    public void SetClearTargetingCircle(Action action)
    {
        _clearTargetingCircle = action;
    }

    public void SetClearTargetingLineRenderer(Action action)
    {
        _clearTargetingLineRenderer = action;
    }

    #endregion
}

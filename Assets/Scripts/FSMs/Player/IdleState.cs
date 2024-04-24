using System;
using Contents.Status;
using UnityEngine;
using UnityEngine.EventSystems;


public class IdleState : IStateBase
{
    // FSM 상태 관련
    enum EInputType
    {
        Nothing,
        KeyBoard,
        Mouse
    }

    private EInputType InputType;
    
    private float _moveHorizontal;
    private float _moveVertical;

    private PlayerController _player;

    private Rigidbody _rb;
    
    private TargetingSystem _targetingSystem;
    private Animator _animator;
    
    public Vector3 DestPos { get; private set; }

    public void Init()
    {
        _player = Managers.Game.Player.GetComponent<PlayerController>();
        
        #region KeyBinding
        
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.WalkKey, ChangeToWalk);
        Managers.Input.AddAction(Managers.Input.KeyButtonUp, Managers.Input.WalkKey, ChangeToRun);
        
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveForwardKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveBackwardKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveLeftKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveRightKey, KeyboardInputCheck);
        
        Managers.Input.LMBPressed += MouseInputCheck;
        Managers.Input.LMBPressed += GetMousePos;
        
        _player.PlayerMousePressedActions.Add(MouseInputCheck);
        _player.PlayerMousePressedActions.Add(GetMousePos);
        #endregion
        
        _animator = _player.GetComponentInChildren<Animator>();
        _rb = _player.GetComponent<Rigidbody>();
        _targetingSystem = Managers.Game.TargetingSystem;
        _player.Status.MoveSpeed = 400f;
    }
    
    public void StartState()
    {
        InputType = EInputType.Nothing;
        
        _animator.SetBool("isRun", false);
        
    }

    public void UpdateState()
    {
        Move();
    }

    public void EndState()
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isRun", false);
    }
    
    private void KeyboardInputCheck()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
        
        InputType = EInputType.KeyBoard;
    }
    
    private void MouseInputCheck()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        InputType = EInputType.Mouse;
    }

    private void ChangeToWalk()
    {
        if (_rb.velocity.magnitude < 0.2f)
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("isWalk", false);
            _player.GetComponent<Status>().MoveSpeed = 400f;
        }
        else
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("isWalk", true);
            _player.GetComponent<Status>().MoveSpeed = 200f;
        }
    }

    private void ChangeToRun()
    {
        if (_rb.velocity.magnitude < 0.2f)
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("isWalk", false);
            _player.Status.MoveSpeed = 400f;
        }
        else
        {
            _animator.SetBool("isWalk", false);
            _animator.SetBool("isRun", true);
            _player.Status.MoveSpeed = 400f;
        }
    }

    private void Move()
    {
        switch (InputType)
        {
            case EInputType.Nothing:
                break;
            case EInputType.KeyBoard:
                if (_moveVertical == 0 && _moveHorizontal == 0)
                {
                    ChangeToInputTypeNothing();
                    break;
                }
                
                MoveByKeyboard();
                break;
            case EInputType.Mouse:

                if (CheckRange())
                {
                    ChangeToInputTypeNothing();
                    break;
                }
                
                MoveByMouse();
                break;
        }
    }

    private bool CheckRange()
    {
        if (_targetingSystem.Target != null)
        {
            if (Vector3.Distance(_player.transform.position, DestPos)
                <= _player.Status.AtkRange - 0.2f)
                return true;
        }
        else
        {
            if (Vector3.Distance(_player.transform.position, DestPos) <= 0.2f)
                return true;
        }
        
        return false;
    }
    
    private void ChangeToInputTypeNothing()
    {
        InputType = EInputType.Nothing;
        _rb.velocity = Vector3.zero;
        _animator.SetBool("isRun", false);
    }

    private void MoveByKeyboard()
    {
        _animator.SetBool("isRun", true);

        var camTransform = Camera.main.transform;
        var movement = camTransform.forward * _moveVertical + camTransform.right * _moveHorizontal;
        movement.y = 0f;

        var camForward = Quaternion.LookRotation(movement);
        _player.transform.rotation = camForward;
        _rb.velocity = movement.normalized * (_player.Status.MoveSpeed * Time.deltaTime);
        
        _moveHorizontal = 0;
        _moveVertical = 0;
    }

    private void GetMousePos()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (Managers.Game.TargetingSystem.isTargetingWorkNow)
        {
            DestPos = Managers.Game.TargetingSystem.Target.transform.position;
        }
        else
        {
            DestPos = Managers.Ray.RayHitPoint;
            _targetingSystem.Target = null;
            
            if (Managers.Ray.RayHitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _targetingSystem.Target = Managers.Ray.RayHitCollider.transform.gameObject;
            }
        }
    }
    
    private void MoveByMouse()
    {
        _animator.SetBool("isRun", true);

        var position = _player.transform.position;
        var newPos = new Vector3(DestPos.x, position.y, DestPos.z);
        DestPos = newPos;
        
        Vector3 moveDirection = (DestPos - position).normalized;
        _rb.velocity = moveDirection * (_player.Status.MoveSpeed * Time.deltaTime);

        Vector3 lookAtTarget = new Vector3(DestPos.x, position.y, DestPos.z);
        _player.transform.LookAt(lookAtTarget);
    }
    
}

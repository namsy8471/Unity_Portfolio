using System;
using Contents.Status;
using UnityEngine;
using UnityEngine.EventSystems;


public class IdleState : IStateBase
{
    // FSM 상태 관련
    enum InputType
    {
        Nothing,
        KeyBoard,
        Mouse
    }

    private InputType _inputType;

    private PlayerController _controller;

    private Rigidbody _rb;
    
    private TargetingSystem _targetingSystem;
    private Animator _animator;

    public float MoveHorizontal { get; set; }
    public float MoveVertical { get; set; }

    public bool CanMove { get; set; } = true;
    public bool FixedToWalk { get; set; } = false;
    public Vector3 DestPos { get; set; }

    public void Init()
    {
        _controller = Managers.Game.Player.GetComponent<PlayerController>();
        
        #region KeyBinding
        
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.WalkKey, ChangeToWalk);
        Managers.Input.AddAction(Managers.Input.KeyButtonUp, Managers.Input.WalkKey, ChangeToRun);
        
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveForwardKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveBackwardKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveLeftKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveRightKey, KeyboardInputCheck);
        
        Managers.Input.LMBPressed += MouseInputCheck;
        Managers.Input.LMBPressed += GetMousePos;
        
        _controller.PlayerMousePressedActions.Add(MouseInputCheck);
        _controller.PlayerMousePressedActions.Add(GetMousePos);
        #endregion
        
        _animator = _controller.GetComponentInChildren<Animator>();
        _rb = _controller.GetComponent<Rigidbody>();
        _targetingSystem = Managers.Game.TargetingSystem;
        _controller.Status.MoveSpeed = 400f;
    }
    
    public void StartState()
    {
        _inputType = InputType.Nothing;
        
        _animator.SetBool("isRun", false);
        
    }

    public void UpdateState()
    {
        if (!CanMove) return;
        Debug.DrawRay(Managers.Game.Player.transform.position + Vector3.up, Managers.Game.Player.transform.forward, Color.red,
            2.0f);
        Move();
    }

    public void EndState()
    {
        ChangeToStop();
    }
    
    private void KeyboardInputCheck()
    {
        MoveHorizontal = Input.GetAxisRaw("Horizontal");
        MoveVertical = Input.GetAxisRaw("Vertical");
        
        _inputType = InputType.KeyBoard;
    }
    
    private void MouseInputCheck()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        _inputType = InputType.Mouse;
    }

    public void ChangeToStop()
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isRun", false);
        _animator.SetBool("isMove", false);
        _controller.Status.MoveSpeed = 400f;
    }
    
    public void ChangeToWalk()
    {
        if (!_animator.GetBool("isMove"))
        {
            ChangeToStop();
        }
        else
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("isWalk", true);
            _controller.Status.MoveSpeed = 200f;
        }
    }

    public void ChangeToRun()
    {
        if (!_animator.GetBool("isMove"))
        {
            ChangeToStop();
        }
        else
        {
            _animator.SetBool("isWalk", false);
            _animator.SetBool("isRun", true);
            _controller.Status.MoveSpeed = 400f;
        }
    }

    private void Move()
    {
        switch (_inputType)
        {
            case InputType.Nothing:
                break;
            case InputType.KeyBoard:
                if (MoveVertical == 0 && MoveHorizontal == 0)
                {
                    ChangeToInputTypeNothing();
                    break;
                }
                
                MoveByKeyboard();
                break;
            case InputType.Mouse:

                if (CheckRange())
                {
                    ChangeToInputTypeNothing();
                    break;
                }
                
                MoveByMouse();
                break;
        }
        
        if(FixedToWalk) ChangeToWalk();
    }

    private bool CheckRange()
    {
        if (Vector3.Distance(_controller.transform.position, DestPos) <= 0.2f)
            return true;
        
        return false;
    }
    
    public void ChangeToInputTypeNothing()
    {
        _inputType = InputType.Nothing;
        _rb.velocity = Vector3.zero;
        
        _animator.SetBool("isRun", false);
        _animator.SetBool("isMove", false);
    }

    private void MoveByKeyboard()
    {
        _animator.SetBool("isRun", true);
        _animator.SetBool("isMove", true);
        
        var camTransform = Camera.main.transform;
        var movement = camTransform.forward * MoveVertical + camTransform.right * MoveHorizontal;
        movement.y = 0f;

        var camForward = Quaternion.LookRotation(movement);
        _controller.transform.rotation = camForward;
        _rb.velocity = movement.normalized * (_controller.Status.MoveSpeed * Time.deltaTime);
        
        MoveHorizontal = 0;
        MoveVertical = 0;
        
        CheckWall();
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
        _animator.SetBool("isMove", true);
        
        var position = _controller.transform.position;
        var newPos = new Vector3(DestPos.x, position.y, DestPos.z);
        DestPos = newPos;
        
        Vector3 moveDirection = (DestPos - position).normalized;
        _rb.velocity = moveDirection * (_controller.Status.MoveSpeed * Time.deltaTime);

        Vector3 lookAtTarget = new Vector3(DestPos.x, position.y, DestPos.z);
        _controller.transform.LookAt(lookAtTarget);

        CheckWall();
    }

    private void CheckWall()
    {
        if (Physics.Raycast(Managers.Game.Player.transform.position + Vector3.up * 0.3f,
                Managers.Game.Player.transform.forward,
                1.0f, LayerMask.GetMask("Building")))
            ChangeToStop();
    }
}

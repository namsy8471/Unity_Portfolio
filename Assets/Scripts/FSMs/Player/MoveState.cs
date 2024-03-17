using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class MoveState : IStateBase
{
    // FSM 상태 관련
    enum InputType
    {
        Start,
        KeyBoard,
        Mouse
    }

    enum MouseState
    {
        Move,
        Finish
    }

    private InputType _inputType;
    private MouseState _mouseState;
    
    private float _moveHorizontal;
    private float _moveVertical;

    private GameObject _player;
    private Vector3 _destPos;
    public Vector3 DestPos
    {
        get => _destPos;
        set => _destPos = value;
    }
    
    private Rigidbody _rb;
    private bool _isMoveDone;

    private TargetingSystem _targetingSystem;
    private Animator _animator;
    
    public void Init()
    {
        _player = Managers.Game.Player;
        
        #region KeyBinding
        
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.WalkKey, ChangeToWalk);
        Managers.Input.AddAction(Managers.Input.KeyButtonUp, Managers.Input.WalkKey, ChangeToRun);
        
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveForwardKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveBackwardKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveLeftKey, KeyboardInputCheck);
        Managers.Input.AddAction(Managers.Input.KeyButtonPressed, Managers.Input.MoveRightKey, KeyboardInputCheck);
        
        Managers.Input.LMBPressed += MouseInputCheck;
        Managers.Input.LMBPressed += GetMousePos;
        
        _player.GetComponent<PlayerController>().PlayerMousePressedActions.Add(MouseInputCheck);
        _player.GetComponent<PlayerController>().PlayerMousePressedActions.Add(GetMousePos);
        #endregion
        
        _animator = _player.GetComponentInChildren<Animator>();
        _rb = _player.GetComponent<Rigidbody>();
        _targetingSystem = Managers.Game.TargetingSystem;
        _player.GetComponent<Stat>().MoveSpeed = 400f;
    }
    
    public void StartState()
    {
        _inputType = InputType.Start;
        
        _animator.SetBool("isRun", true);
        
        _isMoveDone = false;
    }

    public void UpdateState()
    {
        switch (_inputType)
        {
            case InputType.Start:
                if (_moveVertical != 0 || _moveHorizontal != 0)
                {
                    _inputType = InputType.KeyBoard;
                    break;
                }
                
                break;
            
            case InputType.KeyBoard:
                
                if (_moveVertical == 0 && _moveHorizontal == 0)
                {
                    _isMoveDone = true;
                    break;
                }
                
                Move();
                
                break;

            case InputType.Mouse:
            {
                if (_moveVertical != 0 || _moveHorizontal != 0)
                {
                    _inputType = InputType.KeyBoard;
                    break;
                }
                
                switch (_mouseState)
                {
                    case MouseState.Move:

                        if ((Managers.Game.Player.transform.position - _destPos).magnitude <= 0.1f )
                        {
                            _mouseState = MouseState.Finish;
                            break;
                        }
                        
                        MoveByMouse();
                        
                        break;

                    case MouseState.Finish:
                        
                        _isMoveDone = true;
                        
                        break;
                    default:
                        break;
                }

                break;
            }
            
            default:
                break;
        }

    }

    private void KeyboardInputCheck()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
        
        _inputType = InputType.KeyBoard;
    }
    
    private void MouseInputCheck()
    {
        _inputType = InputType.Mouse;
        _mouseState = MouseState.Move;
    }

    public void EndState()
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isRun", false);
    }

    private void ChangeToWalk()
    {
        if (_rb.velocity.magnitude < 0.2f)
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("isWalk", false);
            _player.GetComponent<Stat>().MoveSpeed = 400f;
        }
        else
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("isWalk", true);
            _player.GetComponent<Stat>().MoveSpeed = 200f;
        }
    }

    private void ChangeToRun()
    {
        if (_rb.velocity.magnitude < 0.2f)
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("isWalk", false);
            _player.GetComponent<Stat>().MoveSpeed = 400f;
        }
        else
        {
            _animator.SetBool("isWalk", false);
            _animator.SetBool("isRun", true);
            _player.GetComponent<Stat>().MoveSpeed = 400f;
        }
    }

    private void Move()
    {
        var camTransform = Camera.main.transform;
        // 카메라가 보는 방향으로 이동
        var movement = camTransform.forward * _moveVertical + camTransform.right * _moveHorizontal;
        movement.y = 0f;

        var camForward = Quaternion.LookRotation(movement);
        _player.transform.rotation = camForward;
        _rb.velocity = movement.normalized * (_player.GetComponent<Stat>().MoveSpeed * Time.deltaTime);
        
        _moveHorizontal = 0;
        _moveVertical = 0;
    }

    private void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground") |
                                 1 << LayerMask.NameToLayer("Enemy") |
                                 1 << LayerMask.NameToLayer("Item");
        
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            _destPos = hit.point;
            
            if ((hit.transform.gameObject.layer == 1 << LayerMask.NameToLayer("Enemy") && !hit.collider.isTrigger) ||
                hit.transform.gameObject.layer == 1 << LayerMask.NameToLayer("Item"))
                _targetingSystem.Target = hit.transform.gameObject;
            
            else _targetingSystem.Target = null;
        }
    }
    
    private void MoveByMouse()
    {
        var position = _player.transform.position;
        _destPos.y = position.y;
        Vector3 moveDirection = (_destPos - position).normalized;
        _rb.velocity = moveDirection * (_player.GetComponent<Stat>().MoveSpeed * Time.deltaTime);

        Vector3 lookAtTarget = new Vector3(_destPos.x, position.y, _destPos.z);
        _player.transform.LookAt(lookAtTarget);

        if (_rb.velocity.magnitude <= 0.1f) _isMoveDone = true;
    }
    
    public bool IsMoveDone => _isMoveDone;
}

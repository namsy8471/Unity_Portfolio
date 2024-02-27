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
        Clicked,
        Move,
        Finish
    }
    
    enum WalkStyle
    {
        Walk,
        Run
    }

    private InputType _inputType;
    private MouseState _mouseState;
    private WalkStyle _walkStyle;
    
    // Moving parameter 움직임과 관련된 변수들 ////////////////
    /////////////////////////////////////////////////////////
    
    private float _moveHorizontal;
    private float _moveVertical;

    private GameObject _player;
    private Vector3 _destPos; // 목표 위치
    private Rigidbody _rb;          // 리지드바디

    private bool _isMoveDone;        // 이동 종료?
    private Camera _mainCam;

    ///////////////////////////////////////////////////////////

    private TargetingSystem _targetingSystem;
    private Animator _animator;


    public void Init()
    {
        _player = Managers.Game.Player;
        
        _animator = _player.GetComponentInChildren<Animator>();
        _rb = _player.GetComponent<Rigidbody>();
        _targetingSystem = Managers.Game.TargetingSystem;
        
        _mainCam = Camera.main;
    }
    
    public void StartState()
    {
        _inputType = InputType.Start;

        if (Input.GetKey(KeyCode.LeftShift))
            ChangeState(WalkStyle.Walk);
        else
            ChangeState(WalkStyle.Run);
        
        _isMoveDone = false;
    }

    public void UpdateState()
    {
        
        // 키보드로 움직이는 방식
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
        
        switch (_inputType)
        {
            case InputType.Start:
                if (_moveVertical != 0 || _moveHorizontal != 0)
                {
                    _inputType = InputType.KeyBoard;
                    break;
                }
                
                if (Input.GetMouseButton(0))
                {
                    _inputType = InputType.Mouse;
                    _mouseState = MouseState.Clicked;
                    break;
                }

                _isMoveDone = true;
                break;
            
            case InputType.KeyBoard:

                if (Input.GetMouseButton(0))
                {
                    _inputType = InputType.Mouse;
                    _mouseState = MouseState.Clicked;
                    break;
                }

                if (_moveVertical == 0 && _moveHorizontal == 0)
                {
                    _isMoveDone = true;
                    break;
                }
                
                Move();

                break;

            case InputType.Mouse:
            {
                // Mouse 상태일 때 키보드 입력이 감지되면 마우스 이동으로 변경
                if (_moveVertical != 0 || _moveHorizontal != 0)
                {
                    _inputType = InputType.KeyBoard;
                    break;
                }
                
                // Mouse 상태 내의 FSM
                switch (_mouseState)
                {
                    case MouseState.Clicked:

                        GetMousePos();
                        
                        _mouseState = MouseState.Move;
                        break;

                    case MouseState.Move:

                        if ((Managers.Game.Player.transform.position - _destPos).magnitude <= 0.1f )
                        {
                            _mouseState = MouseState.Finish;
                            break;
                        }
                        
                        MoveByMouse();
                        
                        if (Input.GetMouseButton(0))
                        {
                            _mouseState = MouseState.Clicked;
                            break;
                        }
                        
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

        
        switch (_walkStyle)
        {
            case WalkStyle.Walk:

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    ChangeState(WalkStyle.Run);
                    break;
                }

                break;
            
            case WalkStyle.Run:

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ChangeState(WalkStyle.Walk);
                    break;
                }
                
                break;
        }
    }

    public void EndState()
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isRun", false);
    }

    void ChangeState(WalkStyle style)
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");

        if (Managers.Cursor.IsDragging || EventSystem.current.IsPointerOverGameObject() && (_moveHorizontal == 0 && _moveVertical == 0)) return;

        switch (_walkStyle)
        {
            case WalkStyle.Walk:
                _animator.SetBool("isWalk", false);
                break;
            
            case WalkStyle.Run:
                _animator.SetBool("isRun", false);
                break;
            default:
                break;
        }

        _walkStyle = style;

        switch (_walkStyle)
        {
            case WalkStyle.Walk:
                _animator.SetBool("isWalk", true);
                _player.GetComponent<Stat>().MoveSpeed = 200f;
                break;
            
            case WalkStyle.Run:
                _animator.SetBool("isRun", true);
                _player.GetComponent<Stat>().MoveSpeed = 400f;
                break;
            default:
                break;
        }
    }
    private void Move()
    {
        var camTransform = _mainCam.transform;
        // 카메라가 보는 방향으로 이동
        var movement = camTransform.forward * _moveVertical + camTransform.right * _moveHorizontal;
        movement.y = 0f;

        var camForward = Quaternion.LookRotation(movement);
        _player.transform.rotation = camForward;
        _rb.velocity = movement.normalized * (_player.GetComponent<Stat>().MoveSpeed * Time.deltaTime); // 대각선 이동이 더 빠르지 않도록 노멀라이즈
    }

    private void GetMousePos()
    {
        if (Managers.Cursor.IsDragging || EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground") |
                               1 << LayerMask.NameToLayer("Enemy") |
                               1 << LayerMask.NameToLayer("Item");
        
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            _destPos = hit.point;
            
            if (hit.transform.gameObject.layer == 1 << LayerMask.NameToLayer("Enemy") || hit.transform.gameObject.layer == 1 << LayerMask.NameToLayer("Item"))
                _targetingSystem.Target = hit.transform.gameObject;
            else _targetingSystem.Target = null;
        }
    }
    
    private void MoveByMouse()
    {
        if (Managers.Cursor.IsDragging || EventSystem.current.IsPointerOverGameObject()) return;
        
        // 마우스로 움직이는 방식
        var position = _player.transform.position;

        _destPos.y = position.y; // 캐릭터의 높이와 맞추기
        Vector3 moveDirection = (_destPos - position).normalized;
        _rb.velocity = moveDirection * (_player.GetComponent<Stat>().MoveSpeed * Time.deltaTime);

        // 캐릭터가 바라보는 방향 조절
        Vector3 lookAtTarget = new Vector3(_destPos.x, position.y, _destPos.z);
        _player.transform.LookAt(lookAtTarget);
    }
    
    public bool IsMoveDone => _isMoveDone;
}

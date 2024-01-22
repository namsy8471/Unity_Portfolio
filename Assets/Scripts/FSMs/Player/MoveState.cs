using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MoveState : MonoBehaviour, IStateBase
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
    
    enum WalkState
    {
        Walk,
        Run
    }

    private InputType inputType;
    private MouseState mouseState;
    private WalkState walkState;
    
    // Moving parameter 움직임과 관련된 변수들 ////////////////
    /////////////////////////////////////////////////////////

    [SerializeField]private float movingSpeed = 200;
    [SerializeField]private float walkSpeedOffset = 200f;
    [SerializeField]private float runSpeedOffset = 400f;
    
    private float moveHorizontal;
    private float moveVertical;
    
    private Vector3 targetPosition; // 목표 위치 (마우스 좌클릭)
    private Rigidbody rb;          // 리지드바디

    private bool isMoveDone;        // 이동 종료?
    private Camera mainCam;

    private bool isMouseOnInventory;
    ///////////////////////////////////////////////////////////

    // 마우스 클릭 프리팹용 파티클 매니저
    private ParticleManager particleManager;
    private TargetingSystem targetingSystem;
    private Animator animator;
    
    private void Start()
    {
        particleManager = GameObject.Find("ParticleManager").GetComponent<ParticleManager>();
        targetingSystem = GetComponent<TargetingSystem>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        
        isMoveDone = false;
        isMouseOnInventory = false;
    }
    
    public void StartState()
    {
        // Debug.Log("Move State Start!");
        inputType = InputType.Start;
        
        if (Input.GetKey(KeyCode.LeftShift))
            ChangeState(WalkState.Walk);
        else
            ChangeState(WalkState.Run);
        
        isMoveDone = false;
    }

    public void UpdateState()
    {
        // Debug.Log("Move State Update!");
        
        // 키보드로 움직이는 방식
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        
        switch (inputType)
        {
            case InputType.Start:
                if (moveVertical != 0 || moveHorizontal != 0)
                {
                    inputType = InputType.KeyBoard;
                    break;
                }
                
                if (Input.GetMouseButton(0))
                {
                    inputType = InputType.Mouse;
                    mouseState = MouseState.Clicked;
                    break;
                }

                isMoveDone = true;
                break;
            
            case InputType.KeyBoard:

                if (Input.GetMouseButton(0))
                {
                    inputType = InputType.Mouse;
                    mouseState = MouseState.Clicked;
                    break;
                }

                if (moveVertical == 0 && moveHorizontal == 0)
                {
                    isMoveDone = true;
                    break;
                }
                
                Move();

                break;

            case InputType.Mouse:
            {
                // Mouse 상태일 때 키보드 입력이 감지되면 마우스 이동으로 변경
                if (moveVertical != 0 || moveHorizontal != 0)
                {
                    inputType = InputType.KeyBoard;
                    break;
                }
                
                if (isMouseOnInventory && rb.velocity == Vector3.zero)
                {
                    isMoveDone = true;
                    break;
                }
                
                // Mouse 상태 내의 FSM
                switch (mouseState)
                {
                    case MouseState.Clicked:

                        if(targetingSystem.IsCurrentTargetExist())
                            GetEnemyPos();
                        else
                            GetMousePos();
                        
                        mouseState = MouseState.Move;
                        break;

                    case MouseState.Move:

                        MoveByMouse();
                        
                        if (rb.velocity == Vector3.zero)
                        {
                            isMoveDone = true;
                            break;
                        }
                        
                        if (Input.GetMouseButton(0))
                        {
                            mouseState = MouseState.Clicked;
                            break;
                        }
                        
                        break;

                    case MouseState.Finish:
                        
                        isMoveDone = true;

                        break;
                    default:
                        break;
                }

                break;
            }
            
            default:
                break;
        }

        
        switch (walkState)
        {
            case WalkState.Walk:

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    ChangeState(WalkState.Run);
                    break;
                }

                break;
            
            case WalkState.Run:

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ChangeState(WalkState.Walk);
                    break;
                }
                
                break;
        }
    }

    public void EndState()
    {
        // Debug.Log("Move State End!");
        rb.velocity = Vector3.zero;
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
    }

    void ChangeState(WalkState state)
    {
        if(isMouseOnInventory) return;

        switch (walkState)
        {
            case WalkState.Walk:
                animator.SetBool("isWalk", false);
                break;
            
            case WalkState.Run:
                animator.SetBool("isRun", false);
                break;
            default:
                break;
        }

        walkState = state;

        switch (walkState)
        {
            case WalkState.Walk:
                animator.SetBool("isWalk", true);
                movingSpeed = walkSpeedOffset;
                break;
            
            case WalkState.Run:
                animator.SetBool("isRun", true);
                movingSpeed = runSpeedOffset;
                break;
            default:
                break;
        }
    }
    private void Move()
    {
        var camTransform = mainCam.transform;
        // 카메라가 보는 방향으로 이동
        var movement = camTransform.forward * moveVertical + camTransform.right * moveHorizontal;
        movement.y = 0f;

        var camForward = Quaternion.LookRotation(movement);
        transform.rotation = camForward;
        rb.velocity = movement.normalized * (movingSpeed * Time.deltaTime); // 대각선 이동이 더 빠르지 않도록 노멀라이즈
    }

    private void GetMousePos()
    {
        if (isMouseOnInventory) return;
        
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
        {
            targetPosition = hit.point;
            // 클릭 애니메이션 생성 및 도착지 트리거 생성
            particleManager.CreateMouseClickParticle(targetPosition);
        }
    }

    private void GetEnemyPos()
    {
        if(targetingSystem.IsCurrentTargetExist())
            targetPosition = targetingSystem.GetCurrentTargetPos();
    }
    
    private void MoveByMouse()
    {
        // 마우스로 움직이는 방식
        var position = transform.position;

        targetPosition.y = position.y; // 캐릭터의 높이와 맞추기
        Vector3 moveDirection = (targetPosition - position).normalized;
        rb.velocity = moveDirection * (movingSpeed * Time.deltaTime);

        // 캐릭터가 바라보는 방향 조절
        Vector3 lookAtTarget = new Vector3(targetPosition.x, position.y, targetPosition.z);
        transform.LookAt(lookAtTarget);
    }
    
    public void SetSpeed(float speedValue)
    {
        movingSpeed = speedValue;
    }
    
    public float GetSpeed()
    {
        return movingSpeed;
    }
    
    public bool GetIsMoveDone()
    {
        return isMoveDone;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestPos"))
        {
            Destroy(other);
            mouseState = MouseState.Finish;
        }
    }

    // Unity SendMassage from InventoryHandle
    private void LockMakingWaypoint()
    {
        isMouseOnInventory = true;
    }
    
    // Unity SendMassage from InventoryHandle
    private void UnlockMakingWaypoint()
    {
        isMouseOnInventory = false;
    }
}

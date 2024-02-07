using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    enum CamMoveState
    {
        IdleStart,
        Idle,
        MoveStart,
        Move
    }

    private CamMoveState camMoveState;
    
    [SerializeField]private CinemachineFreeLook freeLookCamera;
    [SerializeField]private float xSpeedOffset = 300.0f;
    [SerializeField]private float ySpeedOffset = 2.0f;

    private float mouseWheel;
    
    void Start()
    {
        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        freeLookCamera.Follow = Managers.Game.Player.transform;
        freeLookCamera.LookAt = Managers.Game.Player.transform;
        
        camMoveState = CamMoveState.IdleStart;
    }

    private void LateUpdate()
    {
        switch (camMoveState)
        {
            case CamMoveState.IdleStart:
                // 카메라 회전을 비활성화
                freeLookCamera.m_XAxis.m_InputAxisName = ""; // X 축 (수평 회전) 입력 비활성화
                freeLookCamera.m_YAxis.m_InputAxisName = ""; // Y 축 (수직 회전) 입력 비활성화
                freeLookCamera.m_XAxis.m_MaxSpeed = 0;
                freeLookCamera.m_YAxis.m_MaxSpeed = 0;

                camMoveState = CamMoveState.Idle;
                break;
            
            case CamMoveState.Idle:
                
                if (Input.GetMouseButton(1))
                {
                    camMoveState = CamMoveState.MoveStart;
                    break;
                }
                break;
            
            case CamMoveState.MoveStart:
                // 카메라 회전을 활성화
                freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X"; // X 축 (수평 회전) 입력 활성화
                freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y"; // Y 축 (수직 회전) 입력 활성화
                freeLookCamera.m_XAxis.m_MaxSpeed = xSpeedOffset;
                freeLookCamera.m_YAxis.m_MaxSpeed = ySpeedOffset;

                camMoveState = CamMoveState.Move;
                break;
            
            case CamMoveState.Move:
                if (Input.GetMouseButtonUp(1))
                {
                    camMoveState = CamMoveState.IdleStart;
                    break;
                }
                
                break;

            default:
                break;
        }

        
        mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        
        
        if(mouseWheel != 0){
            freeLookCamera.m_Orbits[1] = new CinemachineFreeLook.Orbit(
                freeLookCamera.m_Orbits[1].m_Height - mouseWheel * 2f,
                freeLookCamera.m_Orbits[1].m_Radius - mouseWheel * 2f);

            freeLookCamera.m_Orbits[0] = new CinemachineFreeLook.Orbit(
                (freeLookCamera.m_Orbits[1].m_Height - freeLookCamera.m_Orbits[2].m_Height) * 2.0f,
                freeLookCamera.m_Orbits[0].m_Radius);

            if (freeLookCamera.m_Orbits[1].m_Height >= 14)
            {
                freeLookCamera.m_Orbits[1].m_Height = 14;
                freeLookCamera.m_Orbits[1].m_Radius = 17;
            }
            else if (freeLookCamera.m_Orbits[1].m_Height <= 1)
            {
                freeLookCamera.m_Orbits[1].m_Height = 1;
                freeLookCamera.m_Orbits[1].m_Radius = 4;
            }
            
        }
    }
}

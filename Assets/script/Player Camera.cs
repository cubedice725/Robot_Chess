using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    private float scrollSpeed = 2000.0f;
    private float mouseSpeed = 100f;
    float scrollWheel;
    public void SetCamera()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    // 화면 따라가기
    public void PlayerFollow(Transform playerT)
    {
        cam.Follow = playerT;
    }
    // 카메라 움직임
    public void CameraMove()
    {
        Vector3 mousePosition = Input.mousePosition;

        // 카메라 왼쪽으로 천천히
        if (mousePosition.x <= 0)
        {
            CamFollowNull();
            cam.transform.Translate(-Time.deltaTime * mouseSpeed, 0, 0);
        }
        // 카메라 왼쪽으로 빨리
        else if (mousePosition.x <= 100)
        {
            CamFollowNull();
            cam.transform.Translate(-Time.deltaTime * mouseSpeed / 10, 0, 0);
        }

        // 카메라 오른쪽으로 천천히
        else if (mousePosition.x >= Screen.width - 1)
        {
            CamFollowNull();
            cam.transform.Translate(Time.deltaTime * mouseSpeed, 0, 0);
        }
        // 카메라 오른쪽으로 빨리
        else if (mousePosition.x >= Screen.width - 100)
        {
            CamFollowNull();
            cam.transform.Translate(Time.deltaTime * mouseSpeed / 10, 0, 0);
        }

        // 카메라 위로 천천히
        if (mousePosition.y <= 0)
        {
            CamFollowNull();
            cam.transform.Translate(0, -Time.deltaTime * mouseSpeed, 0);
        }
        // 카메라 위로 빨리
        else if (mousePosition.y <= 100)
        {
            CamFollowNull();
            cam.transform.Translate(0, -Time.deltaTime * mouseSpeed/10, 0);
        }

        // 카메라 아래로 천천히
        else if (mousePosition.y >= Screen.height - 1)
        {
            CamFollowNull();
            cam.transform.Translate(0, Time.deltaTime * mouseSpeed, 0);
        }
        // 카메라 아래로 빨리
        else if (mousePosition.y >= Screen.height - 100)
        {
            CamFollowNull();
            cam.transform.Translate(0, Time.deltaTime * mouseSpeed/10, 0);
        }
    }

    // 카메라 줌인, 줌아웃
    public void ZoomInOut()
    {
        scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        cam.transform.Translate(0, 0, scrollWheel * Time.deltaTime * scrollSpeed);

        if (cam.transform.position.y <= 3)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, 3, cam.transform.position.z);
        }
        if (cam.transform.position.y >= 25)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, 25, cam.transform.position.z);
        }

        // 진짜 줌인 줌아웃 하지만 외곡이 심해서 제외
        //cam.m_Lens.FieldOfView += Mathf.FloorToInt(- scrollWheel * Time.deltaTime * scrollSpeed);
        //
        //if (cam.m_Lens.FieldOfView <= 10)
        //{
        //    cam.m_Lens.FieldOfView = 10;
        //}
        //if (cam.m_Lens.FieldOfView >= 100)
        //{
        //    cam.m_Lens.FieldOfView = 100;
        //}
    }

    // 플레이어 추척을 천천히 함
    private void CamFollowNull()
    {
        if (cam.Follow != null)
        {
            cam.Follow = null;
        }
    }
}

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
    // ȭ�� ���󰡱�
    public void PlayerFollow(Transform playerT)
    {
        cam.Follow = playerT;
    }
    // ī�޶� ������
    public void CameraMove()
    {
        Vector3 mousePosition = Input.mousePosition;

        // ī�޶� �������� õõ��
        if (mousePosition.x <= 0)
        {
            CamFollowNull();
            cam.transform.Translate(-Time.deltaTime * mouseSpeed, 0, 0);
        }
        // ī�޶� �������� ����
        else if (mousePosition.x <= 100)
        {
            CamFollowNull();
            cam.transform.Translate(-Time.deltaTime * mouseSpeed / 10, 0, 0);
        }

        // ī�޶� ���������� õõ��
        else if (mousePosition.x >= Screen.width - 1)
        {
            CamFollowNull();
            cam.transform.Translate(Time.deltaTime * mouseSpeed, 0, 0);
        }
        // ī�޶� ���������� ����
        else if (mousePosition.x >= Screen.width - 100)
        {
            CamFollowNull();
            cam.transform.Translate(Time.deltaTime * mouseSpeed / 10, 0, 0);
        }

        // ī�޶� ���� õõ��
        if (mousePosition.y <= 0)
        {
            CamFollowNull();
            cam.transform.Translate(0, -Time.deltaTime * mouseSpeed, 0);
        }
        // ī�޶� ���� ����
        else if (mousePosition.y <= 100)
        {
            CamFollowNull();
            cam.transform.Translate(0, -Time.deltaTime * mouseSpeed/10, 0);
        }

        // ī�޶� �Ʒ��� õõ��
        else if (mousePosition.y >= Screen.height - 1)
        {
            CamFollowNull();
            cam.transform.Translate(0, Time.deltaTime * mouseSpeed, 0);
        }
        // ī�޶� �Ʒ��� ����
        else if (mousePosition.y >= Screen.height - 100)
        {
            CamFollowNull();
            cam.transform.Translate(0, Time.deltaTime * mouseSpeed/10, 0);
        }
    }

    // ī�޶� ����, �ܾƿ�
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

        // ��¥ ���� �ܾƿ� ������ �ܰ��� ���ؼ� ����
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

    // �÷��̾� ��ô�� õõ�� ��
    private void CamFollowNull()
    {
        if (cam.Follow != null)
        {
            cam.Follow = null;
        }
    }
}

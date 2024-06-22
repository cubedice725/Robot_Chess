using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    private float scrollSpeed = 2000.0f;
    float scrollWheel;
    public float ScrollWheel { set { scrollWheel = value; } }
    public void SetCameraControl()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    public void PlayerMoveCamera()
    {

    }
    // Ä«¸Þ¶ó ÁÜÀÎ, ÁÜ¾Æ¿ô
    public void ZoomInOut()
    {
        if (cam.m_Lens.FieldOfView < 10)
        {
            cam.m_Lens.FieldOfView = 11;
        }
        if (cam.m_Lens.FieldOfView > 100)
        {
            cam.m_Lens.FieldOfView = 101;
        }

        cam.m_Lens.FieldOfView += -scrollWheel * Time.deltaTime * scrollSpeed;

        if (cam.m_Lens.FieldOfView < 10)
        {
            cam.m_Lens.FieldOfView = 11;
        }
        if (cam.m_Lens.FieldOfView > 100)
        {
            cam.m_Lens.FieldOfView = 101;
        }
    }
}

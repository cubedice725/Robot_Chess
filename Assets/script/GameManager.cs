using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    Map map;
    Player player;
    PlayerCameraControl playerCamera;

    void Awake()
    {
        // Create Map script object, An object has a script.
        map = GetComponent<Map>();

        // Create Player script object, Another object has a script.
        player = GameObject.Find("Player").GetComponent<Player>();
        playerCamera = GameObject.Find("Player Camera").GetComponent<PlayerCameraControl>();

        // Create objects for scene
        map.SetMap(10000);
        player.SetMovePlane(10);
        playerCamera.SetCameraControl();

        map.CreateMap(100, 100);
    }
    void Update()
    {
        // ���� ���콺 ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            // ���� ī�޶� ���� ���콺 Ŭ���� ���� ray ������ ������
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ray�� ���� ��ü�� Ȯ�� �� ����
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if(hit.transform.name == "Move Plane(Clone)")
                {
                    player.Hit = hit;
                    player.PlayerMove();
                    print(hit.transform.name + hit.transform.position);
                }
                if (hit.transform.name == "Player")
                {
                    player.ReadyPlayerMove();
                    print(hit.transform.name + hit.transform.position);
                }
            }
        }

        if(0f != Input.GetAxis("Mouse ScrollWheel"))
        {
            playerCamera.ScrollWheel = Input.GetAxis("Mouse ScrollWheel");
            playerCamera.ZoomInOut();
        }
    }
}


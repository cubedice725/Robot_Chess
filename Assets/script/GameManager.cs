using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    Map map;
    MiniMap miniMap;
    Player player;
    PlayerCamera playerCamera;

    bool test1 = false;
    bool test2 = false;

    void Awake()
    {
        map = FindObjectOfType<Map>();
        miniMap = FindObjectOfType<MiniMap>();
        player = FindObjectOfType<Player>();
        playerCamera = FindObjectOfType<PlayerCamera>();

        // �� ������ �ʿ��� ���� �� ��� ����
        map.X = 10;
        map.Z = 15;
        map.SetMap();
        
        // �̴ϸ� ������ �ʿ��� ���� �� ��� ����
        miniMap.Wall = 150;
        miniMap.Monster = 1;
        miniMap.SetMiniMap();
        
        player.SetMovePlane(1000);
        playerCamera.SetCamera();
    }
    void FixedUpdate()
    {
//////////////////////////////////////////////////////////
////////////////////���߿� �� �����ؾ���////////////////////
//////////////////////////////////////////////////////////
        if (test1)
        {
            map.SetCheckBox();
            test1 = false;
            test2 = true;
        }
    }
    void Update()
    {
        if (test2)
        {
            map.CheckBox();
            test2 = false;
        }
//////////////////////////////////////////////////////////

        // ---------------- ���콺 ��ġ ----------------
        playerCamera.CameraMove();

        // ---------------- ���콺 Ŭ�� ----------------

        // ���� ���콺 ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            // ���� ī�޶� ���� ���콺 Ŭ���� ���� ray ������ ������
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ray�� ���� ��ü�� Ȯ�� �� ����
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if(hit.transform.name == "Move Plane")
                {
                    player.Hit = hit;
                    player.PlayerMove();
                }
                if (hit.transform.name == "Player")
                {
                    player.ReadyPlayerMove(4);
                }
                print(hit.transform.name + hit.transform.position);
            }
        }

        // ������ ���콺 ��ư�� ������ ��
        if (Input.GetMouseButtonDown(1))
        {
            test1 = true;
        }

        // ---------------- ���콺 �� ----------------
        if (0f != Input.GetAxis("Mouse ScrollWheel"))
        {
            playerCamera.ZoomInOut();
        }

        // ---------------- Ű���� ---------------- 
        if (Input.GetKeyDown("space"))
        {
            playerCamera.PlayerFollow(player.transform);
        }
    }
}


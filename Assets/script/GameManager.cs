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

        // 맵 생성시 필요한 정보 및 블록 생성
        map.X = 10;
        map.Z = 15;
        map.SetMap();
        
        // 미니맵 생성시 필요한 정보 및 블록 생성
        miniMap.Wall = 150;
        miniMap.Monster = 1;
        miniMap.SetMiniMap();
        
        player.SetMovePlane(1000);
        playerCamera.SetCamera();
    }
    void FixedUpdate()
    {
//////////////////////////////////////////////////////////
////////////////////나중에 꼭 수정해야함////////////////////
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

        // ---------------- 마우스 위치 ----------------
        playerCamera.CameraMove();

        // ---------------- 마우스 클릭 ----------------

        // 왼쪽 마우스 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            // 메인 카메라를 통해 마우스 클릭한 곳의 ray 정보를 가져옴
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ray와 닿은 물체를 확인 후 동작
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

        // 오른쪽 마우스 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(1))
        {
            test1 = true;
        }

        // ---------------- 마우스 휠 ----------------
        if (0f != Input.GetAxis("Mouse ScrollWheel"))
        {
            playerCamera.ZoomInOut();
        }

        // ---------------- 키보드 ---------------- 
        if (Input.GetKeyDown("space"))
        {
            playerCamera.PlayerFollow(player.transform);
        }
    }
}


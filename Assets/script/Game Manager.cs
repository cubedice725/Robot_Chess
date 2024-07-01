using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    GameSupporter gameSupporter;
    AStar aStar;
    Monster monster;

    bool fixedEnd = false;
    bool updateEnd = false;

    void Awake()
    {
        // 필요한 컴포넌트 생성
        map = FindObjectOfType<Map>();
        miniMap = FindObjectOfType<MiniMap>();
        player = FindObjectOfType<Player>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        gameSupporter = FindObjectOfType<GameSupporter>();
        aStar = FindObjectOfType<AStar>();
        monster = FindObjectOfType<Monster>();

        // 컴포넌트 Set전 필요한 정보 미리 삽입
        gameSupporter.Width = 100;
        gameSupporter.Length = 100;
        miniMap.Wall = 150;
        miniMap.Monster = 1;

        // 게임에 필요한 Object생성 혹은 설정
        map.SetMap();
        miniMap.SetMiniMap();
        player.SetMovePlane(1000);
        playerCamera.SetCamera();
        aStar.SetAStar();
        monster.SetMoster();
    }
    void FixedUpdate()
    {
        if (updateEnd)
        {
            map.SetCheckBox();
            updateEnd = false;
            fixedEnd = true;
        }
    }
    void Update()
    {
        if (fixedEnd)
        {
            map.CheckBox();
            fixedEnd = false;
        }

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
                    updateEnd = true;
                }
                if (hit.transform.name == "Player")
                {
                    player.ReadyPlayerMove(4);
                }
                //print(hit.transform.name + hit.transform.position);
            }
        }
        
        // 오른쪽 마우스 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(1))
        {
            
        }

        // ---------------- 키보드 ---------------- 
        if (Input.GetKeyDown("space"))
        {
            playerCamera.PlayerFollow(player.transform);
        }
    }
    void LateUpdate()
    {
        // ---------------- 마우스 위치 ----------------
        playerCamera.CameraMove();

        // ---------------- 마우스 휠 ----------------
        if (0f != Input.GetAxis("Mouse ScrollWheel"))
        {
            playerCamera.ZoomInOut();
        }
    }

}


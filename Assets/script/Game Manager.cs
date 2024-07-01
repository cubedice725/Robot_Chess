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
    protected Map map;
    protected Player player;
    protected Monster monster;
    protected MiniMap miniMap;
    protected PlayerCamera playerCamera;
    protected GameSupporter gameSupporter;

    private bool MapCheck = true;
    private bool turnEnd = true;

    private void Awake()
    {
        // 필요한 컴포넌트 생성
        map = FindObjectOfType<Map>();
        player = FindObjectOfType<Player>();
        monster = FindObjectOfType<Monster>();
        miniMap = FindObjectOfType<MiniMap>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        gameSupporter = FindObjectOfType<GameSupporter>();

        // 컴포넌트 Set전 필요한 정보 미리 삽입
        miniMap.Wall = 150;
        miniMap.Monster = 1;
        gameSupporter.MapX = 10;
        gameSupporter.MapZ = 15;

        // 게임에 필요한 Object생성 혹은 설정
        map.SetMap();
        map.SetCheckBox();
        monster.SetMonster();
        miniMap.SetMiniMap();
        player.SetPlayer(1000);
        playerCamera.SetCamera();
    }
    private void Update()
    {
        //맵을 탐색 한번만 작동
        if (MapCheck)
        {
            map.CheckBox();
            MapCheck = false;
        }

        // 한턴이 끝나면 실행
        if (turnEnd)
        {
            miniMap.UpdateMiniMap();
            monster.Move();
            turnEnd = false;
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
                    turnEnd = true;
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
            monster.Move();
        }

        // ---------------- 키보드 ---------------- 
        if (Input.GetKeyDown("space"))
        {
            playerCamera.PlayerFollow(player.transform);
        }
    }
    private void LateUpdate()
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


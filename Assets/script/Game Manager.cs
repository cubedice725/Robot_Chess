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
        // �ʿ��� ������Ʈ ����
        map = FindObjectOfType<Map>();
        player = FindObjectOfType<Player>();
        monster = FindObjectOfType<Monster>();
        miniMap = FindObjectOfType<MiniMap>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        gameSupporter = FindObjectOfType<GameSupporter>();

        // ������Ʈ Set�� �ʿ��� ���� �̸� ����
        miniMap.Wall = 150;
        miniMap.Monster = 1;
        gameSupporter.MapX = 10;
        gameSupporter.MapZ = 15;

        // ���ӿ� �ʿ��� Object���� Ȥ�� ����
        map.SetMap();
        map.SetCheckBox();
        monster.SetMonster();
        miniMap.SetMiniMap();
        player.SetPlayer(1000);
        playerCamera.SetCamera();
    }
    private void Update()
    {
        //���� Ž�� �ѹ��� �۵�
        if (MapCheck)
        {
            map.CheckBox();
            MapCheck = false;
        }

        // ������ ������ ����
        if (turnEnd)
        {
            miniMap.UpdateMiniMap();
            monster.Move();
            turnEnd = false;
        }
        
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
                    turnEnd = true;
                }
                if (hit.transform.name == "Player")
                {
                    player.ReadyPlayerMove(4);
                }
                //print(hit.transform.name + hit.transform.position);
            }
        }
        
        // ������ ���콺 ��ư�� ������ ��
        if (Input.GetMouseButtonDown(1))
        {
            monster.Move();
        }

        // ---------------- Ű���� ---------------- 
        if (Input.GetKeyDown("space"))
        {
            playerCamera.PlayerFollow(player.transform);
        }
    }
    private void LateUpdate()
    {
        // ---------------- ���콺 ��ġ ----------------
        playerCamera.CameraMove();

        // ---------------- ���콺 �� ----------------
        if (0f != Input.GetAxis("Mouse ScrollWheel"))
        {
            playerCamera.ZoomInOut();
        }
    }

}


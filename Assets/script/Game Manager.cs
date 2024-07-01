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
        // �ʿ��� ������Ʈ ����
        map = FindObjectOfType<Map>();
        miniMap = FindObjectOfType<MiniMap>();
        player = FindObjectOfType<Player>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        gameSupporter = FindObjectOfType<GameSupporter>();
        aStar = FindObjectOfType<AStar>();
        monster = FindObjectOfType<Monster>();

        // ������Ʈ Set�� �ʿ��� ���� �̸� ����
        gameSupporter.Width = 100;
        gameSupporter.Length = 100;
        miniMap.Wall = 150;
        miniMap.Monster = 1;

        // ���ӿ� �ʿ��� Object���� Ȥ�� ����
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
                    updateEnd = true;
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
            
        }

        // ---------------- Ű���� ---------------- 
        if (Input.GetKeyDown("space"))
        {
            playerCamera.PlayerFollow(player.transform);
        }
    }
    void LateUpdate()
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


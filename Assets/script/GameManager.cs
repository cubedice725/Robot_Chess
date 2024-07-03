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
    protected GameSupporter gameSupporter;

    private bool MapCheck = true;

    private void Awake()
    {
        // �ʿ��� ������Ʈ ����
        map = FindObjectOfType<Map>();
        player = FindObjectOfType<Player>();
        miniMap = FindObjectOfType<MiniMap>();
        gameSupporter = FindObjectOfType<GameSupporter>();

        // ������Ʈ Set�� �ʿ��� ���� �̸� ����
        miniMap.Wall = 150;
        miniMap.Monster = 10;
        gameSupporter.MapX = 10;
        gameSupporter.MapZ = 15;
        gameSupporter.TurnStart = false;
        gameSupporter.TurnEnd = false;
        // ���ӿ� �ʿ��� Object���� Ȥ�� ����
        map.SetMap();
        map.SetCheckBox();
        miniMap.SetMiniMap();
    }
    private void Update()
    {
        //���� Ž�� �ѹ��� �۵�
        if (MapCheck)
        {
            map.CheckBox();
            miniMap.UpdateMiniMap();
            MapCheck = false;
        }

        // ������ ������ ����
        if (gameSupporter.TurnEnd)
        {
            miniMap.UpdateMiniMap();
            gameSupporter.TurnEnd = false;
        }

    }

}


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
        // 필요한 컴포넌트 생성
        map = FindObjectOfType<Map>();
        player = FindObjectOfType<Player>();
        miniMap = FindObjectOfType<MiniMap>();
        gameSupporter = FindObjectOfType<GameSupporter>();

        // 컴포넌트 Set전 필요한 정보 미리 삽입
        miniMap.Wall = 150;
        miniMap.Monster = 10;
        gameSupporter.MapX = 10;
        gameSupporter.MapZ = 15;
        gameSupporter.TurnStart = false;
        gameSupporter.TurnEnd = false;
        // 게임에 필요한 Object생성 혹은 설정
        map.SetMap();
        map.SetCheckBox();
        miniMap.SetMiniMap();
    }
    private void Update()
    {
        //맵을 탐색 한번만 작동
        if (MapCheck)
        {
            map.CheckBox();
            miniMap.UpdateMiniMap();
            MapCheck = false;
        }

        // 한턴이 끝나면 실행
        if (gameSupporter.TurnEnd)
        {
            miniMap.UpdateMiniMap();
            gameSupporter.TurnEnd = false;
        }

    }

}


using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{
    protected List<GameObject> movePlaneInstList = new List<GameObject>();
    protected GameSupporter gameSupporter;

    private RaycastHit hit;
    public RaycastHit Hit { set{hit = value;} }

    // Player가 움직일때 클릭할 판 준비
    public void SetPlayer(int movePlaneSetCount)
    {
        gameSupporter = FindObjectOfType<GameSupporter>();

        GameObject movePlaneprefab = Resources.Load("Prefab/Move Plane", typeof(GameObject)) as GameObject;
        for (int i = 0; i < movePlaneSetCount; i++)
        {
            movePlaneInstList.Add(Instantiate(movePlaneprefab, transform));
            movePlaneInstList[i].transform.position = new Vector3(0, -100, 0);
            movePlaneInstList[i].SetActive(false);
        }
    }

    // Player가 실제로 움직임
    public void PlayerMove()
    {
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.noting;
        transform.position = new Vector3(hit.transform.position.x, 1, hit.transform.position.z);
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.player;
    }

    // Player이 선택할수 있는 움직임 판 생성
    public void ReadyPlayerMove(int radius)
    {
        // 지름 계산
        int diameter = radius * 2 + 1;
        for (int i = 0; i < diameter * diameter; i++)
        {
            // 마이너스 좌표를 위한 오차 조정
            int width = (i % diameter) - radius;
            int length = (i / diameter) - radius;

            if (width != 0 || length != 0)
            {
                if (Mathf.FloorToInt(pythagoras(width, length)) <= radius)
                {
                    movePlaneInstList[i].transform.localPosition = new Vector3(width, -0.49f, length);
                    movePlaneInstList[i].SetActive(true);
                }
            }
        }
    }

    private float pythagoras(int pythA, int pythB)
    {
        return Mathf.Sqrt((pythA * pythA) + (pythB * pythB));
    }
}
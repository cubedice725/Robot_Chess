using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : AStar
{
    [SerializeField]
    private int movePlaneSetCount = 1000;
    [SerializeField]
    private int radiusMove = 4;
    protected List<GameObject> movePlaneInstList = new List<GameObject>();
    public RaycastHit Hit { get; set; }
    protected override void Awake()
    {
        base.Awake();
        GameObject movePlaneprefab = Resources.Load("Prefab/Move Plane", typeof(GameObject)) as GameObject;
        for (int i = 0; i < movePlaneSetCount; i++)
        {
            movePlaneInstList.Add(Instantiate(movePlaneprefab, transform));
            movePlaneInstList[i].transform.position = new Vector3(0, -100, 0);
            movePlaneInstList[i].SetActive(false);
        }
    }

    public void setPlayerPlane()
    {
        // 지름 계산
        int diameter = radiusMove * 2 + 1;
        for (int i = 0; i < diameter * diameter; i++)
        {
            // 마이너스 좌표를 위한 오차 조정
            int X = (i % diameter) - radiusMove;
            int Z = (i / diameter) - radiusMove;

            if (X != 0 || Z != 0)
            {
                if (Mathf.FloorToInt(Pythagoras(X, Z)) <= radiusMove)
                {
                    movePlaneInstList[i].transform.localPosition = new Vector3(X, -0.49f, Z);
                    movePlaneInstList[i].SetActive(true);
                }
            }
        }
    }

    public void targetPlayerPlane()
    {
        PathFinding();
        Vector3 positionBeforeMoving = transform.position;
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.noting;
        for (int i = 0; i <= radiusMove; i++)
        {
            try
            {
                transform.position = new Vector3((FinalNodeList[i].x - radiusMove) + positionBeforeMoving.x, 1, (FinalNodeList[i].z - radiusMove) + positionBeforeMoving.z);
            }
            catch { }
        }
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.player;
    }

    protected override void SetPathFinding()
    {
        Vector3Int adj = new Vector3Int((int)Hit.transform.position.x - (int)transform.position.x, 0, (int)Hit.transform.position.z - (int)transform.position.z);
        bottomLeft = Vector3Int.zero;

        topRight = new Vector3Int(radiusMove * 2 + 1, 0, radiusMove * 2 + 1);

        startPos = new Vector3Int(radiusMove, 0, radiusMove);

        targetPos = new Vector3Int(startPos.x + adj.x, 0, startPos.z + adj.z);

        // NodeArray의 크기 정해주고, isWall, x, z 대입
        sizeX = topRight.x - bottomLeft.x;
        sizeZ = topRight.z - bottomLeft.z;
        NodeArray = new Node[sizeX, sizeZ];

        for (int i = 0; i < sizeX * sizeZ; i++)
        {
            int map2dObject = 0;
            int map2dX = (int)transform.position.x + ((i / (radiusMove * 2 + 1)) - radiusMove);
            int map2dZ = (int)transform.position.z + ((i % (radiusMove * 2 + 1)) - radiusMove);
            try
            {
                map2dObject = gameSupporter.Map2D[map2dX, map2dZ];
            }
            catch
            {

            }
            bool isWall = false;
            if ((int)GameSupporter.map2dObject.wall == map2dObject || (int)GameSupporter.map2dObject.moster == map2dObject)
            {
                isWall = true;
            }
            NodeArray[i / sizeZ, i % sizeZ] = new Node(isWall, (i / sizeZ) + bottomLeft.x, (i % sizeZ) + bottomLeft.z);
        }

    }
    protected override bool OpenListAddCondition(int checkX, int checkZ)
    {
        // 플레이어가 움직이는 상하좌우 범위를 벗어나지 않고
        if (checkX >= bottomLeft.x && checkX < topRight.x && checkZ >= bottomLeft.z && checkZ < topRight.z)
        {
            // 맵을 벗어나지 않고
            if ((int)transform.position.x + (checkX - radiusMove) < gameSupporter.MapSizeX && (int)transform.position.z + (checkZ - radiusMove) < gameSupporter.MapSizeZ)
            {
                if ((int)transform.position.x + (checkX - radiusMove) >= 0 && (int)transform.position.z + (checkZ - radiusMove) >= 0)
                {
                    // 벽이 아니면서, 닫힌리스트에 없다면
                    if (!NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private float Pythagoras(int pythA, int pythB)
    {
        return Mathf.Sqrt((pythA * pythA) + (pythB * pythB));
    }
}

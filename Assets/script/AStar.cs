using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AStar : MonoBehaviour
{
    protected GameSupporter gameSupporter;
    protected Player player;
    protected Node[,] NodeArray;
    protected Node StartNode, TargetNode, CurNode;

    protected List<Node> FinalNodeList;
    protected List<Node> OpenList, ClosedList;

    protected bool allowDiagonal = true;
    protected bool dontCrossCorner = true;
    protected int sizeX, sizeZ;
    protected Vector3Int bottomLeft, topRight, startPos, targetPos;

    public void SetAStar()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        player = FindObjectOfType<Player>();
    }
    public void PathFinding()
    {
        bottomLeft = Vector3Int.zero;

        topRight = new Vector3Int(gameSupporter.MapX, 0, gameSupporter.MapZ);

        startPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        targetPos = new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z);

        // NodeArray의 크기 정해주고, isWall, x, z 대입
        sizeX = topRight.x - bottomLeft.x;
        sizeZ = topRight.z - bottomLeft.z;
        NodeArray = new Node[sizeX, sizeZ];

        for (int i = 0; i < sizeX * sizeZ; i++)
        {
            bool isWall = false;
            if ((int)GameSupporter.map2dObject.wall == gameSupporter.Map2D[i / sizeZ, i % sizeZ] || (int)GameSupporter.map2dObject.moster == gameSupporter.Map2D[i / sizeZ, i % sizeZ])
            {
                isWall = true;
            }
            NodeArray[i / sizeZ, i % sizeZ] = new Node(isWall, (i / sizeZ) + bottomLeft.x, (i % sizeZ) + bottomLeft.z);
        }

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.z - bottomLeft.z];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                return;
            }

            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z - 1);
                OpenListAdd(CurNode.x + 1, CurNode.z - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.z + 1);
            OpenListAdd(CurNode.x + 1, CurNode.z);
            OpenListAdd(CurNode.x, CurNode.z - 1);
            OpenListAdd(CurNode.x - 1, CurNode.z);
        }
    }

    protected void OpenListAdd(int checkX, int checkZ)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkZ >= bottomLeft.z && checkZ < topRight.z + 1 && !NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal)
            {
                if (NodeArray[CurNode.x - bottomLeft.x, checkZ - bottomLeft.z].isWall && NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall)
                {
                    return;
                }
            }

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner)
            {
                if (NodeArray[CurNode.x - bottomLeft.x, checkZ - bottomLeft.z].isWall || NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall)
                {
                    return;
                }
            }

            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.z - checkZ == 0 ? 10 : 14);

            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.z - TargetNode.z)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }
}

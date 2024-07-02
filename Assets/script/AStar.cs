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

        // NodeArray�� ũ�� �����ְ�, isWall, x, z ����
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

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.z - bottomLeft.z];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // ������
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

            // �֢آע�
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z - 1);
                OpenListAdd(CurNode.x + 1, CurNode.z - 1);
            }

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.z + 1);
            OpenListAdd(CurNode.x + 1, CurNode.z);
            OpenListAdd(CurNode.x, CurNode.z - 1);
            OpenListAdd(CurNode.x - 1, CurNode.z);
        }
    }

    protected void OpenListAdd(int checkX, int checkZ)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkZ >= bottomLeft.z && checkZ < topRight.z + 1 && !NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]))
        {
            // �밢�� ����, �� ���̷� ��� �ȵ�
            if (allowDiagonal)
            {
                if (NodeArray[CurNode.x - bottomLeft.x, checkZ - bottomLeft.z].isWall && NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall)
                {
                    return;
                }
            }

            // �ڳʸ� �������� ���� ������, �̵� �߿� �������� ��ֹ��� ������ �ȵ�
            if (dontCrossCorner)
            {
                if (NodeArray[CurNode.x - bottomLeft.x, checkZ - bottomLeft.z].isWall || NodeArray[checkX - bottomLeft.x, CurNode.z - bottomLeft.z].isWall)
                {
                    return;
                }
            }

            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.z - checkZ == 0 ? 10 : 14);

            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
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

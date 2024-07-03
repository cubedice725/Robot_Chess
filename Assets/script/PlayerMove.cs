using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GameSupporter;
[System.Serializable]
public class PlayerNode
{
    public PlayerNode ParentNode;

    public bool isWall;

    public int x, z, G;
    public PlayerNode(bool _isWall, int _x, int _z) { isWall = _isWall; x = _x; z = _z; }

}

public class PlayerMove : MonoBehaviour
{
    int radiusMove = 4;
    protected GameSupporter gameSupporter;
    protected PlayerNode[,] NodeArray;
    protected PlayerNode StartNode, CurNode;
    public List<PlayerNode> OpenList, ClosedList;
    GameObject closePrefab;
    GameObject OpenPrefab;

    protected bool allowDiagonal = true;
    protected bool dontCrossCorner = false;
    protected int sizeX, sizeZ;
    protected Vector3Int bottomLeft, topRight, startPos;
    protected void Awake()
    {
        closePrefab = Resources.Load("Prefab/close", typeof(GameObject)) as GameObject;
        OpenPrefab = Resources.Load("Prefab/Open", typeof(GameObject)) as GameObject;
        gameSupporter = FindObjectOfType<GameSupporter>();
    }
    protected void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PathFinding();
        }
    }
    public void PathFinding()
    {
        bottomLeft = Vector3Int.zero;

        topRight = new Vector3Int(radiusMove * 2 + 1, 0, radiusMove * 2 + 1);

        startPos = new Vector3Int(radiusMove, 0, radiusMove);

        // NodeArray�� ũ�� �����ְ�, isWall, x, z ����
        sizeX = topRight.x - bottomLeft.x;
        sizeZ = topRight.z - bottomLeft.z;
        NodeArray = new PlayerNode[sizeX, sizeZ];

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
            NodeArray[i / sizeZ, i % sizeZ] = new PlayerNode(isWall, (i / sizeZ) + bottomLeft.x, (i % sizeZ) + bottomLeft.z);
        }
        for (int i = 0; i < sizeX * sizeZ; i++)
        {
            PlayerNode node = NodeArray[i / sizeZ, i % sizeZ];
        }

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];

        OpenList = new List<PlayerNode>() { StartNode };
        ClosedList = new List<PlayerNode>();


        while (OpenList.Count > 0)
        {
            // ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                CurNode = OpenList[i];
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            Instantiate(closePrefab, new Vector3(CurNode.x - radiusMove + 1, 1, CurNode.z - radiusMove + 1), Quaternion.Euler(Vector3.zero));

            //// ������
            //if (OpenList.Count == 0)
            //{
            //    return;
            //}

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.z + 1);
            OpenListAdd(CurNode.x + 1, CurNode.z);
            OpenListAdd(CurNode.x, CurNode.z - 1);
            OpenListAdd(CurNode.x - 1, CurNode.z);
            // �֢آע�
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z + 1);
                OpenListAdd(CurNode.x - 1, CurNode.z - 1);
                OpenListAdd(CurNode.x + 1, CurNode.z - 1);
            }
        }
    }
    protected void OpenListAdd(int checkX, int checkZ)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x && checkZ >= bottomLeft.z && checkZ < topRight.z )
        {
            if (!ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]) && !NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall)

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
                PlayerNode NeighborNode = NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z];
                int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.z - checkZ == 0 ? 10 : 14);
                if (MoveCost >= radiusMove * 14)
                {
                    return;
                }
                // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, ParentNode�� ���� �� ��������Ʈ�� �߰�
                if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.G = MoveCost;
                    NeighborNode.ParentNode = CurNode;

                    OpenList.Add(NeighborNode);
                    Instantiate(OpenPrefab, new Vector3(NeighborNode.x - radiusMove + 1, 1, NeighborNode.z - radiusMove + 1), Quaternion.Euler(Vector3.zero));
                }
            }
        }
    }
}

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

        // NodeArray의 크기 정해주고, isWall, x, z 대입
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

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.z - bottomLeft.z];

        OpenList = new List<PlayerNode>() { StartNode };
        ClosedList = new List<PlayerNode>();


        while (OpenList.Count > 0)
        {
            // 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                CurNode = OpenList[i];
            }

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            Instantiate(closePrefab, new Vector3(CurNode.x - radiusMove + 1, 1, CurNode.z - radiusMove + 1), Quaternion.Euler(Vector3.zero));

            //// 마지막
            //if (OpenList.Count == 0)
            //{
            //    return;
            //}

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.z + 1);
            OpenListAdd(CurNode.x + 1, CurNode.z);
            OpenListAdd(CurNode.x, CurNode.z - 1);
            OpenListAdd(CurNode.x - 1, CurNode.z);
            // ↗↖↙↘
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
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x && checkZ >= bottomLeft.z && checkZ < topRight.z )
        {
            if (!ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z]) && !NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z].isWall)

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
                PlayerNode NeighborNode = NodeArray[checkX - bottomLeft.x, checkZ - bottomLeft.z];
                int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.z - checkZ == 0 ? 10 : 14);
                if (MoveCost >= radiusMove * 14)
                {
                    return;
                }
                // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, ParentNode를 설정 후 열린리스트에 추가
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

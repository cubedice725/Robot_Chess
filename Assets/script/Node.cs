public class Node
{
    public Node ParentNode;

    public bool isWall;
    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, z, G, H;
    public Node(bool _isWall, int _x, int _z) { isWall = _isWall; x = _x; z = _z; }
    public int F { get { return G + H; } }


    
}
public class Node
{
    public Node ParentNode;

    public bool isWall;
    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, z, G, H;
    public Node(bool _isWall, int _x, int _z) { isWall = _isWall; x = _x; z = _z; }
    public int F { get { return G + H; } }


    
}
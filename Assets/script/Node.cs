public class Node
{
    public Node(bool _isWall, int _x, int _z) { isWall = _isWall; x = _x; z = _z; }

    public bool isWall;
    public Node ParentNode;

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, z, G, H;
    public int F { get { return G + H; } }
}
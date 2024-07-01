using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSupporter : MonoBehaviour
{
    public List<List<int>> map2D = new List<List<int>>();
    
    // ∏ ≈©±‚
    private int m_width;
    private int m_length;

    public int Width { get { return m_width; } set { m_width = value; } } // X
    public int Length { get { return m_length; } set { m_length = value; } } // Z

    public enum map2dObject
    { 
        noting = 0,
        wall = 1,
        player = 2,
        moster = 3
    }
}

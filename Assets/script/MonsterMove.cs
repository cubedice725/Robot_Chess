using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterMove : AStar
{
    protected int AttackDistance { get; set; }
    protected int MovingDistance { get; set; }

    protected override void Awake()
    {
        base.Awake();
        AttackDistance = 1;
        MovingDistance = 1;
    }
    
    private void Update()
    {
        if (gameSupporter.TurnStart)
        {
            Move();
            gameSupporter.TurnStart = false;
            gameSupporter.TurnEnd = true;

        }
    }
    public void Move()
    {
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.noting;
        PathFinding();
        if (FinalNodeList.Count < AttackDistance + 3 && FinalNodeList.Count != 0)
        {
            attack();
        }
        else if (FinalNodeList.Count != 0)
        {
            for (int i = 1; i <= MovingDistance; i++)
            {
                transform.position = new Vector3Int(FinalNodeList[i].x, (int)transform.position.y, FinalNodeList[i].z);
            }
        }
        else
        {
            Debug.Log("�÷��̾ ã���� ����");
        }
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.moster;
    }
    public virtual void attack()
    {
        print("����");
    }

}

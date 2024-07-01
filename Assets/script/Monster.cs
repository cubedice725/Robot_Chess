using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : AStar
{
    protected int HP { get; set; }
    protected int ATK { get; set; }
    protected int Defense { get; set; }
    protected int AttackDistance { get; set; }
    protected int MovingDistance { get; set; }

    public virtual void SetMonster()
    {
        SetAStar();
        HP = 4;
        ATK = 1;
        Defense = 1;
        AttackDistance = 1;
        MovingDistance = 1;
    }
    public virtual void Move()
    {
        PathFinding();
        if (FinalNodeList.Count < AttackDistance + 3)
        {
            attack();
        }
        else
        {
            for(int i = 1; i <= MovingDistance; i++)
            {
                transform.position = new Vector3Int(FinalNodeList[i].x, (int)transform.position.y, FinalNodeList[i].z);
            }
        }
    }
    public virtual void attack()
    {
        print("АјАн");
    }

}

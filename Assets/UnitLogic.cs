using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    private Stack<Vector3> path;
    private Vector3 next;
    public SectorController sector;
    public bool moving = false;
    public float health = 1; //make bar
    public bool guinean;
    public GameObject healthin;

    public void Move(Stack<Vector3> p, SectorController target)
    {
        if (p.Count < 1)
            return;
        path = p;
        moving = true;
        sector.friend = null;
        sector = target;
        GetNextMove();
    }

    private void GetNextMove()
    {
        if(path.Count < 1)
        {
            sector.friend = this;
            moving = false;
            return;
        }
        var vec = path.Pop();
        next = new Vector3(vec.x, vec.y, this.transform.position.z);
    }

    private void Update()
    {
        if(moving)
        {
            if(Vector3.Distance(this.transform.position, next) > 0.01f)
            {
                this.transform.position += (next - this.transform.position).normalized * Time.deltaTime;
            } else
            {
                GetNextMove();
            }
        } else
        {
            if(health < 1 && ((sector.ControlLevel == -100 && guinean) || (sector.ControlLevel == 100 && !guinean)))
            {
                UpdateHealth(Time.deltaTime * 0.05f);
            }
        }
    }

    private void UpdateHealth(float add)
    {
        health += add;
        healthin.transform.localScale = new Vector3(0.15f, health, 1);
    }


}

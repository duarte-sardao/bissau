using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : GlobalVars
{
    protected Stack<Vector3> path;
    protected Vector3 next;
    public SectorController sector;
    public bool moving = false;
    public float health = 1; //make bar
    public bool guinean;
    public GameObject healthin;

    public int foreigncount;

    public CampLogic camp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var sect = collision.GetComponent<SectorController>();
        if (sect.foreign)
            foreigncount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var sect = collision.GetComponent<SectorController>();
        if (sect.foreign)
            foreigncount--;
    }

    private void Start()
    {
        sector.friend = this;
        guinean = true;
    }

    public void Move(Stack<Vector3> p, SectorController target)
    {
        if (p.Count < 1)
            return;
        path = p;
        moving = true;

        if(guinean && target.friend != null && !target.friend.moving)
        {
            target.friend.Move(ReversePath(), sector);
        } else
        {
            sector.SetUnit(guinean, null);
        }

        target.SetUnit(guinean, this);
        sector = target;


        GetNextMove();
    }

    private Stack<Vector3> ReversePath()
    {
        var list = path.ToArray();
        var reversed = new Stack<Vector3>();
        reversed.Push(sector.GetCenter(guinean));
        foreach(var pos in list)
        {
            reversed.Push(pos);
        }
        reversed.Pop();
        return reversed;
    }

    private void GetNextMove()
    {
        if(path.Count < 1)
        {
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
                var mult = 1f;
                if (foreigncount > 0) mult = 2f;
                this.transform.position += (next - this.transform.position).normalized * Time.deltaTime * mult;
            } else
            {
                GetNextMove();
            }
        }
    }

    public void UpdateHealth(float add)
    {
        health = Mathf.Clamp(health + add, 0, 1);
        healthin.transform.localScale = new Vector3(0.15f, health, 1);
        if (health == 0)
            Die();
    }

    private void Die()
    {
        if (camp != null)
            camp.queued++;
        Destroy(this.gameObject);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathCalculator : MonoBehaviour
{

    [System.Serializable]
    public struct Sector
    {
        public Sector(SectorController sect, Edge[] edg)
        {
            sector = sect;
            Distance = 99999;
            Edges = edg;
        }

        public SectorController sector;
        public float Distance { get; set; }
        public Edge[] Edges;
    }

    [System.Serializable]
    public struct Edge
    {
        public Edge(int o)
        {
            Out = o;
            Distance = 0;
        }

        public int Out;
        public float Distance { get; set; }
    }

    public Sector[] sectors;

    private void Start()
    {
        for (int j = 0; j < sectors.Length; j++)
        {
            Edge[] edges = sectors[j].Edges;
            for (int i = 0; i < edges.Length; i++)
            {
                sectors[j].Edges[i].Distance = Vector3.Distance(sectors[j].sector.center, sectors[edges[i].Out].sector.center);
            }
        }

        ResetDist();
    }

    public Stack<Vector3> CalculateDistance(SectorController origin, SectorController target, bool guinean)
    {
        if (target.ControlLevel > 100)
            return new Stack<Vector3>();
        int orpos = 0, tarpos = 0;
        for (int i = 0; i < sectors.Length; i++)
        {
            if (sectors[i].sector == origin)
                orpos = i;
            else if (sectors[i].sector == target)
                tarpos = i;
        }

        var investigate = new Queue<int>();
        var path = new Dictionary<int, int>();
        investigate.Enqueue(orpos);
        sectors[orpos].Distance = 0;

        while(investigate.Count > 0)
        {
            int pos = investigate.Dequeue();
            var orlev = sectors[pos].sector.ControlLevel;
            for(int i = 0; i < sectors[pos].Edges.Length; i++)
            {
                var edg = sectors[pos].Edges[i];
                var clev = sectors[edg.Out].sector.ControlLevel;
                //check can move there
                //if current sector is friendly -> nochecks
                //if current sector is hostile -> can go friendly
                var hostile = (guinean && orlev > 0) || (!guinean && orlev < 0);
                if((hostile && orlev*clev > 0) || clev > 100) //means target is same sign ergo unfriendly or its senegal
                {
                    continue;
                }
                //AAAAA
                //calc dist
                float ndist = sectors[pos].Distance + edg.Distance;
                if(sectors[edg.Out].Distance > ndist)
                {
                    sectors[edg.Out].Distance = ndist;
                    path[edg.Out] = pos;
                    if (edg.Out != tarpos)
                    {
                        investigate.Enqueue(edg.Out);
                    }
                }
            }
        }

        ResetDist();
        var pathforunit = new Stack<Vector3>();

        try
        {
            int cur = tarpos;
            while (cur != orpos)
            {
                pathforunit.Push(sectors[cur].sector.GetCenter(guinean));
                cur = path[cur];
            }
        }
        catch (KeyNotFoundException)
        {
            return new Stack<Vector3>();
        }

        return pathforunit;
    }

    private void ResetDist()
    {
        for(int i = 0; i < sectors.Length; i++)
        {
            sectors[i].Distance = 99999;
        }
    }

    public void GetSpotForEnemy(SectorController origin)
    {
        Stack<Vector3> bestpath = null;
        SectorController targ = null;
        float bestval = 999999;
        foreach(Sector sectobj in sectors)
        {
            var sector = sectobj.sector;
            if (sector.enemy != null && sector == origin)
                continue;
            var newval = sector.ControlLevel;
            if (sector.buildings["hospital"].built)
                newval -= 10;
            if (sector.buildings["school"].built)
                newval -= 10;
            if (sector.buildings["farm"].built)
                newval -= 10;
            if (sector.buildings["camp"].built)
                newval -= 50;
            if(sector.friend != null)
            {
                var diff = sector.friend.health - origin.enemy.health;
                if (diff > 0)
                    newval += diff * 10;
                else
                    newval += diff;
            }
            if(newval > bestval) //if value is the same should pick a border one
            {
                var path = CalculateDistance(origin, sector, false);
                if (path.Count < 1)
                    continue;
                bestpath = path;
                targ = sector;
            }
        }
        if(targ != null)
        {
            origin.enemy.Move(bestpath, targ);
        }
    }
}

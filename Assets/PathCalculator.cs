using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathCalculator : GlobalVars
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

    [SerializeField] private Sector[] sectors;

    private void Start()
    {
        for (int j = 0; j < sectors.Length; j++)
        {
            Edge[] edges = sectors[j].Edges;
            for (int i = 0; i < edges.Length; i++)
            {
                sectors[j].Edges[i].Distance = Vector3.Distance(sectors[j].sector.center, sectors[edges[i].Out].sector.center);
            }

            if(!sectors[j].sector.foreign)
            {
                g_sects.Add(sectors[j].sector);
            }
        }
        InvokeRepeating(nameof(CalcOccLevel), 0.01f, 1.0f);
        ResetDist();
    }

    public Stack<Vector3> CalculateDistance(SectorController origin, SectorController target, bool guinean)
    {
        if (target.ControlLevel > 100 || origin == target)
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
                if((hostile && orlev*clev > 0) || clev > 100 || (!guinean && sectors[edg.Out].sector.foreign)) //means target is same sign ergo unfriendly or its senegal
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
            if (sector.enemy != null || sector == origin || sector.foreign)
                continue;
            var newval = sector.ControlLevel;
            //building
            if (sector.buildings["hospital"].built)
                newval -= 10;
            if (sector.buildings["school"].built)
                newval -= 10;
            if (sector.buildings["farm"].built)
                newval -= 10;
            if (sector.buildings["camp"].built)
                newval -= 50;
            //healthdiff if occupied
            if(sector.friend != null)
            {
                var dam_imbal = pt_damage_mult / gn_damage_mult;
                var diff = (sector.friend.health - origin.enemy.health*dam_imbal)*100;
                if (diff > 0 && diff < -20)
                    newval += diff * 10;
                else
                    newval += diff;
            }
            //prio for borders
            foreach(var edge in sectobj.Edges)
            {
                if(!sectors[edge.Out].sector.foreign)
                    newval += (sectors[edge.Out].sector.ControlLevel / 100f);
            }
            if(newval < bestval)
            {
                //Debug.Log(newval);
                var path = CalculateDistance(origin, sector, false);
                if (path.Count < 1)
                    continue;
                bestpath = path;
                bestval = newval;
                targ = sector;
            }
        }
        if(targ != null)
        {
            //Debug.Log(targ);
            origin.enemy.Move(bestpath, targ);
        }
    }

    public void GetRetreatForEnemy(SectorController origin)
    {
        int orpos = 0;
        for (int i = 0; i < sectors.Length; i++)
        {
            if (sectors[i].sector == origin)
                orpos = i;
        }

        var investigate = new Queue<int>();
        var investigated = new List<int>();
        investigate.Enqueue(orpos);
        var pathforunit = new Stack<Vector3>();
        Sector sect = sectors[orpos];

        while (investigate.Count > 0)
        {
            int pos = investigate.Dequeue();
            investigated.Add(pos);
            sect = sectors[pos];
            if(pos != orpos && sect.sector.ControlLevel >= g_fullcontrolp)
            {
                var possible = CalculateDistance(origin, sect.sector, false);
                if (possible.Count > 0)
                {
                    pathforunit = possible;
                    if (Safe(pos))
                        break;
                }
            }
            for (int i = 0; i < sectors[pos].Edges.Length; i++)
            {
                if(!investigated.Contains(sectors[pos].Edges[i].Out))
                    investigate.Enqueue(sectors[pos].Edges[i].Out);
            }
        }

        origin.enemy.Move(pathforunit, sect.sector);
    }

    private bool Safe(int pos)
    {
        bool safe = true;
        for(int i = 0; i < sectors[pos].Edges.Length; i++)
        {
            if (sectors[sectors[pos].Edges[i].Out].sector.ControlLevel < 0)
                return false;
        }
        return safe;
    }

    public bool NeighboursForeign()
    {
        for (int i = 0; i < sectors.Length; i++)
        {
            if (sectors[i].sector.ControlLevel < 0 && !sectors[i].sector.foreign)
            {
                for (int j = 0; j < sectors[i].Edges.Length; j++)
                {
                    var bord = sectors[sectors[i].Edges[j].Out].sector;
                    if (bord.foreign && bord.ControlLevel <= g_fullcontrolp)
                        return true;
                }
            }
        }
        return false;
    }

    private void CalcOccLevel()
    {
        float level = 0;
        float sects = 0;
        for (int i = 0; i < sectors.Length; i++)
        {
            if (!sectors[i].sector.foreign)
            {
                sects++;
                level += sectors[i].sector.ControlLevel * (-0.5f) + 50;
            }
        }
        g_liberationlevel = level / sects;
        //Debug.Log(g_liberationlevel);
    }
}

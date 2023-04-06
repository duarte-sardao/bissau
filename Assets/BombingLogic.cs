using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombingLogic : GlobalVars
{

    [System.Serializable]
    public class Target
    {
        public Target(SectorController sect, string name)
        {
            this.sect = sect;
            this.name = name;
        }

        public SectorController sect;
        public string name;
    }

    public EventSpawner events;
    void Start()
    {
        Invoke(nameof(CheckStart), 10f);
    }

    void CheckStart()
    {
        if (g_liberationlevel >= 25f)
        {
            Invoke(nameof(CalcBomb), 30f);
            events.Spawn("bombing");
        }
        else
            Invoke(nameof(CheckStart), 10f);
    }

    void CalcBomb()
    {
        List<SectorController> terror = new List<SectorController>();
        List<SectorController> units = new List<SectorController>();
        List<SectorController> camps = new List<SectorController>();
        List<Target> buildings = new List<Target>();

        foreach(var sect in g_sects)
        {
            if (sect.ControlLevel < 0)
            {
                terror.Add(sect);
                if (sect.buildings["camp"].built)
                    camps.Add(sect);
                string[] blds = { "hospital", "school", "farm" };
                foreach (var bld in blds)
                {
                    if (sect.buildings[bld].built)
                        buildings.Add(new Target(sect, bld));
                }
            }
            if (sect.Occ(sect.friend))
                units.Add(sect);
        }
        if (camps.Count == 1)
            camps.Clear();

        int terrtarget = terror.Count * g_bombterrormult;
        int unittarget = units.Count * g_bombunitmult + terrtarget;
        int camptarget = camps.Count * g_bombcampmult + unittarget;
        int buildingtarget = buildings.Count * g_bombbuildingmult + camptarget;

        int roll = Random.Range(0, buildingtarget+1);
        if(roll <= terrtarget)
        {
            terror[Random.Range(0, terror.Count)].Bomb("terror");
        }
        else if(roll <= unittarget)
        {
            units[Random.Range(0, units.Count)].Bomb("unit");
        }
        else if (roll <= camptarget)
        {
            camps[Random.Range(0, camps.Count)].Bomb("camp");
        } else
        {
            var targ = buildings[Random.Range(0, buildings.Count)];
            targ.sect.Bomb(targ.name);
        }


        Invoke(nameof(CalcBomb), Random.Range(g_bombrate - 10f, g_bombrate + 10f));
    }
}

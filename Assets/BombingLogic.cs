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

    [SerializeField] private EventSpawner events;
    void Start()
    {
        Invoke(nameof(CheckStart), 10f);
    }

    void CheckStart()
    {
        if (g_liberationlevel >= lev_startbombing)
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
            if (g_protectedunits && sect.Occ(sect.friend) && Random.Range(0, 1) > 0.75f) //chance to skip sector if unit is there
                continue;

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

        float terrtarget = terror.Count * g_bombterrormult;
        float unittarget = units.Count * g_bombunitmult + terrtarget;
        float camptarget = camps.Count * g_bombcampmult + unittarget;
        float buildingtarget = buildings.Count * g_bombbuildingmult + camptarget;

        float roll = Random.Range(0f, buildingtarget);
        SectorController target;
        string type;
        string evt = "air_building";
        if(roll <= terrtarget)
        {
            target = terror[Random.Range(0, terror.Count)];
            type = "terror";
            evt = "air_terror";
        }
        else if(roll <= unittarget)
        {
            target = units[Random.Range(0, units.Count)];
            type = "unit";
            evt = "air_unit";
        }
        else if (roll <= camptarget)
        {
            target = camps[Random.Range(0, camps.Count)];
            type = "camp";
        } else
        {
            var targ = buildings[Random.Range(0, buildings.Count)];
            target = targ.sect;
            type = targ.name;
        }
        target.Bomb(type);
        g_lastbomb = target.sname;
        g_lastbombtype = type;
        StartCoroutine(BombEvent(evt, 1.3f));



        Invoke(nameof(CalcBomb), Mathf.Max(10f, Random.Range(g_bombrate - 10f, g_bombrate + 10f)));
    }

    IEnumerator BombEvent(string evt, float delay) {
        yield return new WaitForSeconds(delay);
        events.Spawn(evt);
    }
}

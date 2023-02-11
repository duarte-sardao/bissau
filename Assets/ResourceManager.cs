using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : GlobalVars
{

    public int politic;
    public int money;
    public int lastpol;
    public int lastmon;
    public List<SectorController> sects = new List<SectorController>();

    public float updatetime;
    private float acctime;

    void Start()
    {
        var all = FindObjectsOfType<SectorController>();
        foreach(var sect in all)
        {
            if(!sect.foreign)
            {
                sects.Add(sect);
            }
        }
    }

    void Update()
    {
        acctime += Time.deltaTime;
        if(acctime >= updatetime)
        {
            GetInputs();
            acctime = 0f;
        }
    }

    void GetInputs()
    {
        lastmon = 5;
        lastpol = 5;
        foreach(var sect in sects)
        {
            if(sect.ControlLevel == -100)
            {
                lastmon += 1;
                lastpol += 1;
                if (sect.buildings["school"].built)
                {
                    lastpol += g_schoolgain;
                    lastmon -= g_buildingcost;
                }
                if (sect.buildings["hospital"].built)
                    lastmon -= g_buildingcost;
                if (sect.buildings["farm"].built)
                    lastmon += g_farmgain;
                if (sect.buildings["camp"].built)
                    lastmon -= g_campcost;
            }
        }
        money = Mathf.Clamp(money+lastmon, -999, 999);
        politic = Mathf.Clamp(politic+lastpol, -999, 999);
    }
}

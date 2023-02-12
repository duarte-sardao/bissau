using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : GlobalVars
{

    [System.Serializable]
    public class Building
    {
        public Building(GameObject mapobj)
        {
            built = false;
            building = false;
            timebuilding = 0;
            obj = mapobj;
        }

        public GameObject obj;
        public float timebuilding { get; set; }
        public bool built;
        public bool building { get; set; }
    }


    public float ControlLevel = 100f;
    public UnitLogic friend = null;
    public UnitLogic enemy = null;
    public GameObject flags;
    public GameObject ptflag;
    public bool foreign = false;

    public Vector3 center;
    public GameObject centerobj;

    public string sname;

    public Dictionary<string, Building> buildings = new Dictionary<string, Building>();
    public GameObject[] buildobj;

    private void Start()
    {
        center = centerobj.transform.position;
        sname = this.gameObject.name;
        char[] spearator = { '-' };
        sname = sname.Split(spearator)[1];

        foreach(var obj in buildobj)
        {
            var bld = new Building(obj);
            bld.obj.SetActive(false);
            buildings.Add(obj.name, bld);
        }


        if (foreign)
            return;
        FlagUpdate();
    }

    private void Update()
    {
        if (foreign)
            return;
        if(occ(friend) && occ(enemy))
        {
            //do battle GRRRR
        }
        else if(occ(friend) && ControlLevel > -100)
        {
            ControlLevel -= Time.deltaTime*10;
            if (ControlLevel < -100)
                ControlLevel = -100;
            FlagUpdate();
        } else if(occ(enemy) && ControlLevel < 100)
        {
            ControlLevel += Time.deltaTime * 10;
            if (ControlLevel > 100)
                ControlLevel = 100;
            FlagUpdate();
        }
        BuildingBuilding();
    }

    private void BuildingBuilding()
    {
        foreach(var key in buildings.Keys) {
            if(buildings[key].building)
            {
                if(ControlLevel != -100)
                {
                    buildings[key].timebuilding = 0f;
                    buildings[key].building = false;
                    return;
                }
                buildings[key].timebuilding += Time.deltaTime;
                if(buildings[key].timebuilding > g_buildtime)
                {
                    buildings[key].timebuilding = 0f;
                    buildings[key].building = false;
                    buildings[key].built = true;
                    buildings[key].obj.SetActive(true);
                }

            }
        }
    }

    private void FlagUpdate()
    {
        if (foreign)
            return;
        if(ControlLevel > 0)
        {
            ptflag.SetActive(true);
        } else
        {
            ptflag.SetActive(false);
        }
        float height = 2.05f + 0.4f * Mathf.Abs(ControlLevel/100);
        flags.transform.localPosition = new Vector3(flags.transform.localPosition.x, height, 0);
    }

    public bool occ(UnitLogic unit)
    {
        return (unit != null && !unit.moving);
    }
}

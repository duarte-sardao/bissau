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
            repairable = false;
            timebuilding = 0;
            obj = mapobj;
        }

        public GameObject obj;
        public float timebuilding { get; set; }
        public bool built;
        public bool repairable;
        public bool building { get; set; }
    }


    public float ControlLevel = 100f;
    public UnitLogic friend = null;
    public UnitLogic enemy = null;
    public GameObject flags;
    public GameObject ptflag;
    public SpriteRenderer pole;
    public bool foreign = false;

    public Vector3 center;
    public GameObject centerobj;
    public Vector3 enemcenter;
    public GameObject enemcenterobj;

    public string sname;

    public Dictionary<string, Building> buildings = new Dictionary<string, Building>();
    public GameObject[] buildobj;

    public bool starter = false;
    public bool bissau = false;

    private GameObject overlay;
    private Bounds bounds;
    private float timeToExplode = 0;
    public GameObject explosion;
    public GameObject bombAnim;

    private void Start()
    {
        center = new Vector3(centerobj.transform.position.x, centerobj.transform.position.y, 1);
        enemcenter = new Vector3(enemcenterobj.transform.position.x, enemcenterobj.transform.position.y, 1);
        sname = this.gameObject.name;
        char[] spearator = { '-' };
        sname = sname.Split(spearator)[1];
        try
        {
            overlay = GameObject.Find("mapoverlays/" + sname);
            overlay.SetActive(false);
        }
        catch (System.Exception) { };
        bounds = GetComponent<PolygonCollider2D>().bounds;

        foreach(var obj in buildobj)
        {
            var bld = new Building(obj);
            bld.obj.SetActive(false);
            buildings.Add(obj.name, bld);
        }

        if(starter)
        {
            ControlLevel = -100;
            buildings["camp"].built = true;
            buildings["camp"].obj.SetActive(true);
            var cmp = transform.GetChild(4).GetChild(0).GetComponent<CampLogic>();
            cmp.queued = 1;
            cmp.accTime = g_unittime;
        }

        if (foreign)
            return;
        FlagUpdate();
    }

    private void Battle()
    {
        friend.UpdateHealth( -pt_damage * Time.deltaTime);
        enemy.UpdateHealth(-gn_damage * Time.deltaTime);
        if((timeToExplode -= Time.deltaTime) < 0)
        {
            DrawExplosions();
            timeToExplode = Random.Range(3, 5);
        }
    }

    private void DrawExplosions()
    {
        Vector3 pos;
        int count = 0;
        do
        {
            pos = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), bounds.center.z);
            count++;
        } while (!bounds.Contains(pos) && count < 6);
        pos = bounds.ClosestPoint(pos);
        Instantiate(explosion, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
    }

    private void UpdateControl(float val)
    {
        if (foreign) return;
        ControlLevel = Mathf.Clamp(ControlLevel + val, -100, 100);
        FlagUpdate();
    }

    private void Update()
    {
        if(Occ(friend) && Occ(enemy))
        {
            Battle();
        }
        else if(Occ(friend))
        {
            if (ControlLevel > -100)
            {
                UpdateControl(-Time.deltaTime * gn_cap);
            } else
            {
                var mult = 1;
                if (buildings["hospital"].built) mult *= 2;
                friend.UpdateHealth(Time.deltaTime * gn_heal * mult);
            }
        } else if(Occ(enemy))
        {
            if (ControlLevel < 100)
            {
                UpdateControl(Time.deltaTime * pt_cap);
            } else
            {
                enemy.UpdateHealth(Time.deltaTime * pt_heal);
            }
        }
        if(bissau && Occ(friend) && ControlLevel > g_fullcontrolg)
            friend.UpdateHealth(-pt_damage * 10 * Time.deltaTime);
        BuildingBuilding();
    }

    private void BuildingBuilding()
    {
        foreach(var key in buildings.Keys) {
            if(buildings[key].building)
            {
                if(ControlLevel > g_fullcontrolg)
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
                    buildings[key].repairable = false;
                    buildings[key].obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    buildings[key].obj.SetActive(true);
                }

            }
        }
    }

    public void DestroyBuilding(string key, int level)
    {
        buildings[key].built = false;
        if(level >= 1)
        {
            buildings[key].repairable = true;
            buildings[key].obj.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f, 1);
            if (level >= 2)
                buildings[key].building = true;
        } else
        {
            buildings[key].obj.SetActive(false);
        }
    }

    public void Bomb(string target)
    {
        Debug.Log("Bombed " + target + " in sector " + sname);
        var pos = new Vector3();
        if (target == "terror")
            pos = this.pole.transform.position;
        else if (target == "unit")
            pos = this.friend.transform.position;
        else
            pos = this.buildings[target].obj.transform.position;
        Instantiate(bombAnim, pos, Quaternion.identity);
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
        if (ControlLevel <= g_fullcontrolg)
            pole.color = new Color32(240, 180, 21, 255);
        else
            pole.color = new Color32(133, 133, 133, 255);
    }

    public bool Occ(UnitLogic unit)
    {
        return (unit != null && !unit.moving);
    }

    public UnitLogic GetUnit(bool guin)
    {
        if (guin)
            return friend;
        return enemy;
    }

    public void SetUnit(bool guin, UnitLogic unit)
    {
        if (guin)
            this.friend = unit;
        else
            this.enemy = unit;
    }

    public Vector3 GetCenter(bool guin)
    {
        if (guin)
            return center;
        return enemcenter;
    }

    public void SetOverlay(bool set)
    {
        overlay.SetActive(set);
    }
}

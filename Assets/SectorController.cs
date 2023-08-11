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
    [SerializeField] private GameObject flags;
    [SerializeField] private GameObject ptflag;
    [SerializeField] private SpriteRenderer pole;
    public bool foreign = false;

    [HideInInspector] public Vector3 center;
    [SerializeField] private GameObject centerobj;
    [HideInInspector] public Vector3 enemcenter;
    [SerializeField] private GameObject enemcenterobj;

    [HideInInspector] public string sname;

    public Dictionary<string, Building> buildings = new Dictionary<string, Building>();
    [SerializeField] private GameObject[] buildobj;

    public bool starter = false;
    public bool bissau = false;

    private GameObject overlay;
    private Bounds bounds;
    private float timeToExplode = 0;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject bombAnim;

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
        friend.UpdateHealth( -multiplier(base_damage, pt_damage_mult) * Time.deltaTime);
        enemy.UpdateHealth(-multiplier(base_damage, gn_damage_mult) * Time.deltaTime);
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
                UpdateControl(-Time.deltaTime * multiplier(base_cap, gn_cap_mult));
            } else
            {
                var mult = 1f;
                if (buildings["hospital"].built)
                {
                    mult *= g_hospitalbonus;
                    if (this.foreign)
                        mult *= 2;
                }
                friend.UpdateHealth(Time.deltaTime * multiplier(base_heal, gn_heal_mult) * mult);
            }
        } else if(Occ(enemy))
        {
            if (ControlLevel < 100)
            {
                UpdateControl(Time.deltaTime * multiplier(base_cap, pt_cap_mult));
            } else
            {
                enemy.UpdateHealth(Time.deltaTime * multiplier(base_heal, pt_heal_mult));
            }
        }
        if(bissau && Occ(friend) && ControlLevel > g_fullcontrolg)
            friend.UpdateHealth(-multiplier(base_damage, pt_damage_mult) * 10 * Time.deltaTime);
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

    public void Bomb(string target)
    {
        Debug.Log("Bombed " + target + " in sector " + sname);
        const float delay = 1f;
        Vector3 pos;
        if (target == "terror")
        {
            pos = this.pole.transform.position;
            Invoke(nameof(Terror), delay);
        }
        else if (target == "unit")
        {
            pos = this.friend.transform.position;
            Invoke(nameof(BombUnit), delay);
        }
        else
        {
            pos = this.buildings[target].obj.transform.position;
            StartCoroutine(DestroyBuilding(target, delay));
        }
        Instantiate(bombAnim, pos, Quaternion.identity);
    }

    private void Terror()
    {
        this.ControlLevel = Mathf.Min(this.ControlLevel + 15 * g_bombintensity, 0f);
        FlagUpdate();
    }

    private void BombUnit()
    {
        this.friend.UpdateHealth(-0.1f * g_bombintensityunits);
    }
    IEnumerator DestroyBuilding(string key, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        buildings[key].built = false;
        if (g_bombintensity <= 2)
        {
            buildings[key].repairable = true;
            buildings[key].obj.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f, 1);
            if (g_bombintensity <= 1)
                buildings[key].building = true;
        }
        else
        {
            buildings[key].obj.SetActive(false);
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

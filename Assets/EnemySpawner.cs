using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : GlobalVars
{
    [SerializeField] private EventSpawner events;
    [SerializeField] private SectorController bissau;
    [SerializeField] private GameObject enemyUnit;
    [SerializeField] private List<GameObject> units;
    private bool spawning = false;
    [SerializeField] private List<int> libLevelsforUnits;

    private double accTime = 0;
    private double nextSpawn = 0;
    private double lastLibLevel;



    private void Start()
    {
        InvokeRepeating(nameof(CheckSpawns), 5f, 5f);
        nextSpawn = libLevelsforUnits[0] * g_resupdatetime * 2;
        lastLibLevel = g_liberationlevel;
    }
    private void CheckSpawns()
    {
        Debug.Log("Units:" + units.Count);
        Debug.Log("Cap:" + g_enemycapacity);
        var increase = false;
        if (spawning)
            return;
        bool captured =  g_liberationlevel > libLevelsforUnits[g_enemycapacity];
        bool timedout = accTime > nextSpawn;
        if (g_enemycapacity < libLevelsforUnits.Count && (captured || timedout))
        {
            nextSpawn = libLevelsforUnits[g_enemycapacity] * g_resupdatetime * 2;
            g_enemycapacity++;
            events.Spawn("new_unit_cap");
            increase = true;
            accTime = 0;
        }
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
                units.RemoveAt(i);
        }
        //Debug.Log(units.Count < g_enemycapacity);
        if (!spawning && units.Count < g_enemycapacity)
        {
            spawning = true;
            Invoke(nameof(Spawn), increase ? 5f : g_ptunittime);
        }
    }

    private void Update()
    {
        if(g_ourunits >= g_enemycapacity && g_liberationlevel == lastLibLevel)
        {
            accTime += Time.deltaTime;
        }
        lastLibLevel = g_liberationlevel;
    }

    private void Spawn()
    {
        if (bissau.enemy != null)
            Invoke(nameof(Spawn), 1f);
        else
        {
            spawning = false;
            var sp = Instantiate(enemyUnit, bissau.enemcenter, Quaternion.identity).GetComponent<UnitLogic>();
            sp.sector = bissau;
            bissau.enemy = sp;
            units.Add(sp.gameObject);
            g_enemylastspawn = sp.sector.sname;
            events.Spawn("new_unit");
        }
    }
}

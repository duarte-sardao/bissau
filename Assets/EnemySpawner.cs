using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : GlobalVars
{
    public EventSpawner events;
    public SectorController bissau;
    public GameObject enemyUnit;
    public List<GameObject> units;
    private bool spawning = false;

    private void Start()
    {
        InvokeRepeating(nameof(CheckSpawns), 5f, 5f);
    }
    private void CheckSpawns()
    {
        if (spawning)
            return;
        var curcap = Mathf.FloorToInt(g_liberationlevel / 15);
        if(curcap > g_enemycapacity)
        {
            g_enemycapacity = curcap;
            events.Spawn("new_unit_cap");
        }
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
                units.RemoveAt(i);
        }
        if (units.Count < g_enemycapacity)
            Invoke(nameof(Spawn), g_ptunittime);
    }

    private void Spawn()
    {
        spawning = true;
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

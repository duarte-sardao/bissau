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

    private void Start()
    {
        InvokeRepeating(nameof(CheckSpawns), 5f, 5f);
    }
    private void CheckSpawns()
    {
        var increase = false;
        if (spawning)
            return;
        if(g_enemycapacity < libLevelsforUnits.Count && g_liberationlevel > libLevelsforUnits[g_enemycapacity])
        {
            g_enemycapacity++;
            events.Spawn("new_unit_cap");
            increase = true;
        }
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
                units.RemoveAt(i);
        }
        //Debug.Log(units.Count < g_enemycapacity);
        if (units.Count < g_enemycapacity)
            Invoke(nameof(Spawn), increase? 5f: g_ptunittime);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : GlobalVars
{
    public EventSpawner events;
    public SectorController bissau;
    public GameObject enemyUnit;
    public List<GameObject> units;
    public int capacity = 0;
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
        if(curcap > capacity)
        {
            capacity = curcap;
            events.Spawn("new_unit_cap").intval = curcap;
        }
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
                units.RemoveAt(i);
        }
        if (units.Count < capacity)
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
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabotage : GlobalVars
{

    private string[] evts = { "sabotage_money", "sabotage_politic", "sabotage_military" };
    [SerializeField] EventSpawner evtman;
    void Start()
    {
        InvokeNext();
    }

    void InvokeNext()
    {
        var time = Random.Range(g_sabotage_freq, g_sabotage_freq * 1.5f);
        Invoke(nameof(DropEvent), time);
    }

    void DropEvent()
    {
        evtman.Spawn(evts[Random.Range(0, evts.Length)]);
        InvokeNext();
    }
}

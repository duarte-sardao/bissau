using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredModifiers : GlobalVars
{
    public void trigger_modifier(string name)
    {
        Invoke(name, 0f);
    }

    public void hearts_and_minds()
    {
        gn_cap_mult -= 0.25f;
        Debug.Log("hearts and minds");
    }

    public void indigenous_recruiting()
    {
        pt_heal_mult += 0.15f;
        g_ptunittime -= 15f;
        pt_cap_mult += 0.1f;
        Debug.Log("indigenous recruits");
    }

    public void bombing_update()
    {
        g_bombterrormult = 2;
        g_bombunitmult = 3;
        g_bombbuildingmult = 2;
        g_bombcampmult = 6;
        Debug.Log("better bombings");
    }

    public void call_other_spinola_events()
    {
        var spwn = FindObjectOfType<EventSpawner>();
        spwn.SpawnDelayed("hearts_and_minds", 30f);
        spwn.SpawnDelayed("indi_recruits", 90f);
        spwn.SpawnDelayed("better_bombs", 140f);
    }

    public void sabo_money()
    {
        g_res.ModifyMoney(-50 * g_sabotage_strength);
    }

    public void sabo_politic()
    {
        g_res.ModifyPolitic(-50 * g_sabotage_strength);
    }

    public void sabo_weaken()
    {
        g_last_sabotage_strength = g_sabotage_strength;
        pt_damage_mult += 0.1f * g_sabotage_strength;
        gn_speed -= 0.1f * g_sabotage_strength;
        var spwn = FindObjectOfType<EventSpawner>();
        spwn.SpawnDelayed("sabotage_mil_fixed", 25f);
    }

    public void sabo_unweaken()
    {
        pt_damage_mult -= 0.1f * g_last_sabotage_strength;
        gn_speed += 0.1f * g_last_sabotage_strength;
    }

    public void new_arms()
    {
        pt_damage_mult += 0.33f;
    }

    public void pt_moto()
    {
        pt_cap_mult += 0.3f;
        pt_speed += 0.4f;
    }

    public void cabral_death()
    {
        g_res.ModifyPolitic(-300);
        gn_cap_mult += 0.2f;
        gn_damage_mult += 0.33f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggeredModifiers : GlobalVars
{
    public void trigger_modifier(string name)
    {
        Invoke(name, 0f);
        Debug.Log("Triggering: " + name);
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
        g_bombterrormult *= 2/5;
        g_bombunitmult *= 2/3;
        g_bombbuildingmult *= 2;
        g_bombcampmult *= 6;
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

    public void green_sea()
    {
        g_res.ModifyPolitic(150);
        g_res.ModifyMoney(-25);
    }

    public void declaration_indie()
    {
        var spwn = FindObjectOfType<EventSpawner>();
        spwn.SpawnDelayed("carnation_revolution", 1f);
    }

    public void carnation()
    {
        var spwn = FindObjectOfType<EventSpawner>();
        spwn.SpawnDelayed("last_event", 1f);
    }

    public void up_declare()
    {
        var spwn = FindObjectOfType<EventSpawner>();
        spwn.SpawnDelayed("indie", 1f);
    }


    public void up_open_senegal()
    {
        GameObject.Find("senegal_overlay").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("26-Senegal (Ziguinchor)").GetComponent<SectorController>().ControlLevel = -100;
        GameObject.Find("29-Senegal (Mandou)").GetComponent<SectorController>().ControlLevel = -100;
        g_borderpol += 2;
        g_bordermon += 2;
    }

    public void guinea_buff()
    {
        g_bordermon += 5;
        g_borderpol += 5;
    }

    public void guinea_hospital()
    {
        var place = GameObject.Find("27-Guinea (Boké)").GetComponent<SectorController>();
        place.buildings["hospital"].building = true;
        place.buildings["hospital"].timebuilding = g_buildtime;
    }

    public void guinea_camp()
    {
        var place = GameObject.Find("27-Guinea (Boké)").GetComponent<SectorController>();
        place.buildings["camp"].building = true;
        place.buildings["camp"].timebuilding = g_buildtime;
    }

    public void foreign_aid()
    {
        g_aidmon += 2;
        g_aidpol += 2;
    }

    public void sweden()
    {
        foreign_aid();
        gn_heal_mult += 0.1f;
    }

    public void yugwar()
    {
        foreign_aid();
        foreign_aid();
    }

    public void soviet_guns()
    {
        china_guns();
        china_guns();
        china_guns();
        g_campcost -= 1;
    }

    public void china_guns()
    {
        gn_damage_mult += 0.1f;
        g_campcost += 2;
    }

    public void cuba()
    {
        gn_damage_mult += 0.05f;
    }

    public void antiair()
    {
        reducebomb();
        g_protectedunits = true;
    }

    public void reducebomb()
    {
        g_bombrate += 15f;
        g_bombintensityunits -= 1;
    }

    public void motorize()
    {
        gn_cap_mult += 0.3f;
        gn_speed += 0.4f;
        gn_damage_mult += 0.05f;
    }

    public void gorilla()
    {
        gn_damage_mult -= 0.5f;
        pt_damage_mult -= 0.55f;
    }

    public void probing()
    {
        gn_damage_mult += 0.1f;
        pt_speed -= 0.1f;
        pt_cap_mult -= 0.1f;
    }

    public void offensive()
    {
        gn_damage_mult += 0.5f;
        pt_damage_mult += 0.55f;
    }

    public void terrain()
    {
        gn_speed += 0.1f;
        pt_damage_mult -= 0.1f;
    }

    public void devaid()
    {
        g_schoolgain += 2;
        g_farmgain += 2;
        g_hospitalbonus += 0.33f;
    }

    public void cheapbuild()
    {
        g_costtobuild -= 20;
    }

    public void aa()
    {
        g_bombintensity -= 1;
    }

    public void aa2()
    {
        aa();
        g_bombcampmult *= 2/6;
    }

    public void warehouses()
    {
        g_farmgain += 4;
        pt_cap_mult -= 0.1f;
    }

    public void politicaledu()
    {
        g_schoolgain += 4;
        gn_cap_mult += 0.1f;
    }

    public void permacom()
    {
        g_sabotage_strength -= 2;
        g_threepol += 5;
        Debug.Log("perma");
    }

    public void elections()
    {
        g_sabotage_freq += 60f;
        g_threepol += 5;
    }

    public void loyal()
    {
        g_sabotage_freq += 60f;
        pt_cap_mult -= 0.1f;
    }

    public void game_end()
    {
        SceneManager.LoadScene("GameEnd");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{

    static public List<SectorController> g_sects = new List<SectorController>();

    static public int g_buildingcost = 1;
    static public int g_schoolgain = 4;
    static public int g_farmgain = 5;
    static public float g_hospitalbonus = 2;
    static public int g_campcost = 10;
    static public float g_buildtime = 10;
    static public int g_costtobuild = 50;

    static public float base_damage = 0.06f;
    static public float pt_damage_mult = 1f;
    static public float gn_damage_mult = 1.2f;

    static public float base_heal = 0.05f;
    static public float pt_heal_mult = 1f;
    static public float gn_heal_mult = 1f;

    static public float gn_speed = 1;
    static public float pt_speed = 1;

    static public float base_cap = 10;
    static public float pt_cap_mult = 1f;
    static public float gn_cap_mult = 1.1f;

    public float multiplier(float basev, float mult) { return basev * Mathf.Max(0.1f, mult); }

    static public float g_unittime = 30f;
    static public float g_ptunittime = 60f;

    static public float g_liberationlevel = 0f;
    static public float g_requiredliblevel = 80f;

    static public float g_fullcontrolg = -100f;
    static public float g_fullcontrolp = 100f;

    static public int g_enemycapacity = 0;
    static public string g_enemylastspawn;

    static public float lev_startbombing = 20f;
    static public float g_bombrate = 30f;
    static public bool g_protectedunits = false;
    static public int g_bombintensity = 3;
    static public int g_bombintensityunits = 3;

    static public float g_bombterrormult = 5;
    static public float g_bombunitmult = 2;
    static public float g_bombbuildingmult = 1;
    static public float g_bombcampmult = 1;

    static public string g_lastbomb = "";
    static public string g_lastbombtype = "";

    static public float g_sabotage_freq = 120f;
    static public int g_sabotage_strength = 4;
    static protected int g_last_sabotage_strength;

    static protected int g_borderpol = 5;
    static protected int g_bordermon = 5;
    static protected int g_aidpol = 5;
    static protected int g_aidmon = 5;
    static protected int g_threepol = 0;

    static public ResourceManager g_res;

    public int gf_buildcost(bool repair)
    {
        return repair ? g_costtobuild / 2 : g_costtobuild;
    }
}

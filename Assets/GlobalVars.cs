using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{

    static public List<SectorController> g_sects = new List<SectorController>();

    static public int g_buildingcost = 1;
    static public int g_schoolgain = 4;
    static public int g_farmgain = 5;
    static public int g_campcost = 10;
    static public float g_buildtime = 10;
    static public int g_costtobuild = 50;

    static public float pt_damage = 0.03f;
    static public float gn_damage = 0.04f;
    static public float pt_heal = 0.05f;
    static public float gn_heal = 0.05f;
    static public float pt_cap = 10;
    static public float gn_cap = 1100;

    static public float g_unittime = 30f;
    static public float g_ptunittime = 60f;

    static public float g_liberationlevel = 0f;

    static public float g_fullcontrolg = -100f;
    static public float g_fullcontrolp = 100f;

    static public int g_enemycapacity = 0;
    static public string g_enemylastspawn;

    static public float g_bombrate = 30f;
    static public int g_bombintensity = 3;
    static public int g_bombterrormult = 5;
    static public int g_bombunitmult = 2;
    static public int g_bombbuildingmult = 1;
    static public int g_bombcampmult = 1;

    public int gf_buildcost(bool repair)
    {
        return repair ? g_costtobuild / 2 : g_costtobuild;
    }
}

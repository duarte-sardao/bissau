using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class ResourceManager : GlobalVars
{

    public int politic;
    public int money;
    [SerializeField] private int lastpol;
    [SerializeField] private int lastmon;

    [SerializeField] private float updatetime;
    private float acctime;

    [SerializeField] private TMPro.TMP_Text polval;
    [SerializeField] private TMPro.TMP_Text polgain;
    [SerializeField] private TMPro.TMP_Text monval;
    [SerializeField] private TMPro.TMP_Text mongain;
    [SerializeField] private TMPro.TMP_Text moninfo;
    [SerializeField] private TMPro.TMP_Text polinfo;

    [SerializeField] private PathCalculator path;

    public LocalizedStringTable m_StringTable;

    private AudioSource clip;


    void Start()
    {
        clip = GetComponent<AudioSource>();
        UpdateBoard();
        g_res = this;
    }

    void Update()
    {
        acctime += Time.deltaTime;
        if(acctime >= updatetime)
        {
            clip.Play();
            GetInputs();
            acctime = 0f;
        }
    }

    void GetInputs()
    {
        //terribleness i could do this with a mere smart string but i dont wanna do that dog itd be such a mess to wrangle
        //just imagine having add to one ok maybe not that difficult but a massive block of formatters for each one unredeable
        //the big gain would be not having to wait for the tick for the language to update but why would you be updating language midgame
        Dictionary<string, int> pol = new Dictionary<string, int>()
        {
            { "BASE_VALUE", 5},
            { "SECTORS", 0},
            { "BUILDINGS", 0},
            { "BORDERING", 0},
            { "AID", 0},
            { "COMM", 0}
        };
        Dictionary<string, int> mon = new Dictionary<string, int>(pol);
        lastmon = 0;
        lastpol = 0;
        foreach(var sect in g_sects)
        {
            if(sect.ControlLevel <= g_fullcontrolg)
            {
                mon["SECTORS"] += 1;
                pol["SECTORS"] += 1;
                if (sect.buildings["school"].built)
                {
                    pol["BUILDINGS"] += g_schoolgain;
                    mon["BUILDINGS"] -= g_buildingcost;
                }
                if (sect.buildings["hospital"].built)
                    mon["BUILDINGS"] -= g_buildingcost;
                if (sect.buildings["farm"].built)
                    mon["BUILDINGS"] += g_farmgain;
                if (sect.buildings["camp"].built)
                    mon["BUILDINGS"] -= g_campcost;
            }
        }
        if (path.NeighboursForeign())
        {
            mon["BORDERING"] += g_bordermon;
            //pol["BORDERING"] += g_borderpol;
        }
        mon["AID"] += g_aidmon;
        //pol["AID"] += g_aidpol;
        pol["COMM"] += g_threepol;
        polinfo.text = "";
        moninfo.text = "";
        string badpol = "";
        string badmon = "";
        foreach (string key in pol.Keys)
        {
            lastmon += mon[key];
            lastpol += pol[key];
            badmon += UpdateInfo(key, mon[key], moninfo);
            badpol += UpdateInfo(key, pol[key], polinfo);
        };
        polinfo.text += badpol;
        moninfo.text += badmon;
        money = Mathf.Clamp(money+lastmon, -999, 999);
        politic = Mathf.Clamp(politic+lastpol, -999, 999);

        UpdateBoard();
    }

    string UpdateInfo(string name, int value, TMPro.TMP_Text txt)
    {
        string badtext = "";
        if(value > 0)
        {
            txt.text += "<color=#008F00>" + m_StringTable.GetTable().GetEntry(name).LocalizedValue + ": +" + value + "\n"; 
        } else if(value < 0)
        {
            badtext = "<color=#8F0000>" + m_StringTable.GetTable().GetEntry(name).LocalizedValue + ": " + value + "\n";
        }
        return badtext;
    }

    public void ModifyMoney(int val)
    {
        money += val;
        monval.text = money.ToString();
    }

    public void ModifyPolitic(int val)
    {
        politic += val;
        polval.text = politic.ToString();
    }

    private void UpdateBoard()
    {
        monval.text = money.ToString();
        polval.text = politic.ToString();
        mongain.text = lastmon.ToString("+0;-#");
        polgain.text = lastpol.ToString("+0;-#");
    }
}

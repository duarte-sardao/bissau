using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        UpdateBoard();
    }

    void Update()
    {
        acctime += Time.deltaTime;
        if(acctime >= updatetime)
        {
            GetInputs();
            acctime = 0f;
        }
    }

    void GetInputs()
    {
        Dictionary<string, int> pol = new Dictionary<string, int>()
        {
            { "Base Value", 5},
            { "Sectors", 0},
            { "Buildings", 0},
            { "Bordering Ally", 0},
        };
        Dictionary<string, int> mon = new Dictionary<string, int>()
        {
            { "Base Value", 5},
            { "Sectors", 0},
            { "Buildings", 0},
            { "Bordering Ally", 0},
        };
        lastmon = 0;
        lastpol = 0;
        foreach(var sect in g_sects)
        {
            if(sect.ControlLevel <= g_fullcontrolg)
            {
                mon["Sectors"] += 1;
                pol["Sectors"] += 1;
                if (sect.buildings["school"].built)
                {
                    pol["Buildings"] += g_schoolgain;
                    mon["Buildings"] -= g_buildingcost;
                }
                if (sect.buildings["hospital"].built)
                    mon["Buildings"] -= g_buildingcost;
                if (sect.buildings["farm"].built)
                    mon["Buildings"] += g_farmgain;
                if (sect.buildings["camp"].built)
                    mon["Buildings"] -= g_campcost;
            }
        }
        if (path.NeighboursForeign())
        {
            mon["Bordering Ally"] += 5;
            pol["Bordering Ally"] += 5;
        }
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
            txt.text += "<color=green>" + name + ": +" + value + "\n"; 
        } else if(value < 0)
        {
            badtext = "<color=red>" + name + ": " + value + "\n";
        }
        return badtext;
    }

    private void UpdateBoard()
    {
        monval.text = money.ToString();
        polval.text = politic.ToString();
        mongain.text = lastmon.ToString("+0;-#");
        polgain.text = lastpol.ToString("+0;-#");
    }
}

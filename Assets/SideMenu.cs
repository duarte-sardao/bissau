using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : ResourceManager
{
    [SerializeField] private bool open;
    [SerializeField] private SectorController sector;
    [SerializeField] private Canvas sidecanvas;
    [SerializeField] private TMPro.TMP_Text nametext;

    [SerializeField] private TMPro.TMP_Text campcost;
    [SerializeField] private TMPro.TMP_Text hospcost;
    [SerializeField] private TMPro.TMP_Text schoolgain;
    [SerializeField] private TMPro.TMP_Text schoolcost;
    [SerializeField] private TMPro.TMP_Text farmgain;
    [SerializeField] private TMPro.TMP_Text cost;
    [SerializeField] private TMPro.TMP_Text rcost;

    private readonly string[] builds = { "camp", "hospital", "school", "farm" };
    [SerializeField] private Button[] buildButts;
    [SerializeField] private TMPro.TMP_Text[] buildProg;
    [SerializeField] private Image[] buildCheck;
    [SerializeField] private GameObject[] buildDamage;

    public void Start()
    {
        sidecanvas.enabled = false;
    }

    public void SetupUI()
    {
        nametext.text = sector.sname;
        campcost.text = "-" + g_campcost;
        hospcost.text = "-" + g_buildingcost;
        schoolcost.text = "-" + g_buildingcost;
        schoolgain.text = "+" + g_schoolgain;
        farmgain.text = "+" + g_farmgain;
        cost.text = g_costtobuild.ToString();
        rcost.text = gf_buildcost(true).ToString();
    }

    public void Open(SectorController sector)
    {
        if (sector.foreign)
            return;
        if(this.sector != null)
            this.sector.SetOverlay(false);
        this.sector = sector;
        open = true;
        SetupUI();
        sector.SetOverlay(true);
    }

    public void Close()
    {
        open = false;
        sidecanvas.enabled = false;
        sector.SetOverlay(false);
    }

    private void Update()
    {
        var newx = this.transform.position.x;
        if (open && this.transform.position.x > -1.34f)
        {
            newx -= Time.deltaTime * 50;
            if(newx <= -1.35f)
            {
                sidecanvas.enabled = true;
            }
        } else if(!open && this.transform.position.x < 4.75)
        {
            newx += Time.deltaTime * 50;
        }
        this.transform.position = new Vector3(Mathf.Clamp(newx, -1.35f, 4.75f), 0, -1);
        UpdateBuildTimes();
    }

    private void UpdateBuildTimes()
    {
        try
        {
            for (int i = 0; i < 4; i++)
            {
                var buildd = sector.buildings[builds[i]];
                buildButts[i].gameObject.SetActive(false);
                buildProg[i].gameObject.SetActive(false);
                buildCheck[i].gameObject.SetActive(false);
                buildDamage[i].SetActive(false);
                //some text saying its repairable here
                //and mention repair cost (fixed, just say its half)
                if (buildd.built)
                {
                    buildCheck[i].gameObject.SetActive(true);
                }
                else if (buildd.building)
                {
                    buildProg[i].gameObject.SetActive(true);
                    buildProg[i].text = Mathf.RoundToInt(buildd.timebuilding / g_buildtime * 100) + "%";
                }
                else
                {
                    if (sector.ControlLevel <= g_fullcontrolg && money >= gf_buildcost(buildd.repairable))
                    {
                        buildButts[i].gameObject.SetActive(true);
                    }
                    if (buildd.repairable)
                        buildDamage[i].SetActive(true);
                }
            }
        }
        catch (System.NullReferenceException) { };
    }

    public void Build(string building)
    {
        //money -= gf_buildcost(sector.buildings[building].repairable);
        ModifyMoney(-gf_buildcost(sector.buildings[building].repairable));
        sector.buildings[building].building = true;
    }
}

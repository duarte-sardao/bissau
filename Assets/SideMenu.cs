using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : ResourceManager
{
    public bool open;
    public SectorController sector;
    public Canvas sidecanvas;
    public TMPro.TMP_Text nametext;

    public TMPro.TMP_Text campcost;
    public TMPro.TMP_Text hospcost;
    public TMPro.TMP_Text schoolgain;
    public TMPro.TMP_Text schoolcost;
    public TMPro.TMP_Text farmgain;
    public TMPro.TMP_Text cost;

    private readonly string[] builds = { "camp", "hospital", "school", "farm" };
    public Button[] buildButts;
    public TMPro.TMP_Text[] buildProg;
    public Image[] buildCheck;

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
                if (buildd.built)
                {
                    buildCheck[i].gameObject.SetActive(true);
                }
                else if (buildd.building)
                {
                    buildProg[i].gameObject.SetActive(true);
                    buildProg[i].text = Mathf.RoundToInt(buildd.timebuilding / g_buildtime * 100) + "%";
                }
                else if(sector.ControlLevel <= g_fullcontrolg && money >= 50)
                {
                    buildButts[i].gameObject.SetActive(true);
                }
            }
        }
        catch (System.NullReferenceException) { };
    }

    public void Build(string building)
    {
        money -= 50;
        sector.buildings[building].building = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampLogic : GlobalVars
{
    [SerializeField] private GameObject bar;
    private SpriteRenderer spr;
    private SpriteRenderer cmp;
    public float accTime;
    public int queued;
    [SerializeField] private GameObject unit;
    private SectorController sector;

    private void Start()
    {
        sector = transform.parent.parent.GetComponent<SectorController>();
        spr = bar.GetComponentInChildren<SpriteRenderer>();
        cmp = GetComponent<SpriteRenderer>();
        queued = 1;
    }

    private void Update()
    {
        if (cmp.color.a < 1)
            accTime = 0;
        else if(queued > 0 && sector.ControlLevel == -100)
        {
            accTime = Mathf.Clamp(accTime + Time.deltaTime, 0, g_unittime);
            if (accTime >= g_unittime && sector.friend == null)
                Spawn();
        }
        Bar();
    }

    private void Spawn()
    {
        var sp = Instantiate(unit, sector.center, Quaternion.identity).GetComponent<UnitLogic>();
        sp.sector = sector;
        sp.camp = this;
        sector.friend = sp;
        accTime = 0;
        queued--;
    }

    private void Bar()
    {
        if(queued < 1)
        {
            bar.transform.localScale = new Vector3(0, 1, 1);
        } else if(g_unittime == accTime && sector.friend != null)
        {
            bar.transform.localScale = new Vector3(1, 1, 1);
            spr.color = Color.red;
        } else
        {
            bar.transform.localScale = new Vector3((g_unittime - accTime) / g_unittime, 1, 1);
            if (sector.ControlLevel == g_fullcontrolg)
                spr.color = Color.white;
            else
                spr.color = Color.red;
        }
    }

    private void OnDisable()
    {
        accTime = 0;
    }
}

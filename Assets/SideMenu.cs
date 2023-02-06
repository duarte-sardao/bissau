using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenu : MonoBehaviour
{
    public bool open;
    public SectorController sector;
    public Canvas sidecanvas;
    public TMPro.TMP_Text nametext;

    public void Start()
    {
        sidecanvas.enabled = false;
    }

    public void SetupUI()
    {
        nametext.text = sector.sname;
    }

    public void Open(SectorController sector)
    {
        if (sector.foreign)
            return;
        this.sector = sector;
        open = true;
        SetupUI();
    }

    public void Close()
    {
        open = false;
        sidecanvas.enabled = false;
    }

    private void Update()
    {
        var newx = this.transform.position.x;
        if (open && this.transform.position.x > -1.35f)
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
    }
}

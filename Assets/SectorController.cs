using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : MonoBehaviour
{


    public float ControlLevel = 100f;
    public UnitLogic friend = null;
    public UnitLogic enemy = null;
    public GameObject flags;
    public GameObject ptflag;
    public bool foreign = false;

    public Vector3 center;
    public GameObject centerobj;

    private void Start()
    {
        center = centerobj.transform.position;
        if (foreign)
            return;
        FlagUpdate();
    }

    private void Update()
    {
        if (foreign)
            return;
        if(friend != null && enemy != null)
        {
            //do battle GRRRR
        }
        else if(friend != null && ControlLevel > -100)
        {
            ControlLevel -= Time.deltaTime*10;
            if (ControlLevel < -100)
                ControlLevel = -100;
            FlagUpdate();
        } else if(enemy != null && ControlLevel < 100)
        {
            ControlLevel += Time.deltaTime * 10;
            if (ControlLevel > 100)
                ControlLevel = 100;
            FlagUpdate();
        }
    }

    private void FlagUpdate()
    {
        if (foreign)
            return;
        if(ControlLevel > 0)
        {
            ptflag.SetActive(true);
        } else
        {
            ptflag.SetActive(false);
        }
        float height = 2.05f + 0.4f * Mathf.Abs(ControlLevel/100);
        flags.transform.localPosition = new Vector3(flags.transform.localPosition.x, height, 0);
    }
}

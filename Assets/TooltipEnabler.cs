using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipEnabler : MonoBehaviour
{

    public GameObject tooltip;

    private void Start()
    {
        tooltip.SetActive(false);
    }

    private void OnMouseEnter()
    {
        tooltip.SetActive(true);
    }

    private void OnMouseOver()
    {
        //Debug.Log(Input.mousePosition);
        tooltip.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tooltip.transform.position = new Vector3(tooltip.transform.position.x, tooltip.transform.position.y, 0);
    }

    private void OnMouseExit()
    {
        tooltip.SetActive(false);
    }
}

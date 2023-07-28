using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiberationButton : GlobalVars
{
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void OnEnable()
    {
        img.fillAmount = g_liberationlevel / 80f;
    }
}

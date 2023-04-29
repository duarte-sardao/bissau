using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class EventLogic : DragUI
{
    [SerializeField] private string triggered_evt = null;
    protected override void Start()
    {
        base.Start();
        Invoke(nameof(Close), 15f);
    }
    public void Close()
    {
        if (triggered_evt != null && triggered_evt.Length > 0)
            trigger_modifier(triggered_evt);
        Destroy(this.gameObject);
    }
}

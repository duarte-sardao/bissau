using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class EventLogic : DragUI
{
    protected override void Start()
    {
        base.Start();
        Invoke(nameof(Close), 15f);
    }
    public void Close()
    {
        Destroy(this.gameObject);
    }
}

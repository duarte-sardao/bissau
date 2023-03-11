using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class EventLogic : MonoBehaviour
{
    //public int intval;
    //public string stringval;
    //public LocalizeStringEvent[] evs;
   //ublic List<LocalizedString> strs = new List<LocalizedString>();


    void Start()
    {
        Invoke(nameof(Close), 15f);
    }
   /**  foreach (var ev in evs)
            strs.Add(ev.StringReference);
        //strs = this.GetComponentsInChildren<LocalizedString>();
        //this.GetComponentsInChildren<>
        //refresh text to update vals
    }

    public void UpdateInt(int v)
    {
        intval = v;
        UpdateTexts();
    }

    public void UpdateString(string v)
    {
        stringval = v;
        UpdateTexts();
    }

    void UpdateTexts()
    {
        foreach(var str in strs)
            str.RefreshString();
    }**/

    public void Close()
    {
        Destroy(this.gameObject);
    }
}

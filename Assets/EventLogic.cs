using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLogic : MonoBehaviour
{
    public int intval;
    public string stringval;


    void Start()
    {
        Invoke(nameof(Close), 15f);
        //this.GetComponentsInChildren<>
        //refresh text to update vals
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}

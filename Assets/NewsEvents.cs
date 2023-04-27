using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsEvents : GlobalVars
{
    [System.Serializable]
    public struct EvtPair
    {
        public string name;
        public float limit;
    }



    [SerializeField] private List<EvtPair> evts = new List<EvtPair>();
    [SerializeField] private EventSpawner events;
    private int pos;

    private void Start()
    {
        evts.Sort((s1, s2) => s1.limit.CompareTo(s2.limit));
        InvokeRepeating(nameof(CheckSpawn), 3f, 3f);
    }

    void CheckSpawn()
    {
        if (pos >= evts.Count)
            Destroy(this.gameObject);
        if(g_liberationlevel >= evts[pos].limit)
        {
            events.Spawn(evts[pos].name);
            pos++;
        }
    }
}

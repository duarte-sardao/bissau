using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringObjDic : SerializableDictionary<string, GameObject> { }

public class EventSpawner : MonoBehaviour
{
    public StringObjDic events;

    public EventLogic Spawn(string name)
    {
        return Instantiate(events[name], this.transform).GetComponent<EventLogic>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringObjDic : SerializableDictionary<string, GameObject> { }

public class EventSpawner : MonoBehaviour
{
    [SerializeField] private StringObjDic events;

    public EventLogic Spawn(string name)
    {
        return Instantiate(events[name], this.transform).GetComponent<EventLogic>();
    }
    public void SpawnDelayed(string name, float delay)
    {
        StartCoroutine(SpDelay(name, delay));
    }
    private IEnumerator SpDelay(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        Spawn(name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glimmer : MonoBehaviour
{
    private SpriteRenderer sprite;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Debug.Log(Mathf.Sin(Time.realtimeSinceStartup * 3) / 3f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (Mathf.Sin(Time.realtimeSinceStartup*3)+1)/3f);
    }
}

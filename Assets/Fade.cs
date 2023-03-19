using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        sprite.color = new Color(1, 1, 1, sprite.color.a - Time.deltaTime);
        if (sprite.color.a <= 0)
            Destroy(gameObject);
    }
}

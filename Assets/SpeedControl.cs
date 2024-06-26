using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    private List<float> speeds = new List<float>(){ 0.5f, 1, 2, 5 };
    [SerializeField] private List<Sprite> gearSprites;
    [SerializeField] private SpriteRenderer gear;
    private int curSpeed = 1;
    private float acctime = 0;
    private AudioSource clip;

    private void Start()
    {
        clip = GetComponent<AudioSource>();
        UpdateSpeed();
    }

    public void AddSpeed()
    {
        clip.Play();
        if (curSpeed < speeds.Count - 1)
            curSpeed++;
        UpdateSpeed();
    }

    public void RemoveSpeed()
    {
        clip.Play();
        if (curSpeed > 0)
            curSpeed--;
        UpdateSpeed();
    }

    public void UpdateSpeed()
    {
        Time.timeScale = speeds[curSpeed];
        gear.sprite = gearSprites[curSpeed];
    }

    public void PauseUnPause()
    {
        clip.Play();
        if (Time.timeScale == 0f)
            UpdateSpeed();
        else
            Time.timeScale = 0;
    }

    private void Update()
    {
        if(Time.timeScale == 0)
        {
            acctime += Time.unscaledDeltaTime;
            acctime %= 1;
            if (acctime < 0.5)
                gear.sprite = gearSprites[gearSprites.Count-1];
            else
                gear.sprite = gearSprites[curSpeed];
        }
        if (Input.GetKeyDown(KeyCode.Space))
            PauseUnPause();
    }
}

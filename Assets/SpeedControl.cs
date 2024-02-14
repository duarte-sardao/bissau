using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    private List<float> speeds = new List<float>(){ 0.5f, 1, 2, 5 };
    private int curSpeed = 1;
    
    public void AddSpeed()
    {
        if (curSpeed < speeds.Count - 1)
            curSpeed++;
        UpdateSpeed();
    }

    public void RemoveSpeed()
    {
        if (curSpeed > 0)
            curSpeed--;
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        Time.timeScale = speeds[curSpeed];
    }

    public void PauseUnPause()
    {
        if (Time.timeScale == 0f)
            UpdateSpeed();
        else
            Time.timeScale = 0;
    }
}

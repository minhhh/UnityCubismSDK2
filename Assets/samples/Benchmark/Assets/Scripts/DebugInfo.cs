﻿using UnityEngine;

public class DebugInfo : MonoBehaviour {
    // FPS
    private float oldTime;
    private int frame = 0;
    private float frameRate = 0f;
    private const float INTERVAL = 0.5f; 
  

    void Start() 
    {
        oldTime = Time.realtimeSinceStartup;
    }


    void Update()
    {
        // FPS
        frame++;
        float time = Time.realtimeSinceStartup - oldTime;
        if (time >= INTERVAL)
        {
            frameRate = frame / time;
            oldTime = Time.realtimeSinceStartup;
            frame = 0;
        }
    }


    public float getFrameRate()
    {
        return frameRate;
    }


    void OnGUI()
    {
        GUI.Label(new Rect(25,25, 160, 20), "FPS : " + frameRate.ToString());
    }
}
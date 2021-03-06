using UnityEngine;
using System;
using live2d;

[ExecuteInEditMode]
public class SimpleModelLipSync: MonoBehaviour 
{
    public TextAsset mocFile;
    public Texture2D[] textureFiles;

    private float scaleVolume = 20;
    private bool smoothing = true;
    private bool useMic = false;
    private float lastVolume = 0;

	private Live2DModelUnity live2DModel;
    private Matrix4x4 live2DCanvasPos;
   

	void Start () 
	{
        Live2D.init();

        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

        for (int i = 0; i < textureFiles.Length; i++)
        {
            live2DModel.setTexture(i, textureFiles[i]);
        }

        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);
	}


    void Update()
    {
        if (live2DModel == null) return;
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

        if (!Application.isPlaying)
        {
            live2DModel.update();
            live2DModel.draw();
            return;
        }
        float volume = 0;

        if (smoothing)
        {
            float currentVolume = GetCurrentVolume(GetComponent<AudioSource>());

            if (Mathf.Abs(lastVolume - currentVolume) < 0.2f)
            {
                volume = lastVolume * 0.9f + currentVolume * 0.1f;
            }
            else if (lastVolume - currentVolume > 0.2f)
            {
                volume = lastVolume * 0.7f + currentVolume * 0.3f;
            }
            else
            {
                volume = lastVolume * 0.2f + currentVolume * 0.8f;
            }
            lastVolume = volume;
        }
        else
        {
            volume = GetCurrentVolume(GetComponent<AudioSource>());
        }


        live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", volume * scaleVolume);


        live2DModel.update();
    }
	
	
	void OnRenderObject()
	{
        if (live2DModel == null) return;
		live2DModel.draw();
	}

    private float GetCurrentVolume(AudioSource audio)
    {
        float[] data = new float[256];
        float sum = 0;
        audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            sum += Mathf.Abs(s);
        }
        return sum / 256.0f;
    }

    void OnGUI()
    {
        if (!Application.isPlaying) return;

        String playMessage;
        if (useMic)
        {
            playMessage = "Record";
        }
        else
        {
            playMessage = "Play";
        }
        if (GUI.Button(new Rect(10, 10, 200, 30), playMessage))
        {
            if (GetComponent<AudioSource>().isPlaying)
            {
                if (useMic)
                {
                    var deviceName = Microphone.devices[0];
                    Microphone.End(deviceName);
                }
                GetComponent<AudioSource>().Stop();
            }
            GetComponent<AudioSource>().Play();
        }

        if (GUI.Button(new Rect(10, 50, 200, 30), "Smooth"))
        {
            smoothing = !smoothing;
        }

        String smoothingMessage;

        if (smoothing)
        {
            smoothingMessage = "Smooth : ON";            
        }
        else
        {
            smoothingMessage = "Smooth : OFF";            
        }
        GUI.Label(new Rect(10, 90, 200, 30), smoothingMessage);  

        GUI.Label(new Rect(10, 130, 50, 30), "Scale : ");
        scaleVolume=GUI.HorizontalSlider(new Rect(60, 135, 150, 30), scaleVolume, 1, 100);

        if (!useMic)
        {
            if (GUI.Button(new Rect(10, 170, 200, 30), "Connect Mic"))
            {
                if (SetupMic(GetComponent<AudioSource>()))
                {
                    useMic = true;
                }
            }
        }
    }

    public bool SetupMic(AudioSource audio)
    {
        const int RECORDING_SEC = 20;
        const int FREQENCY = 44100;

        if (Microphone.devices.Length == 0)
        {
            Debug.Log("Mic is not found.");
            return false;
        }
       
        var deviceName = Microphone.devices[0];

        audio.clip = Microphone.Start(deviceName, false, RECORDING_SEC, FREQENCY);

        return true;
    }
}
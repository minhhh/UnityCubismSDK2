using UnityEngine;
using System;
using live2d;

[ExecuteInEditMode]
public class SimpleModelStreamingAssets: MonoBehaviour
{
    private Live2DModelUnity live2DModel;
    private Matrix4x4 live2DCanvasPos;

    public String MODEL_PATH = "haru.moc";
    public String[] TEXTURE_PATHS = { 
        "haru.1024/texture_00.png", 
        "haru.1024/texture_01.png",
        "haru.1024/texture_02.png"
    };

    void Start ()
    {
        Live2D.init ();

        String dataDir = Application.streamingAssetsPath + "/";
        live2DModel = Live2DModelUnity.loadModel (dataDir + MODEL_PATH);

        for (int i = 0; i < TEXTURE_PATHS.Length; i++) {
            var texture = TextureUtil.LoadTexture (dataDir + TEXTURE_PATHS [i]);

            
            live2DModel.setTexture (i, texture);
        }

        float modelWidth = live2DModel.getCanvasWidth ();
        live2DCanvasPos = Matrix4x4.Ortho (0, modelWidth, modelWidth, 0, -50.0f, 50.0f);
    }

	
    void OnRenderObject ()
    {
        if (live2DModel == null)
            return;
        live2DModel.setMatrix (transform.localToWorldMatrix * live2DCanvasPos);

        if (!Application.isPlaying) {
            live2DModel.update ();
            live2DModel.draw ();
            return;
        }
		
        double t = (UtSystem.getUserTimeMSec () / 1000.0) * 2 * Math.PI;
        live2DModel.setParamFloat ("PARAM_ANGLE_X", (float)(30 * Math.Sin (t / 3.0)));
		
		
        live2DModel.update ();
        live2DModel.draw ();
    }
}
using UnityEngine;
using System;
using live2d;

[ExecuteInEditMode]
public class SimpleModelTextureAtlas : MonoBehaviour
{
    public TextAsset layoutFile;
    public TextAsset mocFile;
    public Texture2D textureFile;
	
    private Live2DModelUnity _live2DModel;
    private TextureAtlasLayout _layout;
    private Matrix4x4 _live2DCanvasPos;

    void Start ()
    {
        Live2D.init ();

        _Load ();
    }


    void _Load ()
    {
        _live2DModel = Live2DModelUnity.loadModel (mocFile.bytes);


        _live2DModel.setTexture (0, textureFile);

        _layout = TextureAtlasLayout.loadJson (layoutFile.bytes);

        var frames = _layout.getFrames ();
        int n = frames.Count;
        for (int i = 0; i < n; i++) {
            var item = frames [i];
            _SetTextureMap (i, 0, _layout.width, _layout.height, item.srcWidth, item.srcHeight, item.x, item.y, item.trimX, item.trimY);
        }


        float modelWidth = _live2DModel.getCanvasWidth ();
        _live2DCanvasPos = Matrix4x4.Ortho (0, modelWidth, modelWidth, 0, -50.0f, 50.0f);
    }

    private void _SetTextureMap (int srcIndex, int dstIndex, int atlasW, int atlasH, int srcW, int srcH, int x, int y, int trimX, int trimY)
    {
        float scaleX = (float)srcW / atlasW;
        float scaleY = (float)srcH / atlasH;

        float offsetX = (float)(x - trimX) / atlasW;
        float offsetY = (float)(y - trimY) / atlasH;

        _live2DModel.setTextureMap (srcIndex, dstIndex, scaleX, scaleY, offsetX, offsetY);
    }


    void Update ()
    {
        if (_live2DModel == null)
            _Load ();
        _live2DModel.setMatrix (transform.localToWorldMatrix * _live2DCanvasPos);

        if (!Application.isPlaying) {
            _live2DModel.update ();
            return;
        }

        double timeSec = UtSystem.getUserTimeMSec () / 1000.0;
        double t = timeSec * 2 * Math.PI;
        _live2DModel.setParamFloat ("PARAM_ANGLE_X", (float)(30f * Math.Sin (t / 3.0)));


        _live2DModel.update ();
    }

	
    void OnRenderObject ()
    {
        if (_live2DModel == null) {
            _Load ();
        }
            
        _live2DModel.draw ();
    }
}
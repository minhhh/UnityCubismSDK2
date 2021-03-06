/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using System;
using System.Collections.Generic;
using live2d;

public class TextureAtlasLayout
{
    public string image;
    public int width;
    public int height;

    private List<TextureLayout> frames=new List<TextureLayout>();




    public TextureAtlasLayout()
    {
    }

    public List<TextureLayout> getFrames()
    {
        return frames;
    }

    public static TextureAtlasLayout loadJson(byte[] buf)
    {
        return loadJson(System.Text.Encoding.GetEncoding("UTF-8").GetString(buf));
    }


    public static TextureAtlasLayout loadJson(string buf)
    {
        return loadJson(buf.ToCharArray());
    }

    public static TextureAtlasLayout loadJson(char[] buf)
    {
        Value json = Json.parseFromBytes(buf);

        return loadJson(json);
    }

    //e.g)
    //{"frames": [
    //{
    //    "filename": "texture_00.png",
    //    "frame": {"x":2,"y":2,"w":1016,"h":1004},
    //    "spriteSourceSize": {"x":3,"y":13,"w":1016,"h":1004},
    //    "sourceSize": {"w":1024,"h":1024}
    //},
    //{
    //    "filename": "texture_01.png",
    //    "frame": {"x":1020,"y":2,"w":972,"h":1011},
    //    "spriteSourceSize": {"x":29,"y":4,"w":972,"h":1011},
    //    "sourceSize": {"w":1024,"h":1024}
    //}],
    //"meta": {
    //    "image": "texture_atlas.png",
    //    "size": {"w":2048,"h":2048}
    //}
    //}
    public static TextureAtlasLayout loadJson(Value json)
    {
        var ret = new TextureAtlasLayout();

        try
        {
            ret.image = json.get("meta").get("image").toString();
            ret.width = json.get("meta").get("size").get("w").toInt(0);
            ret.height = json.get("meta").get("size").get("h").toInt(0);

            var array = json.get("frames").getVector(null);
            int n = array.Count;
            for (int i = 0; i < n; i++)
            {
                ret.frames.Add( TextureLayout.loadJson(array[i]) );
            }
        }
        catch (Exception)
        {
            UtDebug.print("JSON Parse Error!");
        }

        return ret;
    }

    public string ToString()
    {
        return string.Format("TextureAtlasLayout image:{0} size({1},{2}) frames({3})", image,width,height,frames.Count);
    }

    public string ToStringVerbose()
    {
        var ret = ToString();
        foreach (var item in frames)
        {
            ret +="\n"+ item.ToString();
        }

        return ret;
    }
}
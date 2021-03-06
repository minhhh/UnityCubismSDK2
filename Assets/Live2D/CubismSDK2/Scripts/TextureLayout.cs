/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using System;
using live2d;



public class TextureLayout
{
    public string filename;
    public int srcWidth;
    public int srcHeight;

    public int x;
    public int y;

    public int trimX;
    public int trimY;


    public TextureLayout()
    {
    }



    public static TextureLayout loadJson(byte[] buf)
    {
        return loadJson(System.Text.Encoding.GetEncoding("UTF-8").GetString(buf));
    }


    public static TextureLayout loadJson(string buf)
    {
        return loadJson(buf.ToCharArray());
    }

    public static TextureLayout loadJson(char[] buf)
    {
        Value json = Json.parseFromBytes(buf);
       
        return loadJson(json);
    }

    //e.g)
    //{
    //    "filename": "texture_00.png",
    //    "frame": {"x":2,"y":2,"w":1016,"h":1004},
    //    "spriteSourceSize": {"x":3,"y":13,"w":1016,"h":1004},
    //    "sourceSize": {"w":1024,"h":1024}
    //}
    public static TextureLayout loadJson(Value json)
    {
        var ret = new TextureLayout();

        try
        {
            ret.filename = json.get("filename").toString();
            ret.srcWidth = json.get("sourceSize").get("w").toInt(0);
            ret.srcHeight = json.get("sourceSize").get("h").toInt(0);

            ret.x = json.get("frame").get("x").toInt(0);
            ret.y = json.get("frame").get("y").toInt(0);
            ret.trimX = json.get("spriteSourceSize").get("x").toInt(0);
            ret.trimY = json.get("spriteSourceSize").get("y").toInt(0);
        }
        catch (Exception)
        {
            UtDebug.print("JSON Parse Error!");
        }
       
        return ret;
    }


    public string ToString()
    {
        return string.Format( "TextureLayout filename:{0} srcSize({1},{2}) frame({3},{4}) trim({5},{6}",filename,srcWidth,srcHeight,x,y,trimX,trimY);
    }
}
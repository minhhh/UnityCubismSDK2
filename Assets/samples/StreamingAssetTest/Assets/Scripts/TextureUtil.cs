﻿using UnityEngine;
using System.IO;

public class TextureUtil {

    static public Texture2D LoadTexture(string path)
    {
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(LoadBin(path));
        //Debug.Log(string.Format("bin texture w:{0} h:{1}", texture.width, texture.height));
        return texture;
    }


    static byte[] LoadBin(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] buf = br.ReadBytes((int)br.BaseStream.Length);
        br.Close();
        return buf;
    }
}
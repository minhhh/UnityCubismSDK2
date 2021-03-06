﻿using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;

public class Live2DImporter : AssetPostprocessor 
{
	static void OnPostprocessAllAssets 
		(string[] importedAssets, string[] deletedAssets, 
		 string[] movedAssets, string[] movedFromPath)
	{
		foreach(string asset in importedAssets)
		{
			if (!asset.EndsWith(".moc") && !asset.EndsWith(".mtn")){continue;}
			renameLive2DFiles(asset);

			Debug.Log("asset : \n"+asset);
		}
		AssetDatabase.Refresh();
	}


	private static void renameLive2DFiles(string asset)
	{
		Debug.Log("asset : "+asset);
		
		string oldExt = asset.Substring(asset.Length-4,4); 
		string newExt = oldExt+".bytes";
		
		if(File.Exists(asset+".bytes"))
		{
			
			string renamedFilePath = asset;
			int n = 0;
			for(int i = 0; File.Exists(renamedFilePath+".bytes");i++)
			{
				n = i+1;
				string newPath = "("+n.ToString()+")"+oldExt;
				renamedFilePath = asset.Replace(oldExt,newPath);
			}
			File.Move(asset, Path.ChangeExtension(renamedFilePath, newExt));
		}
		else
		{
			File.Move(asset, Path.ChangeExtension(asset, newExt));
		}
		File.Delete(asset+".meta");
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CustomMapLogic : MonoBehaviour
{
         public static Texture2D LoadPNG(string filePath) {

         Debug.Log(Application.dataPath);
        //only in editor
         filePath = Application.dataPath + "/Resources/Maps/" + filePath;
         Debug.Log(filePath);
         Debug.Log(File.Exists(filePath));
         Texture2D tex = null;
         byte[] fileData;
     
         if (File.Exists(filePath))     {
            Debug.Log("ddd");
             fileData = File.ReadAllBytes(filePath);
             tex = new Texture2D(2, 2);
             tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
         }
         return tex;
     }
}

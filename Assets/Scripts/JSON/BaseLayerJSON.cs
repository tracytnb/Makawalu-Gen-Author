using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseLayerJSON
{
    public string jsonURL;
    public string imgURL;
    public string baseTitle;
    public string baseDesc;

    public BaseLayerJSON (string json, string img, string title, string desc)
    {
        jsonURL = json;
        imgURL = img;
        baseTitle = title;
        baseDesc = desc;
    }
}

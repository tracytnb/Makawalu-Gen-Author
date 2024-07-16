using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataLayer
{
    public string dataLayerPath;

    public DataLayer()
    {
        dataLayerPath = string.Empty;
    }

    public DataLayer(string dataPath)
    {
        this.dataLayerPath = dataPath;
    }
}

[Serializable]
public class ProjectJSON
{
    public string projectName;
    public string projectDesc;
    public string jsonURL;
    public string baseLayerPath;
    public DataLayer[] dataLayerPaths;
    // public string jsonElementsPositionsPath;

    public ProjectJSON(string name, string desc, string json, string BLPath, DataLayer[] DLPaths)
    {
        // string jsonTEPositionsPath use up above
        projectName = name;
        projectDesc = desc;
        jsonURL = json;
        baseLayerPath = BLPath;
        dataLayerPaths = DLPaths;
        // jsonElementsPositionsPath = jsonTEPositionsPath;
    }

    public void InitializeDefaultPaths()
    {
        // Maximum of 10 data layer paths can be entered
        dataLayerPaths = new DataLayer[10];

        for (int i = 0; i < dataLayerPaths.Length; i++)
        {
            dataLayerPaths[i] = new DataLayer();
        }

        Debug.Log("ProjectJSON: Initialized project's json with 10 empty data layer paths.");
    }
}

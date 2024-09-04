using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SFB;

public class ProjectJSONConAuth : MonoBehaviour
{
    // Where majority of global variables should be
    public static string persistentFolderPath;
    public static string rootPath;
    public string persJsonURL; // persistentJsonUrl
    public string projDirectoryPath;
    //public static string selectedFolderPath;
    public string selectedFolderPath;
    public string projectName;
    public string projectDesc;
    public string jsonURL;
    public string baseLayerPath;
    public DataLayer[] dataLayerPaths;
    // public string jsonTEPositionsPath;
    // public GameObject baseLayerManager;
    // public GameObject dataLayerManager;
    // To save to selected folder
    public string jsonURLPath;
    public string jsonBaseLayerPath;
    public DataLayer[] jsonDataLayerPaths;
    public GameObject baseManager;
    public GameObject dataUIManager;

    private void Start()
    {
        baseManager = GameObject.FindWithTag("BaseJsonManager");
        dataUIManager = GameObject.FindWithTag("DataLayerUIManager");

        dataLayerPaths = CreateNewDataArray();
        jsonDataLayerPaths = CreateNewDataArray();
        //for (int i = 0; i < dataLayerPaths.Length; i++)
        //{
        //    dataLayerPaths[i] = new DataLayer();
        //}
    }


    public DataLayer[] CreateNewDataArray()
    {
        // 10 data layers set as max
        DataLayer[] data = new DataLayer[10];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new DataLayer();
        }

        return data;
    }

    /*
     * FUNCTIONS FOR SAVING TO PERSISTENT PATH 
     */

    //  Saves progress of everything into persistent path
    public void SaveProjectToPersistent(string title, string desc, string basePath, DataLayer[] dataPaths)
    {
        string path = Path.Combine(Application.persistentDataPath, "ProjectsInProgress");
        path = Path.Combine(path, title); // Set the project folder's name
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        persistentFolderPath = path; // set's persistent root folder path

        path = Path.Combine(path, "ProjectInformation.json");
        Debug.Log("Persistent path: " + path);

        persJsonURL = path;
        jsonURL = persJsonURL;

        // save to JSON with set class variables
        ProjectJSON newJSON = new ProjectJSON("", projectName, projectDesc, jsonURL, baseLayerPath, dataLayerPaths);

        newJSON.projectName = title;
        newJSON.projectDesc = desc;
        newJSON.jsonURL = jsonURL;
        newJSON.baseLayerPath = basePath;

        newJSON.dataLayerPaths = dataPaths;

        if (newJSON.dataLayerPaths.Length > 0)
        {
            Debug.Log("hi");
            Debug.Log("Data layer path 1:" + dataLayerPaths[0].dataLayerPath);
        }

        string entry = JsonUtility.ToJson(newJSON, true);
        File.WriteAllText(jsonURL, entry);
        Debug.Log("SAVE PROJINFO JSON: " + entry);
    }


    public void SaveProjInfoToPersistent(string title, string desc)
    {
        // Update class variables
        projectName = title;
        projectDesc = desc;

        // Update persistent path information
        SaveProjectToPersistent(projectName, projectDesc, baseLayerPath, dataLayerPaths);
    }

    public void SaveBasePathToPersistent(string basePath)
    {
        if (Directory.Exists(basePath))
        {
            // Update class variable
            baseLayerPath = basePath;

            // Update persistent path information
            SaveProjectToPersistent(projectName, projectDesc, baseLayerPath, dataLayerPaths);
        }
    }

    public void SaveDataPathToPersistent(string layerPath)
    {
        for (int i = 0; i < dataLayerPaths.Length; i++)
        {
            if (string.IsNullOrEmpty(dataLayerPaths[i].dataLayerPath))
            {
                dataLayerPaths[i].dataLayerPath = layerPath;
                break;
            }
        }

        SaveProjectToPersistent(projectName, projectDesc, baseLayerPath, dataLayerPaths);
    }

    /*
     * FUNCTIONS FOR SAVING TO PROJECT FOLDER 
     */

    public void SaveProjectToJSON(string targetPath)
    {
        // Write base layer to new folder
        string path = Path.Combine(targetPath);
        path = Path.Combine(path, projectName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        selectedFolderPath = path;
        //rootPath = Path.GetFileName(path);

        path = Path.Combine(path, "ProjectInformation.json");
        Debug.Log("FINAL FOLDER PATH: " + path);

        jsonURLPath = path;
        // Handle base
        jsonBaseLayerPath = baseManager.GetComponent<BaseLayerJSONCon>().SaveBaseToFolder(selectedFolderPath);
        // Handle data layers
        //DataLayer[] jsonDataLayers = new DataLayer[10];
        //DataLayer[] dataPaths = CreateNewDataArray();
        //dataPaths = dataUIManager.GetComponent<DataLayerUIManager>().SaveLayersToFolder(targetPath);
        //jsonDataLayerPaths = CreateNewDataArray();
        jsonDataLayerPaths = dataUIManager.GetComponent<DataLayerUIManager>().SaveLayersToFolder(selectedFolderPath);
        ProjectJSON projJSON = new ProjectJSON(rootPath, projectName, projectDesc, jsonURLPath, jsonBaseLayerPath, jsonDataLayerPaths);

        string entry = JsonUtility.ToJson(projJSON, true);
        File.WriteAllText(jsonURLPath, entry);
        Debug.Log("SAVED FINAL PROJ INFO: " + entry);
    }


    //public string ReplaceRelativePath(string root, string fullPath, string newRelativePath)
    //{
    //    int rootIndex = fullPath.IndexOf(root);

    //    if (rootIndex >= 0)
    //    {
    //        int rootLength = root.Length;

    //        // Extract part of path starting from root including the root string
    //        string relative = fullPath.Substring(rootIndex + rootLength);
    //        // combine new root path with relative path and delete remaining slash
    //        string updated = Path.Combine(newRelativePath, relative.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    //        return updated;
    //    }
    //    else
    //    {
    //        Debug.LogError($"ERROR: The root path {root} was not found in path {fullPath}");
    //        return fullPath;
    //    }
    //}
}


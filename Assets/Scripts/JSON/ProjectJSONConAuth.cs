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

    //public void SaveProjPathToJSON(string projPath)
    //{
    //    // Set the global variable for projectPath
    //    selectedFolderPath = projPath;
    //    Debug.Log("selectedFolderPath: " + selectedFolderPath);
    //    rootPath = Path.GetFileName(selectedFolderPath);
    //    Debug.Log("rootFolderPath: " + rootPath);
    //    // create new Json file for projPath provided 
    //    string path = Path.Combine(rootPath, "ProjectInformation.json"); // /Users/tracy/Desktop/dsadsa/ProjectInformation.json
    //    Debug.Log("New Project Folder jsonURL: " + path);
    //    jsonURL = path;
    //    // Create a new project JSON file
    //    ProjectJSON newProjJSON = new ProjectJSON(projectName, projectDesc, jsonURL, baseLayerPath, dataLayerPaths);
    //    string entry = JsonUtility.ToJson(newProjJSON, true);
    //    File.WriteAllText(path, entry);
    //    Debug.Log("SAVE  PROJPATH JSON:\n" + entry);
    //}

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
        jsonURL = persJsonURL; // this is not final, need to change where this is set later

        // save to JSON with set class variables
        ProjectJSON newJSON = new ProjectJSON(projectName, projectDesc, jsonURL, baseLayerPath, dataLayerPaths);

        newJSON.projectName = title;
        newJSON.projectDesc = desc;
        newJSON.jsonURL = jsonURL;
        newJSON.baseLayerPath = basePath;
        newJSON.dataLayerPaths = dataPaths;

        // Initialize empty data layer paths
        if (dataLayerPaths.Length == 0)
        {
            // create empty data layers 
            newJSON.InitializeDefaultPaths();
        }

        string entry = JsonUtility.ToJson(newJSON, true);
        File.WriteAllText(jsonURL, entry);
        Debug.Log("SAVE PROJINFO JSON: " + entry);
    }


    public void SaveProjInfoToJSON(string title, string desc)
    {
        // Update class variables
        projectName = title;
        projectDesc = desc;

        // Update persistent path information
        SaveProjectToPersistent(projectName, projectDesc, baseLayerPath, dataLayerPaths);
    }

    public void SaveBasePathToJSON(string basePath)
    {
        if (Directory.Exists(basePath))
        {
            // Update class variable
            baseLayerPath = basePath;

            // Update persistent path information
            SaveProjectToPersistent(projectName, projectDesc, baseLayerPath, dataLayerPaths);
        }
    }

    public void SavePersistentToJSON()
    {
        // Implement: save everything from persistent path into project folder
        // Copy everything from persistent path
        // Save into selected directory
    }
}


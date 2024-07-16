using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProjectJSONConAuth : MonoBehaviour
{
    // Where majority of global variables should be
    public static string selectedFolderPath;
    public string projectName;
    public string projectDesc;
    public string jsonURL;
    public string baseLayerPath;
    public DataLayer[] dataLayerPaths;
    // public string jsonTEPositionsPath;
    // public GameObject baseLayerManager;
    // public GameObject dataLayerManager;

    public void SaveProjPathToJSON(string projPath)
    {
        // Set the global variable for projectPath
        selectedFolderPath = projPath;
        // create new Json file for projPath provided 
        string path = Path.Combine(projPath, "ProjectInformation.json");
        Debug.Log("New Project Folder jsonURL: " + path);
        jsonURL = path;
        // Create a new project JSON file
        ProjectJSON newProjJSON = new ProjectJSON(projectName, projectDesc, jsonURL, baseLayerPath, dataLayerPaths);
        string entry = JsonUtility.ToJson(newProjJSON, true);
        File.WriteAllText(path, entry);
        Debug.Log("SAVE  PROJPATH JSON:\n" + entry);
    }

    public void SaveProjInfoToJSON(string title, string desc)
    {
        if (File.Exists(jsonURL))
        {
            // Update class variables
            projectName = title;
            projectDesc = desc;

            // save to JSON with set class variables
            ProjectJSON newJSON = new ProjectJSON(projectName, projectDesc, jsonURL, baseLayerPath, dataLayerPaths);
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
    }

    public void SaveBaseLayerToJSON(string basePath)
    {
        
    }
}


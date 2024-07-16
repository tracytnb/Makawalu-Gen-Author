using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProjectJSONCon : MonoBehaviour
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
        // Initialize empty data layer paths
        if (dataLayerPaths.Length == 0)
        {
            newProjJSON.InitializeDefaultPaths();
        }
        string entry = JsonUtility.ToJson(newProjJSON, true);
        File.WriteAllText(path, entry);
        Debug.Log("SAVE  PROJPATH JSON:\n" + entry);
    }

    public void SaveProjInfoToJSON(string title, string desc)
    {
        ProjectJSON existingJson = LoadCurrentFromJSON();

        if (existingJson != null)
        {
            existingJson.projectName = title;
            existingJson.projectDesc = desc;

            string json = JsonUtility.ToJson(existingJson, true);
            File.WriteAllText(jsonURL, json);

            Debug.Log("SAVE PROJINFO JSON: " + json);
        }
    }

    public ProjectJSON LoadCurrentFromJSON()
    {
        if (File.Exists(jsonURL))
        {
            // Get current JSON file
            string json = File.ReadAllText(jsonURL);
            return JsonUtility.FromJson<ProjectJSON>(json);
        }
        else
        {
            Debug.LogError("ERROR: jsonURL doesn't exist, make sure Project Path has been saved");
            return null;
        }
    }
}

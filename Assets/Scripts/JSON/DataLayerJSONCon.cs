using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataLayerJSONCon : MonoBehaviour
{
    // Add tool tips later 
    public string jsonURL;
    public string layerTitle;
    public string layerDesc;
    public string layerCredit;
    public string layerIcon;
    public string layerColor;
    public SubLayer[] layerSubLayers;
    public string layerDateType;
    public DateValue[] layerTimeScale;
    public List<Texture> layerSubLayerImages; // For table display
    // Data game objects
    public GameObject projManager;

    void Start()
    {
        projManager = GameObject.FindWithTag("ProjectJsonManager");

        if (projManager == null)
        {
            Debug.LogError("ERROR: Unable to find project manager");
            return;
        }
    }

    public void SaveDataLayerToPersistent(string title, string desc, string credit, string iconPath, string color, SubLayer[] subLayers, string dateType, DateValue[] time)
    {
        string path = ProjectJSONConAuth.persistentFolderPath;
        string dirPath = Path.Combine(path, title);

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        // name json with title of data layer
        path = Path.Combine(dirPath, title + "_dl.json");

        // if user has changed the data layer title but there exists a path already
        if (jsonURL != null && jsonURL != path)
        {
            // Rename json filename to reflect new basemap title name
            string newPath = HelperMethods.RenameFile(path, jsonURL);

            // set the new jsonURL path
            if (newPath != null)
            {
                jsonURL = newPath;
            }
        }

        // Handle icon path
        string savedIconPath = HelperMethods.CopyImageFile(iconPath, dirPath, title, "_icon.png");

        // Handle sub layer image paths
        foreach (SubLayer sub in subLayers)
        {
            sub.imgURLPath = HelperMethods.CopyImageFile(sub.imgURLPath, dirPath, title, $"_{sub.subName}.png");
        }

        // Update class variables
        jsonURL = path;
        layerTitle = title;
        layerDesc = desc;
        layerCredit = credit;
        layerIcon = savedIconPath;
        layerColor = color;
        layerSubLayers = subLayers;
        layerDateType = dateType;
        layerTimeScale = time;

        // save to persistent JSON with set class variables
        DataLayerJSON dataJSON = new DataLayerJSON(jsonURL, layerTitle, layerDesc, layerCredit, layerIcon, layerColor, layerSubLayers, layerDateType, layerTimeScale);
        string entry = JsonUtility.ToJson(dataJSON, true);
        File.WriteAllText(path, entry);
        Debug.Log("SAVED DATA LAYER PERSISTENT PATH JSON\n" + entry);
        projManager.GetComponent<ProjectJSONConAuth>().SaveDataPathToPersistent(dirPath);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/* 
communicates with BaseLayerUICon and ProjectJSONConAuth
*/

public class BaseLayerJSONCon : MonoBehaviour
{
    public string jsonURL;
    public string baseTitle;
    public string baseDesc;
    public string imgURL;
    public GameObject projManager;

    void Start()
    {
        projManager = GameObject.FindWithTag("ProjectManager");

        if (projManager == null)
        {
            Debug.LogError("ERROR: Unable to find project manager");
            return;
        }
    }

    public void SaveBaseInfoToJSON(string imgPath, string title, string desc)
    {
        // reference global variable folder path
        string path = ProjectJSONConAuth.selectedFolderPath;
        string dirPath = Path.Combine(path, "BaseLayer");
        // create BaseLayer directory if it doesn't already exist
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    
        // name the json of the base layer using title
        path = Path.Combine(dirPath, title + "_bl.json");

        // if user has changed the title
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
        
        // Handle image
        string savedImgPath = SaveBaseImg(imgPath, dirPath, title);

        // Update class variables
        jsonURL = path;
        imgURL = savedImgPath;
        baseTitle = title;
        baseDesc = desc;

        // save to JSON with set class variables
        BaseLayerJSON baseJSON = new BaseLayerJSON(jsonURL, imgURL, baseTitle, baseDesc);
        string entry = JsonUtility.ToJson(baseJSON, true);
        File.WriteAllText(path, entry);
        Debug.Log("SAVE BASEPATH JSON:\n" + entry);
    }

    public string SaveBaseImg(string sourceFile, string destDir, string title)
    {
        // rename basemap png file
        string destFilePath = Path.Combine(destDir, title + ".png");

        // copy file to another location
        // overwrites file if already exists
        System.IO.File.Copy(sourceFile, destFilePath, true);

        Debug.Log("Basemap file img copied from: " + sourceFile + " \nTo destination: " + destFilePath);
        return destFilePath;
    }
}

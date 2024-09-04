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
        projManager = GameObject.FindWithTag("ProjectJsonManager");

        if (projManager == null)
        {
            Debug.LogError("ERROR: Unable to find project manager");
            return;
        }
    }

    public void SaveBaseInfoToPersistent(string imgPath, string title, string desc)
    {
        // reference global variable folder path
        string path = ProjectJSONConAuth.persistentFolderPath;
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
        string savedImgPath = HelperMethods.CopyImageFile(imgPath, dirPath, title, ".png");

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
        projManager.GetComponent<ProjectJSONConAuth>().SaveBasePathToPersistent(dirPath);
        Debug.Log("SAVED Base Directory path to PERSISTENT PATH Project JSON: " + dirPath);

    }

    public string SaveBaseToFolder(string finalPath)
    {
        string path = finalPath;
        string dirPath = Path.Combine(path, "BaseLayer");
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        path = Path.Combine(dirPath, baseTitle + "_bl.json");

        // Handle image
        string savedImgPath = HelperMethods.CopyImageFile(imgURL, dirPath, baseTitle, ".png");

        BaseLayerJSON baseJSON = new BaseLayerJSON(path, savedImgPath, baseTitle, baseDesc);
        string entry = JsonUtility.ToJson(baseJSON, true);
        File.WriteAllText(path, entry);
        Debug.Log("SAVE BASEPATH JSON:\n" + entry);
        projManager.GetComponent<ProjectJSONConAuth>().jsonBaseLayerPath = dirPath;

        return dirPath;
;    }
}

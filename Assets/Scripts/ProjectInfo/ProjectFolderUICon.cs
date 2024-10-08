using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;

public class ProjectFolderUICon : MonoBehaviour
{
    public TMP_InputField projectPath;
    public Button projPathBrowseButton;
    public Button projPathSaveButton;
    public GameObject projManager;
    public string pathFolder;

    void Start()
    {
        projManager = GameObject.FindWithTag("ProjectJsonManager");

        if (projManager == null)
        {
            Debug.LogError("ERROR: Unable to find project manager");
        }

        projPathBrowseButton.onClick.AddListener(() => {
            SelectValidateProjPath();
        });

        projPathSaveButton.onClick.AddListener(() => {
            // send to JSON to be saved
            //projManager.GetComponent<ProjectJSONConAuth>().SaveProjPathToJSON(projectPath.text);
            // SaveProjPath(projectPath.text);
            projManager.GetComponent<ProjectJSONConAuth>().SaveProjectToJSON(pathFolder);
        });
    }

    public void SelectValidateProjPath()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select where to save the project folder", "", false);

        if (paths.Length > 0)
        {
            string path = paths[0];

            if (Directory.Exists(path))
            {
                projectPath.text = path;
                pathFolder = path;
                //projManager.GetComponent<ProjectJSONConAuth>().SaveProjectToJSON(path);
            }
            else
            {
                Debug.LogError("ERROR: Path does not exist, cannot use it to save project folder in.");
            }
            //// Check if the folder is empty
            //if (IsFolderEmpty(path))
            //{
            //    // display selected folder path
            //    projectPath.text = path;
            //    Debug.Log("The folder is empty.");
            //}
            //else
            //{
            //    Debug.LogError("ERROR: The folder is not empty. Please select an empty folder to begin.");
            //}
        }
        else
        {
            Debug.LogError("ERROR: Error with the selected folder path");
        }
    }

    public bool IsFolderEmpty(string path)
    {
        // checks if file path selected is an empty folder
        int num = Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length;
        return num == 0;
    }
}

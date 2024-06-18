using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;

public class ProjectFolderSelection : MonoBehaviour
{
    public static string selectedFolderPath;

    public void OpenFolder()
    {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);

        if (paths.Length > 0)
        {
            selectedFolderPath = paths[0];
            Debug.Log("Selected Folder: " + selectedFolderPath);

            // Check if the folder is empty
            if (IsFolderEmpty(selectedFolderPath))
            {
                Debug.Log("The folder is empty.");
            }
            else
            {
                Debug.LogError("ERROR: The folder is not empty. Please select an empty folder to begin.");
            }
        }
    }

    public bool IsFolderEmpty(string path)
    {
        // checks if file path selected is an empty folder
        int num = Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length;
        return num == 0;
    }
}

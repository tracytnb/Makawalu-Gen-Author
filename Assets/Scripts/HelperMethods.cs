using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HelperMethods : MonoBehaviour
{
    public static string RenameFile(string newFilePath, string oldFilePath)
    {
        // If the JSON path is not null and does not equal the new filename
        if (!string.IsNullOrEmpty(oldFilePath) && oldFilePath != newFilePath)
        {
            // Check if JSON path exists first
            if (File.Exists(oldFilePath))
            {
                try
                {
                    // Try to rename the oldFilePath to newFilePath
                    File.Move(oldFilePath, newFilePath);
                    Debug.Log("NOTICE: File renamed from:\n" + oldFilePath + "To new file: \n" + newFilePath);
                    return newFilePath;
                }
                catch (Exception e)
                {
                    Debug.LogError("ERROR: Renaming file: " + e.Message);
                    return "";
                }
            }
            else
            {
                Debug.Log("ERROR: Old file not found " + oldFilePath);
                return "";
            }
        }

        return "";
    }
}

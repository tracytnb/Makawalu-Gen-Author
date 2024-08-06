using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

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

    public static IEnumerator DisplayTextureFromPath(string imgPath, int size, RawImage rawImage, string type)
    {
        if (File.Exists(imgPath))
        {
            // Store image file data
            byte[] bytes = File.ReadAllBytes(imgPath);
            Texture2D texture = new(size, size); // Create a temporary texture (size will be updated)
            texture.LoadImage(bytes);
            rawImage.gameObject.SetActive(true);
            rawImage.texture = texture; // Update texture of basemap
            Debug.Log($"{type} texture has been updated to file in path: " + imgPath);

        }
        else
        {
            Debug.LogError("File does not exist: " + imgPath);
        }

        yield return null;
    }

    public static string CopyImageFile(string sourceFile, string destDirectory, string title, string type)
    {
        // rename file to new path based on title, and type of image (jpg, png)
        string destFilePath = Path.Combine(destDirectory, title + type);

        // copy file to another location
        // overwrites if already exists
        System.IO.File.Copy(sourceFile, destFilePath, true);
        Debug.Log($"{title} img coped from: {sourceFile}\nTo destination path: {destFilePath}");

        return destFilePath;
    }
}

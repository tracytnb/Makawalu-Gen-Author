using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Drawing;
using Color = UnityEngine.Color;
using System.Runtime.Serialization.Formatters.Binary;

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

    public static void DisplayTextureFromPath(string imgPath, int size, RawImage rawImage, string type)
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

        //yield return null;
    }

    public static string CopyImageFile(string sourceFile, string destDirectory, string title, string type)
    {
        // rename file to new path based on title, and type of image (jpg, png)
        string destFilePath = Path.Combine(destDirectory, title + type);

        //var source = new FileInfo(sourceFile);
        //source.CopyTo(destFilePath, true);

        try
        {
            using(var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                //System.IO.File.Copy(sourceFile, destFilePath, true);
                Debug.Log("File opens successfully, no other process is using it");
            }
        }
        catch (IOException ioEx)
        {
            Debug.LogError($"Failed to ACCESS file: {ioEx.Message}");
            return null;
        }

        try
        {
            System.IO.File.Copy(sourceFile, destFilePath, true);
            Debug.Log($"{title} image copied from: {sourceFile} to destination file: {destFilePath}");
        }
        catch (IOException io)
        {
            Debug.LogError($"Failed to COPY file: {io}");
            return null;
        }

        // copy file to another location
        // overwrites if already exists
        //System.IO.File.Copy(sourceFile, destFilePath, true);
        //Debug.Log($"{title} img coped from: {sourceFile}\nTo destination path: {destFilePath}");

        return destFilePath;
    }

    public static Color ParseHexColor(string hex)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            Debug.Log("Color parsed: " + color);
            return color;
        }
        else
        {
            Debug.LogError("ERROR: Unable to parse hex string");
            return Color.white;
        }
    }

    public static Texture2D GetTexture(string imgPath, int size)
    {
        if (File.Exists(imgPath))
        {
            // Store image file data
            byte[] bytes = File.ReadAllBytes(imgPath);
            Texture2D texture = new(size, size); // Create a temporary texture (size will be updated)
            texture.LoadImage(bytes);
            return texture;
        }
        else
        {
            Debug.LogError("File does not exist: " + imgPath);
            return null;
        }
    }
}

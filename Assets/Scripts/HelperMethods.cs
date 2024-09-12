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

    public static void DeleteDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            try
            {
                // Delete the directory and all its contents recursively
                Directory.Delete(directoryPath, true);
                Debug.Log("NOTICE: Directory deleted successfully: " + directoryPath);
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR: Failed to delete directory: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("ERROR: Directory not found: " + directoryPath);
        }
    }

    public static Texture2D DisplayTextureFromPath(string imgPath, RawImage rawImage, string type)
    {
        if (File.Exists(imgPath))
        {
            // Store image file data
            byte[] bytes = File.ReadAllBytes(imgPath);
            Texture2D texture = new(2, 2); // Create a temporary texture (size will be updated)
            //texture.LoadImage(bytes);
            rawImage.gameObject.SetActive(true);

            if (texture.LoadImage(bytes))
            {
                // Adjust the texture of the RawImage
                rawImage.gameObject.SetActive(true);
                rawImage.texture = texture;

                // Force update the AspectRatioFitter (if needed)
                AspectRatioFitter aspectRatioFitter = rawImage.GetComponent<AspectRatioFitter>();
                if (aspectRatioFitter != null)
                {
                    aspectRatioFitter.aspectRatio = (float)texture.width / texture.height;
                    aspectRatioFitter.SetLayoutHorizontal();
                    aspectRatioFitter.SetLayoutVertical();
                }

                Debug.Log($"{type} texture has been updated to file in path: " + imgPath);
            }
            else
            {
                Debug.LogError("Failed to load image data from: " + imgPath);
                return null;
            }

            rawImage.texture = texture; // Update texture of basemap
            Debug.Log($"{type} texture has been updated to file in path: " + imgPath);
            return texture;

        }
        else
        {
            Debug.LogError("File does not exist: " + imgPath);
            return null;
        }

        //yield return null;
    }

    //public static string CopyImageFile(string sourceFile, string destDirectory, string title, string type)
    //{
    //    // rename file to new path based on title, and type of image (jpg, png)
    //    string destFilePath = Path.Combine(destDirectory, title + type);

    //    if (sourceFile == destDirectory)
    //    {
    //        Debug.Log("Paths are already the same");
    //        return destFilePath;
    //    }

    //    try
    //    {
    //        using(var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    //        {
    //            //System.IO.File.Copy(sourceFile, destFilePath, true);
    //            Debug.Log("File opens successfully, no other process is using it");
    //        }
    //    }
    //    catch (IOException ioEx)
    //    {
    //        Debug.LogError($"Failed to ACCESS file: {ioEx.Message}");
    //        return null;
    //    }

    //    try
    //    {
    //        System.IO.File.Copy(sourceFile, destFilePath, true);
    //        Debug.Log($"{title} image copied from: {sourceFile} to destination file: {destFilePath}");
    //    }
    //    catch (IOException io)
    //    {
    //        Debug.LogError($"Failed to COPY file: {io}");
    //        return null;
    //    }

    //    // copy file to another location
    //    // overwrites if already exists
    //    //System.IO.File.Copy(sourceFile, destFilePath, true);
    //    //Debug.Log($"{title} img coped from: {sourceFile}\nTo destination path: {destFilePath}");

    //    return destFilePath;
    //}

    public static string CopyImageFile(string sourceFile, string destDirectory, string title, string type)
    {
        // Rename file to new path based on title and type of image (jpg, png)
        string destFilePath = Path.Combine(destDirectory, title + type);

        if (sourceFile == destFilePath)
        {
            Debug.Log("Paths are already the same");
            return destFilePath;
        }

        try
        {
            // Try to copy the file directly
            File.Copy(sourceFile, destFilePath, true); // Overwrites if already exists
            Debug.Log($"{title} image copied from: {sourceFile} to destination file: {destFilePath}");
        }
        catch (IOException ioEx)
        {
            Debug.LogError($"Failed to copy file due to an I/O error: {ioEx.Message}");
            return null;
        }
        catch (UnauthorizedAccessException uae)
        {
            Debug.LogError($"Access to the file denied: {uae.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Unexpected error while copying file: {ex.Message}");
            return null;
        }

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
}

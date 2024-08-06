using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using SFB;

public class SubLayerUICon : MonoBehaviour
{
    // Input data 
    public TMP_InputField inputSubName;
    public TMP_InputField inputSubImagePath;
    public TMP_InputField inputSubDateValue;
    public Image inputSubColor;
    // Data to pass
    public string subName;
    public string subImagePath;
    public DateValue subDateValue;
    public string subColor;
    // Data objects
    public GameObject subManager;
    public GameObject parentDataLayer;
    // buttons
    public Button browseSubImageButton;
    public Button deleteSubButton;

    void Start()
    {
        subManager = GameObject.FindWithTag("SubManager");
        parentDataLayer = GameObject.FindWithTag("DataLayer");

        browseSubImageButton.onClick.AddListener(() =>
        {
            // Select image
            SelectSubLayerImage();
        });

        deleteSubButton.onClick.AddListener(() =>
        {
            DeleteSubLayer();
        });
    }

    public void SelectSubLayerImage()
    {
        var extensions = new[]
{
            new ExtensionFilter("ImageFiles", "png"),
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Select Sub Layer Image", "", extensions, true);

        if (paths.Length > 0)
        {
            ValidateSubLayerImage(paths[0]);
        }
    }

    public void ValidateSubLayerImage(string imgPath)
    {
        if (string.IsNullOrEmpty(imgPath))
        {
            Debug.LogError("ERROR: Icon image is not selected");
            return;
        }

        string fileExt = Path.GetExtension(imgPath.ToLower());

        // Check if the file is a PNG
        if (fileExt != ".png")
        {
            Debug.Log("ERROR: Icon image must be of type .PNG, .JPG, or .JPEG");
            return;
        }

        if (!File.Exists(imgPath))
        {
            Debug.LogError("ERROR: Icon image path does not exist");
            return;
        }

        Debug.Log("NOTICE: Data layer icon path is a valid file");
        subImagePath = imgPath;
        inputSubImagePath.text = imgPath;
    }

    public string ValidateSubLayerName()
    {
        if (string.IsNullOrEmpty(inputSubName.text))
        {
            Debug.LogError("ERROR: Sub layer name cannot be empty");
            return "";
        }

        subName = inputSubName.text;
        return subName;
    }

    public string ValidateSubImg()
    {
        string subPath = inputSubImagePath.text;

        if (string.IsNullOrEmpty(subPath))
        {
            Debug.LogError($"ERROR: Sub layer image path for sub layer {subName} was not selected");
            return "";
        }

        if (Path.GetExtension(subPath).ToLower() != ".png")
        {
            Debug.Log("ERROR: Only PNG files are allowed.");
            return "";
        }

        return subPath;
    }

    public (DateValue, string) ValidateDateInfo()
    {
        // Validates if the date value entered is valid and validates if the dateValue entered matches the datetype selected
        string inputText = inputSubDateValue.text;
        Debug.Log(inputSubDateValue.text);
        DateType selectedDateType = parentDataLayer.GetComponent<DataLayerUICon>().selectedDateType; // 0 or 1 or 2 or 3

        switch (selectedDateType)
        {
            case DateType.Year:
                // If it is exactly 4 digits
                if (Regex.IsMatch(inputText, @"^\d{4}$"))
                {
                    Debug.Log("Valid year input: " + inputText);
                    subDateValue = new DateValue(yearVal: inputText, monthVal: "");
                    return (subDateValue, $"Correct Input for YYYY: {subDateValue}");
                }

                Debug.Log("ERROR: Must be of format YYYY");
                return (null, $"WRONG FORMAT OF YYYY, inputted: {subDateValue}"); // default to null

            case DateType.Month:
                // If it is exactly 2 digits
                string monthPattern = @"^(0[0-9]|1[0-2])$";
                if (Regex.IsMatch(inputText, monthPattern))
                {
                    Debug.Log("Valid month input: " + inputText);
                    subDateValue = new DateValue(yearVal: "", monthVal: inputText);
                    return (subDateValue, $"Correct Input for MM: {subDateValue}");
                }

                Debug.Log("ERROR: Must be of format MM");
                return (null, $"WRONG FORMAT OF MM, inputted: {subDateValue}"); // default to null

            case DateType.MonthYear:
                // expression to match MM/YYYY format
                string monthYearPattern = @"^(0[1-9]|1[0-2])\/\d{4}$";

                if (Regex.IsMatch(inputText, monthYearPattern))
                {
                    string[] parts = inputText.Split('/');
                    string month = parts[0];
                    string year = parts[1];
                    subDateValue = new DateValue(yearVal: year, monthVal: month);
                    Debug.Log("DateValue: " + subDateValue);
                    return (subDateValue, $"Correct Input for MM/YY: {subDateValue}");
                }
                else
                {
                    Debug.Log("ERROR: Must be of format MM/YYYY");
                    return (null, $"WRONG FORMAT OF MM/YYYY, inputted: {subDateValue}"); // default to null
                }

            default:
                Debug.Log("No date value selected, data is not time-based and will be set as an empty string");

                return  (new DateValue(), $"Date value is empty"); // default to null
        }
    }

    public string ValidateSubColor()
    {
        Color color = inputSubColor.color;
        subColor = ColorUtility.ToHtmlStringRGB(color);

        if (subColor == null)
        {
            Debug.LogError("sub color does not exist");
            return "";
        }

        Debug.Log("Sub color: " + subColor);
        return subColor;
    }

    public void DeleteSubLayer()
    {
        int listCount = subManager.GetComponent<SubLayerManager>().subLayerUIList.childCount;

        if (listCount < 2)
        {
            Debug.LogError("ERROR: Cannot delete, only one sub layer left");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Removed subLayer. Sub layer list count is now " + subManager.GetComponent<SubLayerManager>().subLayerUIList.childCount);
        }
    }
}

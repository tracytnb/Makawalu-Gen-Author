using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SFB;
using System.IO;
using static SubLayerUICon;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

// Attached to each individual Data Layer created
public class DataLayerUICon : MonoBehaviour
{
    // Input data
    public TMP_InputField inputLayerName;
    public TMP_InputField inputLayerDesc;
    public TMP_InputField inputLayerCredit;
    public Image inputLayerColor;
    public RawImage inputLayerIcon;
    public TMP_Dropdown dropdownDateType;
    // Data to pass 
    public string layerName;
    public string layerDesc;
    public string layerCredit;
    public string layerIconPath;
    public SubLayer[] layerSubLayers;
    public DateType selectedDateType;
    public string layerDateType;
    public DateValue[] layerTimescales;
    public string layerColor;
    // Data objects
    public List<Texture> layerSubLayerImages;
    public GameObject dataManager;
    public GameObject subLayerManager;
    public GameObject colorPicker;
    public ScrollRect scrollRect;
    // buttons
    public Button dataSelectColorButton;
    public Button dataIconButton;
    public Button dataSaveButton;

    private void Start()
    {
        colorPicker.gameObject.SetActive(false);
        dataManager = GameObject.FindWithTag("DataManager");
        subLayerManager = GameObject.FindWithTag("SubManager");

        dataSelectColorButton.onClick.AddListener(() =>
        {
            SelectDataLayerColor();
        });

        dataIconButton.onClick.AddListener(() =>
        {
            SelectDataLayerIcon();
        });

        dataSaveButton.onClick.AddListener(() =>
        {
            ValidateDataLayerInfo();
        });
    }

    public void SaveDataLayerToJSON()
    {
        // Used at the end of creating the project
    }

    public void ValidateDataLayerInfo()
    {
        if (string.IsNullOrEmpty(inputLayerName.text))
        {
            Debug.LogError("ERROR: Data layer name is not set. Add in data layer name.");
            return;
        }

        if (string.IsNullOrEmpty(inputLayerDesc.text))
        {
            Debug.LogError("ERROR: Data layer description is not set. Add in a description for the data layer");
            return;
        }

        // Set data layer object variables 
        layerName = inputLayerName.text;
        layerDesc = inputLayerDesc.text;
        layerCredit = inputLayerCredit.text; // Credit is okay to be empty? 

        // Validate color selection, cannot be white or black
        layerColor = "#" + ValidateLayerColor();
        if (string.IsNullOrEmpty(layerColor))
        {
            Debug.LogError("ERROR: Data layer color cannot be black or white. Change main color.");
            return;
        }

        // Validate sub layers
        selectedDateType = GetSelectedDateType();
        layerDateType = selectedDateType.ToString();


        bool subReady = subLayerManager.GetComponent<SubLayerManager>().ValidateSubLayers();

        if (subReady)
        {
            layerTimescales = subLayerManager.GetComponent<SubLayerManager>().dateValueList.ToArray();
            layerSubLayers = subLayerManager.GetComponent<SubLayerManager>().subLayerList.ToArray();

            dataManager.GetComponent<DataLayerJSONCon>().SaveDataLayerToPersistent(layerName, layerDesc, layerCredit, layerIconPath, layerColor, layerSubLayers, layerDateType, layerTimescales);
        }
    }

    public DateType GetSelectedDateType()
    {
        int dropdownValue = dropdownDateType.value;
        DateType dateType = (DateType)dropdownValue; // Convert to DateType enum

        return dateType;
    }

    public void SelectDataLayerColor()
    {
        colorPicker.SetActive(!colorPicker.activeSelf);

        if (colorPicker.activeSelf)
        {
            Debug.Log("color picker on");
            scrollRect.enabled = false;
        }
        else
        {
            Debug.Log("color picker off");
            scrollRect.enabled = true;
            Image pickedColor = colorPicker.GetComponent<ColorPaletteController>().pickedColorImage;
            inputLayerColor.color = pickedColor.color;
            layerColor = "#" + ValidateLayerColor();
            Debug.Log("LAYERCOLOR: " + layerColor);
            subLayerManager.GetComponent<SubLayerManager>().UpdateSubColors();
        }
    }

    public void SelectDataLayerIcon()
    {
        var extensions = new[]
        {
            new ExtensionFilter("ImageFiles", "png", "jpg", "jpeg"),
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Select Data Layer Icon Image", "", extensions, true);

        if (paths.Length > 0)
        {
            ValidateDataLayerIcon(paths[0]);
            inputLayerIcon.gameObject.SetActive(true);
            StartCoroutine(HelperMethods.DisplayTextureFromPath(paths[0], 150, inputLayerIcon, "Data Layer Icon"));
        }
    }

    public void ValidateDataLayerIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath))
        {
            Debug.LogError("ERROR: Icon image is not selected");
            return;
        }

        string fileExt = Path.GetExtension(iconPath.ToLower());

        // Check if the file is a PNG
        if (fileExt != ".png" && fileExt != ".jpg" && fileExt != ".jpeg")
        {
            Debug.Log("ERROR: Icon image must be of type .PNG, .JPG, or .JPEG");
            return;
        }

        if (!File.Exists(iconPath))
        {
            Debug.LogError("ERROR: Icon image path does not exist");
            return;
        }

        Debug.Log("NOTICE: Data layer icon path is a valid file");
        layerIconPath = iconPath;

    }

    public string ValidateLayerColor()
    {
        Color color = inputLayerColor.color;

        if (color == Color.white || color == Color.black)
        {
            Debug.LogError("ERROR: Color cannot be white or black");
            return "";
        }

        // convert it to hex string to be saved later
        string hexColor = ColorUtility.ToHtmlStringRGB(color);

        Debug.Log("Color is hex: " + hexColor);
        return hexColor;
    }
}

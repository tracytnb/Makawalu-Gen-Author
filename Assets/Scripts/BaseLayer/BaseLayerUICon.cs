using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;

public class BaseLayerUICon : MonoBehaviour
{
    public TMP_InputField inputBaseTitle;
    public TMP_InputField inputBaseDesc;
    public TMP_InputField inputBaseImgPath;
    public string baseTitle;
    public string baseDesc;
    public string baseImgPath;
    public RawImage baseImg;
    public GameObject baseManager;
    public Button baseBrowseButton;
    public Button baseSaveButton;

    // Start is called before the first frame update
    void Start()
    {
        baseManager = GameObject.FindWithTag("BaseManager");
        baseImg.gameObject.SetActive(false);

        baseBrowseButton.onClick.AddListener(() => {
            SelectBasemapImage();
        });

        baseSaveButton.onClick.AddListener(() => {
            ValidateBaseInfo();
        });
    }

    public void SelectBasemapImage()
    {
        var extensions = new [] {
            new ExtensionFilter("ImageFiles", "png"),
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Select Background Map Image", "", extensions, true);

        if (paths.Length > 0)
        {
            baseImgPath = paths[0];
            ValidateBaseImg(paths[0]);
            inputBaseImgPath.text = baseImgPath;
            baseImgPath = inputBaseImgPath.text;
            StartCoroutine(HelperMethods.DisplayTextureFromPath(paths[0], 390, baseImg, "Base Layer"));
            Debug.Log("Selected file paths[0]: " + paths[0]);
        }
        else
        {
            Debug.LogError("ERROR: No file was selected");
        }
    }

    public void ValidateBaseInfo()
    {
        if (string.IsNullOrEmpty(inputBaseTitle.text))
        {
            Debug.LogError("ERROR: Background Map Title cannot be empty");
            return;
        }

        if (string.IsNullOrEmpty(inputBaseDesc.text))
        {
            Debug.LogError("ERROR: Background Map Description cannot be empty");
            return;
        }

        baseTitle = inputBaseTitle.text;
        baseDesc = inputBaseDesc.text;

        baseManager.GetComponent<BaseLayerJSONCon>().SaveBaseInfoToPersistent(baseImgPath, baseTitle, baseDesc);
    }

    // For error checking
    public void ValidateBaseImg(string imgPath)
    {
        if (string.IsNullOrEmpty(baseImgPath))
        {
            Debug.LogError("ERROR: Background Map Image is not selected");
            return;
        }
        
        // Check if the file is a PNG
        if (Path.GetExtension(imgPath).ToLower() != ".png")
        {
            Debug.Log("ERROR: Only PNG files are allowed.");
            return;
        }
        
        if (!File.Exists(baseImgPath))
        {
            Debug.LogError("ERROR: Background map image path provided does not exist");
            return;
        }

        Debug.Log("NOTICE: base image path is a valid PNG file");
    }
}

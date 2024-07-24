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
            inputBaseImgPath.text = baseImgPath;
            StartCoroutine(DisplayTextureFromPath(baseImgPath));
            Debug.Log("Selected file paths[0]: " + paths[0]);
        }
        else
        {
            Debug.LogError("ERROR: No file was selected");
        }
    }

    public IEnumerator DisplayTextureFromPath(string imgPath)
    {
        if (File.Exists(imgPath))
        {
            // Store image file data
            byte[] bytes = File.ReadAllBytes(imgPath);
            Texture2D texture = new(450, 450); // Create a temporary texture (size will be updated)
            texture.LoadImage(bytes); 
            baseImg.gameObject.SetActive(true);
            baseImg.texture = texture; // Update texture of basemap
            Debug.Log("Base layer texture has been updated to file in path: " + imgPath);

        }
        else
        {
            Debug.LogError("File does not exist: " + imgPath);
        }

        yield return null;
    }

    public void ValidateBaseInfo()
    {
        baseImgPath = inputBaseImgPath.text;
        baseTitle = inputBaseTitle.text;
        baseDesc = inputBaseDesc.text;

        ValidateBaseImg(baseImgPath);

        if (string.IsNullOrEmpty(baseTitle))
        {
            Debug.LogError("ERROR: Background Map Title cannot be empty");
            return;
        }

        if (string.IsNullOrEmpty(baseDesc))
        {
            Debug.LogError("ERROR: Background Map Description cannot be empty");
            return;
        }

        baseManager.GetComponent<BaseLayerJSONCon>().SaveBaseInfoToJSON(baseImgPath, baseTitle, baseDesc);
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

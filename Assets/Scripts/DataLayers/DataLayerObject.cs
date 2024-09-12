using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * A data layer object prefab, might need to change to OutputListItem later on? (FOR LATER)
 * Stores and updates saved data from DataLayerUIManager whilst user is actively changing things around
 * 
 * Interacts with the Output View and updates the Input View when clicked
 * 
 */
public class DataLayerObject : MonoBehaviour
{
    // Prefab game objects attached that need to be updated
    public TMP_Text DLName;
    public Image DLColor;
    // Prefabs for game objects on the TABLE
    public Transform DLImageGroupList;
    public GameObject DLImagePrefab;
    // Data passed 
    public string layerName;
    public string layerDesc;
    public string layerCredit;
    public string layerIconPath;
    public string layerColor;
    public SubLayer[] layerSubLayers;
    public Texture2D[] layerSubImages;
    public DateType selectedDateType;
    public string layerDateType;
    public DateValue[] layerTimescales;
    // Objects to reference
    public GameObject dataUIManager;
    // Buttons
    public Button selectedDataButton;

    void Start()
    {
        selectedDataButton = gameObject.GetComponent<Button>();

        // If data object button was clicked, populate the Input View - Data Layer step with existing information from data object
        selectedDataButton.onClick.AddListener(() =>
        {
            ViewDataObjectInfo();
        });

        dataUIManager = GameObject.FindWithTag("DataLayerUIManager");

        if (dataUIManager == null)
        {
            Debug.LogError("ERROR: Unable to locate object with tag \'DataLayerUIManager\'");
        }
        Debug.Log("Found DataLayerUIManager gameObject");

        if (dataUIManager != null)
        {
            dataUIManager.GetComponent<DataLayerUIManager>().InstantiateManagers();
        }
    }

    public void RenameObjects(string layerName, string oldName)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            string current = child.name;
            Debug.Log(child.name);

            if (!string.IsNullOrEmpty(oldName))
            {
                string suffix = "_" + oldName;

                if (current.EndsWith(suffix))
                {
                    // remove previous layer name
                    current = current.Substring(0, current.Length - suffix.Length);
                }
            }

            // rename with new suffix
            child.name = current + "_" + layerName;
            Debug.Log(child.name);
        }
    }

    public void UpdateOutputListInfo(string name, string color)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("ERROR: Data Layer NAME is null or empty");
            return;
        }

        if (string.IsNullOrEmpty(color))
        {
            Debug.LogError("ERROR: Data Layer COLOR is null or empty");
            return;
        }

        DLName.text = name;
        DLColor.color = HelperMethods.ParseHexColor(color);
    }

    public void ViewDataObjectInfo() =>
        // pass the information to be loaded back into the Input View from DataLayerUIManager
        dataUIManager.GetComponent<DataLayerUIManager>().DataLayerToEdit(gameObject, layerName, layerDesc, layerCredit, layerIconPath, layerColor, layerSubLayers, layerDateType, layerTimescales);

    public void SetDataLayerImageGroup()
    {
        // reset the list
        if (DLImageGroupList == null)
        {
            Debug.LogError("ERROR: Unable to find DL image group");
            return;
        }
        else
        {
            foreach(Transform child in DLImageGroupList)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        DLImageGroupList.name = "DL_ImageGroup_" + layerName;
        for (int i = 0; i < layerSubLayers.Length; i++)
        {
            SubLayer subLayer = layerSubLayers[i];
            GameObject newSubImage = Instantiate(DLImagePrefab, DLImageGroupList);
            newSubImage.name = "DL_Image_" + subLayer.subName;
            RawImage subImage = newSubImage.GetComponent<RawImage>();
            subImage.texture = layerSubImages[i]; // Use dictionary instead?
            subImage.color = HelperMethods.ParseHexColor(subLayer.subColorCode);
        }
    }
}

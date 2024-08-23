using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Communicates between DataLayerInputUICon and DataLayerOutputUICon and DataLayerJsonManager
 */
public class DataLayerUIManager : MonoBehaviour
{
    // Data objects
    public Transform outputObjectList; // the table items addded in the item list
    public GameObject dataLayerObjectPrefab;
    public Transform tableImagesList; // the Table_Images_MOVEONLY list
    public Transform tableImageGroupPrefab;
    public GameObject dataJsonManager;
    public GameObject dataInputView;
    public GameObject subUIManager;
    public GameObject dataLayerToEdit;

    void Start()
    {
        dataJsonManager = GameObject.FindWithTag("DataLayerJsonManager");
        GameObject objList = GameObject.FindWithTag("OutputList");
        //dataInputView = GameObject.FindWithTag("DataLayerInput");
        //subUIManager = GameObject.FindWithTag("SubLayerInputManager");
        tableImagesList = GameObject.FindWithTag("Table_Images").transform;

        if (objList != null)
        {
            outputObjectList = objList.transform;
        }
    }

    public void InstantiateManagers()
    {
        dataInputView = GameObject.FindWithTag("DataLayerInput");
        subUIManager = GameObject.FindWithTag("SubLayerInputManager");
    }

    // Adding a NEW data layer
    public void AddDataLayerInput(string name, string desc, string credit, string iconPath, string color, SubLayer[] subLayers, Texture2D[] subImages, string dateType, DateValue[] timescales)
    {
        // Instantiate a new data layer object
        GameObject newDataLayer = Instantiate(dataLayerObjectPrefab, outputObjectList);
        newDataLayer.GetComponent<DataLayerObject>().layerName = name;
        newDataLayer.GetComponent<DataLayerObject>().layerDesc = desc;
        newDataLayer.GetComponent<DataLayerObject>().layerCredit = credit;
        newDataLayer.GetComponent<DataLayerObject>().layerIconPath = iconPath;
        newDataLayer.GetComponent<DataLayerObject>().layerColor = color;
        newDataLayer.GetComponent<DataLayerObject>().layerSubLayers = subLayers;
        newDataLayer.GetComponent<DataLayerObject>().layerSubImages = subImages;
        newDataLayer.GetComponent<DataLayerObject>().layerDateType = dateType;
        newDataLayer.GetComponent<DataLayerObject>().layerTimescales = timescales;

        // Rename gameObject
        newDataLayer.name = "DL_Item_" + name;
        // Rename data layer's objects
        newDataLayer.GetComponent<DataLayerObject>().RenameObjects(name);
        // Update data layer object prefab
        newDataLayer.GetComponent<DataLayerObject>().UpdateOutputListInfo(name, color);
        // Create table group for data layer
        Transform newGroup = Instantiate(tableImageGroupPrefab, tableImagesList);
        newDataLayer.GetComponent<DataLayerObject>().DLImageGroupList = newGroup;
        newDataLayer.GetComponent<DataLayerObject>().SetDataLayerImageGroup();

        // Save a new data layer to persistent path
        dataJsonManager.GetComponent<DataLayerJSONCon>().SaveDataLayerToPersistent(name, desc, credit, iconPath, color, subLayers, dateType, timescales);
    }

    public void CheckExistingLayerName(string layerName)
    {
        if (outputObjectList.childCount > 0)
        {
            foreach(Transform obj in outputObjectList)
            {
                Debug.Log(obj.gameObject.name);
                if (obj.gameObject.name == "DLItem_" + layerName)
                {
                    Debug.LogError($"ERROR: Data layer with name {layerName} already exists. Cannot make new data layer. Please name it a different name.");
                    return;
                }
            }
        }
    }

    // Display the data layer that was selected back into the Input View panel 
    public void DataLayerToEdit(GameObject dataLayerRef, string name, string desc, string credit, string iconPath, string color, SubLayer[] subLayers, string dateType, DateValue[] timescales)
    {
        // change to update text
        dataInputView.GetComponent<DataLayerInputUICon>().dataSaveButton.GetComponentInChildren<TMP_Text>().text = "Update";
        // Set the data layer object reference
        dataLayerToEdit = dataLayerRef;
        dataInputView.GetComponent<DataLayerInputUICon>().beingEdited = true;
        // Load variables
        dataInputView.GetComponent<DataLayerInputUICon>().layerName = name;
        dataInputView.GetComponent<DataLayerInputUICon>().layerDesc = desc;
        dataInputView.GetComponent<DataLayerInputUICon>().layerCredit = credit;
        dataInputView.GetComponent<DataLayerInputUICon>().layerIconPath = iconPath;
        dataInputView.GetComponent<DataLayerInputUICon>().layerColor = color;
        dataInputView.GetComponent<DataLayerInputUICon>().layerSubLayers = subLayers;
        dataInputView.GetComponent<DataLayerInputUICon>().layerDateType = dateType; 
        DateType type = (DateType)Enum.Parse(typeof(DateType), dateType);
        dataInputView.GetComponent<DataLayerInputUICon>().dropdownDateType.value = (int)type;
        // Load existing input fields
        dataInputView.GetComponent<DataLayerInputUICon>().inputLayerName.text = name;
        dataInputView.GetComponent<DataLayerInputUICon>().inputLayerDesc.text = desc;
        dataInputView.GetComponent<DataLayerInputUICon>().inputLayerCredit.text = credit;
        // Load objects
        dataInputView.GetComponent<DataLayerInputUICon>().inputLayerColor.color = HelperMethods.ParseHexColor(color);
        dataInputView.GetComponent<DataLayerInputUICon>().layerIconPath = iconPath;
        //dataInputView.GetComponent<DataLayerInputUICon>().inputLayerIcon = 
        RawImage inputImage = dataInputView.GetComponent<DataLayerInputUICon>().inputLayerIcon;
        //Debug.Log("DJKLSAJDKLAS: " + iconPath);
        //StartCoroutine(HelperMethods.DisplayTextureFromPath(iconPath, 150, inputImage, "Data Layer Icon"));
        HelperMethods.DisplayTextureFromPath(iconPath, inputImage, "Data Layer Icon");
        // Load sub layer prefabs
        subUIManager.GetComponent<SubLayerInputManager>().LoadInputSubLayers(subLayers, dateType);
    }

    public void UpdateExistingDataLayer(string name, string desc, string credit, string iconPath, string color, SubLayer[] subLayers, Texture2D[] subImages, string dateType, DateValue[] timescales)
    {
        dataLayerToEdit.GetComponent<DataLayerObject>().layerName = name;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerDesc = desc;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerCredit = credit;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerIconPath = iconPath;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerColor = color;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerSubLayers = subLayers;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerSubImages = subImages;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerDateType = dateType;
        dataLayerToEdit.GetComponent<DataLayerObject>().layerTimescales = timescales;

        // Rename gameObject
        dataLayerToEdit.name = "DL_Item_" + name;
        // Update data layer object name
        dataLayerToEdit.GetComponent<DataLayerObject>().RenameObjects(name);
        // Update data layer object prefab
        dataLayerToEdit.GetComponent<DataLayerObject>().UpdateOutputListInfo(name, color);
        // Update Image Group
        dataLayerToEdit.GetComponent<DataLayerObject>().SetDataLayerImageGroup();
        // Save an existing data layer to persistent path
        dataJsonManager.GetComponent<DataLayerJSONCon>().SaveDataLayerToPersistent(name, desc, credit, iconPath, color, subLayers, dateType, timescales);
        // Change updated to false
        dataLayerToEdit = null;
        dataInputView.GetComponent<DataLayerInputUICon>().beingEdited = false;
        // change to save text
        dataInputView.GetComponent<DataLayerInputUICon>().dataSaveButton.GetComponentInChildren<TMP_Text>().text = "Save";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class SubLayerManager : MonoBehaviour
{
    // Data to pass 
    public List<SubLayer> subLayerList;
    public List<DateValue> dateValueList;
    public List<Texture> subTextureList;
    // Data objects
    public Transform subLayerUIList; // SubDataLayerScrollList > ItemContent
    public GameObject subLayerPrefab;
    public GameObject newSubLayer;
    public GameObject dataLayerObject;
    // buttons
    public Button addSubLayerButton;

    private void Start()
    {
        dataLayerObject = GameObject.FindWithTag("DataLayer");

        addSubLayerButton.onClick.AddListener(() =>
        {
            AddSubLayer();
            // instantiates a new sublayer entry prefab
        });
    }

    // Validates the sub layer list in the UI
    public bool ValidateSubLayers()
    {
        bool subLayersReady = true; // start with valid
        // start with a new sub layer list and date value list each time to be saved and passed
        subLayerList = new List<SubLayer>();
        dateValueList = new List<DateValue>();

        Debug.Log(subLayerUIList.childCount);
        foreach (Transform subLayer in subLayerUIList)
        {
            var subUICon = subLayer.GetComponent<SubLayerUICon>();

            string name = subUICon.ValidateSubLayerName();
            Debug.Log("NAME:" + name);
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("display sub layer NAME error indicator");
            }

            string img = subUICon.ValidateSubImg();
            Debug.Log("IMG PATH: " + img);
            if (string.IsNullOrEmpty(img))
            {
                Debug.LogError("display sub layer IMAGE error indicator");
            }

            string color = subUICon.ValidateSubColor();
            if (string.IsNullOrEmpty(img))
            {
                Debug.LogError("display sub layer IMAGE error indicator");
            }

            // add to date value list here, entries can be nullable
            (DateValue dateValue, string returnString) = subUICon.ValidateDateInfo();
            Debug.Log("DATE VALUE: " + dateValue.year + dateValue.month);
            Debug.Log($"REPORT: {returnString}");
            dateValueList.Add(dateValue);

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(img) && !string.IsNullOrEmpty(color))
            {
                // Add a new valid subLayer item to the sub layer list that will be passed and saved to JSON
                SubLayer newSub = new SubLayer(name, img, dateValue, color);
                subLayerList.Add(newSub);
            }
        }

        // Confirm the length of sublayers matches with date values entered
        if (subLayerList.Count == dateValueList.Count)
        {
            for (int i = 0; i < subLayerList.Count; i++)
            {
                Debug.Log($"Sub List: {subLayerList[i].dateValue}, Date List: {dateValueList[i]}");
            }

            Debug.Log($"Sub Layer count {subLayerList.Count} matches date values count {dateValueList.Count}");
        }
        else
        {
            Debug.Log("ERJOIERJERE");
            subLayersReady = false;
        }

        if (subLayersReady)
        {
            Debug.Log("All sublayers are valid");
            return subLayersReady;
            
        }
        else
        {
            Debug.LogError("some sub layers need to be fixed");
            return subLayersReady;
        }
    }

    public void AddSubLayer()
    {
        // check count of sublayer
        if (subLayerUIList.childCount == 10)
        {
            Debug.LogError($"ERROR: SubLayer count is {subLayerUIList.childCount} which is maximum");
            return;
        }

        // adds a new sublayer to the subLayer list
        newSubLayer = Instantiate(subLayerPrefab, subLayerUIList);
        // ensure it is added below the previous items
        newSubLayer.transform.SetAsLastSibling();
        // Update the sub layer colors
        UpdateSubColors();
    }

    public void UpdateSubColors()
    {
        int childCount = subLayerUIList.childCount;
        int segments = childCount - 1;
        float increment = 1.0f / segments;
        Color mainColor;

        if (ColorUtility.TryParseHtmlString(dataLayerObject.GetComponent<DataLayerUICon>().layerColor, out mainColor))
        {
            Debug.Log("main color was set: " + mainColor);
        }
        else
        {
            Debug.Log("Main color not set yet");
            return;
        }

        Debug.Log("UpdateColor child count: " + subLayerUIList.childCount);

        for (int i = 0; i < childCount; i++)
        {
            Transform subImgTrans = subLayerUIList.GetChild(i).Find("SubLayerColor");

            if (subImgTrans != null)
            {
                Image subImgObj = subImgTrans.GetComponent<Image>();
                Color newColor;

                if (i == 0)
                {
                    subImgObj.color = mainColor;
                }
                else
                {
                    float t = (float)i / segments;

                    newColor = Color.Lerp(mainColor, mainColor * 0.4f, t);
                    newColor.a = 1.0f;
                    Debug.Log($"sub layer color at {i} is {newColor}");

                    subImgObj.color = newColor;
                }
            }
            else
            {
                Debug.Log("Image gameObject not found in subDataLayerItem");
            }
        }

    }
}

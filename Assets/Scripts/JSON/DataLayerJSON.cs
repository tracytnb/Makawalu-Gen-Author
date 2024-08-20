using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubLayer
{
    [Tooltip("The name of the sub layer")]
    public string subName;
    [Tooltip("The image URL path of the sub layer's image")]
    public string imgURLPath;
    public DateValue dateValue;
    public string subColorCode;

    public SubLayer()
    {
        subName = string.Empty;
        //dateValue = new DateValue(DateTime.Now.Year); // default to current year
        imgURLPath = string.Empty;
        subColorCode = string.Empty;
    }

    public SubLayer(string subName, string subImgURLPath, DateValue subDateValue, string subColorCode)
    {
        this.subName = subName;
        this.dateValue = subDateValue;
        this.imgURLPath = subImgURLPath;
        this.subColorCode = subColorCode;
    }

    public bool IsFilled()
    {
        // need to change this
        return !string.IsNullOrEmpty(subName) && !string.IsNullOrEmpty(imgURLPath) && !string.IsNullOrEmpty(subColorCode);
    }
}

[Serializable]
public class DateValue
{
    public string year;
    public string month;

    public DateValue()
    {
        year = string.Empty;
        month = string.Empty;
    }


    public DateValue(string yearVal, string monthVal)
    {
        this.year = yearVal;
        this.month = monthVal;
    }

    public string ToYearString()
    {
        return year;
    }

    public string ToMonthString()
    {
        return month;
    }

    public string ToMonthYearString()
    {
        return $"{month}/{year}";
    }
}

public enum DateType
{
    None, // index 0
    Year, 
    Month, 
    MonthYear // index 3
}

public class DataLayerJSON
{
    public string jsonURL;
    public string layerTitle;
    public string layerDesc;
    public string layerCredit;
    [Tooltip("The icon of the layer")]
    public string layerIcon;
    [Tooltip("correlating UI controller component")]
    public string layerColor;
    public SubLayer[] layerSubLayers;
    [Tooltip("The timescales related to layers")]
    public string layerDateType;
    public DateValue[] layerTimescale;

    public DataLayerJSON(string json, string title, string desc, string credit, string icon, string color, SubLayer[] subLayers, string dateType, DateValue[] time)
    {
        jsonURL = json;
        layerTitle = title;
        layerDesc = desc;
        layerCredit = credit;
        layerIcon = icon;
        layerColor = color;
        layerSubLayers = subLayers;
        layerDateType = dateType;
        layerTimescale = time;
    }

    // Initializes 10 empty sublayers since 10 is the max amount to accept
    public void InitializeDefaultValues()
    {
        layerSubLayers = new SubLayer[10];
        for (int i = 0; i < layerSubLayers.Length; i++)
        {
            layerSubLayers[i] = new SubLayer();
        }
        Debug.Log("SubLayers initialized with 10 empty SubLayer objects.");
    }
}

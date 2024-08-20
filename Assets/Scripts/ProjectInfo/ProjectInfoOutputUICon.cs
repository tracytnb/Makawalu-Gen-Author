using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Handles Project Information and Base Layer information to be displayed
 */
public class ProjectInfoOutputUICon : MonoBehaviour
{
    // Output_Item Prefabs in the Table Items Added list
    public GameObject outputBaseTitle;
    public GameObject outputBaseImg;
    // GameObjects in the table canvas
    public GameObject baseTitleTable; 
    public GameObject baseImgTable;

    void Start()
    {
        outputBaseTitle.GetComponent<OutputListItem>().outputObjects.Add(baseTitleTable);
        outputBaseImg.GetComponent<OutputListItem>().outputObjects.Add(baseImgTable);
    }

    public void UpdateOutputProjectInfo(string baseName, string basePath)
    {
        // Update list labels
        outputBaseTitle.GetComponent<OutputListItem>().outputLabel.text = baseName + " Map Title";
        outputBaseImg.GetComponent<OutputListItem>().outputLabel.text = baseName + " Map Image";
        // Update table objects
        baseTitleTable.GetComponent<TMP_Text>().text = baseName;
        baseImgTable.GetComponent<RawImage>().texture = HelperMethods.GetTexture(basePath, 1000);
    }
}

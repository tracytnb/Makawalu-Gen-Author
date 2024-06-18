using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSubDataLayer : MonoBehaviour
{
    public RectTransform scrollContent;
    public GameObject itemPrefab;

    public void AddNewItem()
    {
        // Instantiate a new instance of the item prefab
        GameObject newItem = Instantiate(itemPrefab, scrollContent);

        int addButtonIndex = scrollContent.childCount - 1;

        // Ensure that the new item is added below the previous items but above the add button
        newItem.transform.SetSiblingIndex(addButtonIndex);
    }
}

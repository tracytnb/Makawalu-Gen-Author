using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSDLItem : MonoBehaviour
{
    public RectTransform scrollContent;
    public GameObject itemPrefab;
    public List<GameObject> SDChildren = new List<GameObject>();
    public Color mainColor;

    void Start()
    {
        foreach (Transform childTransform in scrollContent)
        {
            SDChildren.Add(childTransform.gameObject);
        }
    }

    public void AddNewItem()
    {
        // Instantiate a new instance of the item prefab
        GameObject newItem = Instantiate(itemPrefab, scrollContent);

        int addButtonIndex = scrollContent.childCount - 1;

        // Ensure that the new item is added below the previous items
        newItem.transform.SetAsLastSibling();
        SDChildren.Add(newItem);
        UpdateColors();
    }

    public void UpdateColors()
    {
        int segments = SDChildren.Count - 1;
        Debug.Log(SDChildren.Count);
        float increment = 1.0f / segments;

        for (int i = 0; i < SDChildren.Count; i++)
        {
            GameObject child = SDChildren[i];
            Transform childTrans = child.transform;
            // Search for the "DLColor" GameObject within the current childTransform
            Transform dlColorTransform = childTrans.Find("DLColor");

            if (dlColorTransform != null)
            {
                Image dlImage = dlColorTransform.GetComponent<Image>();
                Color color;

                float t = (float)i / (SDChildren.Count - 1);
                color = Color.Lerp(mainColor, mainColor * 0.4f, t);

                color.a = 1.0f;
                Debug.Log(color);

                // Set the color of the Image component
                dlImage.color = color;

                Debug.Log("Updated color for DLColor under " + childTrans.name);
            }
            else
            {
                Debug.LogWarning("DLColor GameObject not found under " + childTrans.name);
            }
        }
    }
}

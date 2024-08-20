using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Default actions for an item under the output list 
 */
public class OutputListItem : MonoBehaviour
{
    public TMP_Text outputLabel;
    public GameObject visibleOn;
    public GameObject visibleOff;
    public List<GameObject> outputObjects = new List<GameObject>(); // Reference to a gameObject that will be toggled on and off in the table view
    public Button visibleButton;
    int num;

    void Start()
    {
        num = 1;

        visibleButton.onClick.AddListener(() =>
        {
            ActivateObjects();
        });
    }

    public void ActivateObjects()
    {
        if (num == 0)
        {
            // turn item on
            visibleOff.SetActive(false);
            visibleOn.SetActive(true);

            foreach (GameObject obj in outputObjects)
            {
                obj.SetActive(true);
            }
            num = 1;
        }
        else
        {
            visibleOff.SetActive(true);
            visibleOn.SetActive(false);

            foreach (GameObject obj in outputObjects)
            {
                obj.SetActive(false);
            }
            num = 0;
        }
    }
}

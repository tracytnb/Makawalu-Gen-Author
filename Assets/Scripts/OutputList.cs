using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputList : MonoBehaviour
{
    public string tagged;
    public GameObject[] items;
    public GameObject offIcon;
    public GameObject onIcon;
    public Button tabButton; // self
    int num;

    // Start is called before the first frame update
    void Start()
    {
        num = 1;
        items = GameObject.FindGameObjectsWithTag(tagged);

        tabButton = gameObject.GetComponent<Button>();

        tabButton.onClick.AddListener(() =>
        {
            ActivateAllItems();
        });
    }

    public void ActivateAllItems()
    {
        if (num == 0)
        {
            // turn item on
            offIcon.SetActive(false);
            onIcon.SetActive(true);

            foreach (GameObject obj in items)
            {
                obj.SetActive(true);
            }
            num = 1;
        }
        else
        {
            offIcon.SetActive(true);
            onIcon.SetActive(false);

            foreach (GameObject obj in items)
            {
                obj.SetActive(false);
            }
            num = 0;
        }
    }
}

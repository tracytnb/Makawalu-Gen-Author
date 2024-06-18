using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public string tagged;
    public GameObject[] items;
    public GameObject offImg;
    public GameObject onImg;
    int num;
    // Start is called before the first frame update
    void Start()
    {
        num = 1;
        items = GameObject.FindGameObjectsWithTag(tagged);
    }

    public void ActivateItem()
    {
        if (num == 0)
        {
            // turn item on
            offImg.SetActive(false);
            onImg.SetActive(true);

            foreach (GameObject obj in items)
            {
                obj.SetActive(true);
            }
            num = 1;
        }
        else
        {
            offImg.SetActive(true);
            onImg.SetActive(false);

            foreach (GameObject obj in items)
            {
                obj.SetActive(false);
            }
            num = 0;
        }
    }
}

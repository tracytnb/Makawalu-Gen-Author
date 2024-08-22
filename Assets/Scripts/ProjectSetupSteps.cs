using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectSetupSteps : MonoBehaviour
{
    public List<GameObject> projectStepButtons = new List<GameObject>();
    public Transform projectStepList;

    // Start is called before the first frame update
    void Start()
    {
        // Add to the list of buttons
        foreach(Transform obj in projectStepList)
        {
            projectStepButtons.Add(obj.gameObject);
        }

        // Turn everything off
        foreach(GameObject button in projectStepButtons)
        {
            // Add a listener to each button
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                TurnOffOtherButtons(button);
            });

            button.GetComponent<Image>().enabled = false;
        }

        // Only the first button is highlighted
        projectStepButtons[0].GetComponent<Image>().enabled = true;
    }

    public void TurnOffOtherButtons(GameObject selectedButton)
    {
        foreach(GameObject button in projectStepButtons)
        {
            // Turn off all other buttons except the one that was pressed
            button.GetComponent<Image>().enabled = (button == selectedButton);
        }
    }
}

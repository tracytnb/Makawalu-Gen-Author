using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectInfoUICon : MonoBehaviour
{
    public TMP_InputField InputProjTitle;
    public TMP_InputField InputProjDesc;
    public string projTitle;
    public string projDesc;
    public Button projInfoSaveButton;
    public GameObject projManager;

    // Start is called before the first frame update
    void Start()
    {
        projManager = GameObject.FindWithTag("ProjectManager");

        projInfoSaveButton.onClick.AddListener(() => {
            ValidateProjInfo();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValidateProjInfo()
    {
        projTitle = InputProjTitle.text;
        projDesc = InputProjDesc.text;

        if (string.IsNullOrEmpty(projTitle))
        {
            // Future: Validate for punctuations and characters that are not allowed?
            Debug.LogError("ERROR: Project Title must contain something");
            return;
            // Future: Add GUI error notice/message 
        }

        if (string.IsNullOrEmpty(projDesc))
        {
            Debug.LogError("ERROR: Project Description must contain a short description");
            return;
            // Future: Add GUI error notice/message 
        }

        // everything is filled in
        projManager.GetComponent<ProjectJSONCon>().SaveProjInfoToJSON(projTitle, projDesc);
        // SaveProjectInfo(projTitle, projDesc);
    }

    public void SaveProjectInfo(string title, string desc)
    {
        // saves the newly entered information into JSON

        Debug.Log("ENTERED SAVE PROJECT INFO: " + title + " " + desc);
        // after saving into JSON, update the GUI on the right side panel with necessary information
    }

    public void UpdateProjectView()
    {
        // Future: Updates the right panel view 
    }
}

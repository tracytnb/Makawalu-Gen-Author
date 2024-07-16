using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectInfoUICon : MonoBehaviour
{
    public TMP_InputField inputProjTitle;
    public TMP_InputField inputProjDesc;
    public string projTitle;
    public string projDesc;
    public GameObject projManager;
    public Button projInfoSaveButton;

    // Start is called before the first frame update
    void Start()
    {
        projManager = GameObject.FindWithTag("ProjectManager");

        if (projManager == null)
        {
            Debug.LogError("ERROR: Unable to find project manager");
        }
        
        projInfoSaveButton.onClick.AddListener(() => {
            ValidateProjInfo();
        });
    }

    public void ValidateProjInfo()
    {
        projTitle = inputProjTitle.text;
        projDesc = inputProjDesc.text;

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
        Debug.Log("ENTERED SAVE PROJECT INFO:\nProject Title: " + projTitle + "\nProject Desc: " + projDesc);
        projManager.GetComponent<ProjectJSONConAuth>().SaveProjInfoToJSON(projTitle, projDesc);
    }

    public void UpdateProjectView()
    {
        // Future: Updates the right panel view 
    }
}

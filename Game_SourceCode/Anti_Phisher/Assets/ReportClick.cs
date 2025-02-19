using UnityEngine;
using UnityEngine.UI;

public class ReportClick : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;

    public Button reportButton;

    void Start()
    {
        reportButton = GetComponent<Button>();

        if (reportButton != null)
        {
            reportButton.onClick.AddListener(OnButtonClick);
        }

        else
        {
            Debug.LogWarning("Report Button component was null when trying to add a listener to it");
        }

    }

    void OnButtonClick()
    {
        if (gameStateManager.encounterState == EncounterState.Response)
        {
            selectionData.responseSelection.emailReported = true;
            Debug.Log("Response - Email was reported");
            gameStateManager.encounterState = EncounterState.RFeedback;
            gameStateManager.emailDisplayed = false;
            gameStateManager.answerCheckRequired = true;
        }
        
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DeleteClick : MonoBehaviour //Buton logic to change the response selection value once clicked
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;

    public Button deleteButton;

    void Start()
    {
        deleteButton = GetComponent<Button>();

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(OnButtonClick);
        }

        else
        {
            Debug.LogWarning("Delete Button component was null when trying to add a listener to it");
        }

    }

    void OnButtonClick()
    {
        selectionData.responseSelection.emailDeleted = true;
        Debug.Log("Response - Email was deleted");
        gameStateManager.encounterState = EncounterState.Feedback;
        gameStateManager.feedbackState = FeedbackState.AnswersCheck;
    }


}

using UnityEngine;
using UnityEngine.UI;

public class DeleteClick : MonoBehaviour //Buton logic to change the response selection value once clicked
{
    GameStateManager gameStateManager;
    SelectionData selectionData;

    public Button deleteButton;

    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;
        selectionData = GameManager.Instance.selectionData;

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
        if (gameStateManager.encounterState == EncounterState.Response)
        {
            selectionData.responseSelection.emailDeleted = true;
            Debug.Log("Response - Email was deleted");
            gameStateManager.encounterState = EncounterState.RFeedback;
            gameStateManager.emailDisplayed = false;
            gameStateManager.answerCheckRequired = true;
        }
        
    }


}

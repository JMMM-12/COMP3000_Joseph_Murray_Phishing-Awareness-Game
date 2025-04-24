using UnityEngine;
using UnityEngine.UI;

public class ReplyClick : MonoBehaviour //Buton logic to change the response selection value once clicked
{
    GameStateManager gameStateManager;
    SelectionData selectionData;

    public Button replyButton;

    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;
        selectionData = GameManager.Instance.selectionData;

        replyButton = GetComponent<Button>();

        if (replyButton != null)
        {
            replyButton.onClick.AddListener(OnButtonClick);
        }

        else
        {
            Debug.LogWarning("Reply Button component was null when trying to add a listener to it");
        }

    }

    void OnButtonClick()
    {
        if (gameStateManager.encounterState == EncounterState.Response)
        {
            selectionData.responseSelection.emailReply = true;
            Debug.Log("Response - Email was replied to");
            gameStateManager.encounterState = EncounterState.RFeedback;
            gameStateManager.emailDisplayed = false;
            gameStateManager.answerCheckRequired = true;
        }
        
    }

    
}

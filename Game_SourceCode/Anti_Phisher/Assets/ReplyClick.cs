using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReplyClick : MonoBehaviour //Buton logic to change the response selection value once clicked
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;

    public Button replyButton;

    void Start()
    {
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
        selectionData.responseSelection.emailReply = true;
        Debug.Log("Response - Email was replied to");
        gameStateManager.encounterState = EncounterState.Feedback;
    }

    
}

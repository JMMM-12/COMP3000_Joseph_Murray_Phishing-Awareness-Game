using UnityEngine;
using UnityEngine.UI;

public class ChangeState : MonoBehaviour //Button OnClick logic to change to the Response phase once the Confirm Selection button is pressed
{
    public GameStateManager gameStateManager;

    public Button confirmButton;

    private void Start()
    {
        confirmButton = GetComponent<Button>();

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnButtonClick);
        }

        else
        {
            Debug.LogWarning("Button Component was null when trying to add a listener to it");
        }
    }




    void OnButtonClick()
    {
        gameStateManager.encounterState = EncounterState.IFeedback;
        gameStateManager.emailDisplayed = false;
        gameStateManager.answerCheckRequired = true;
    }
}

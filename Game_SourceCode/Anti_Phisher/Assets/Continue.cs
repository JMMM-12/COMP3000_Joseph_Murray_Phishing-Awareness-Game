using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public EncounterResults encounterResults;

    public Button continueButton;

    private void Start()
    {
        continueButton = GetComponent<Button>();

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnButtonClick);
        }

        else
        {
            Debug.LogWarning("Button Component was null when trying to add a listener to it");
        }
    }




    void OnButtonClick()
    {
        if (gameStateManager.encounterState == EncounterState.IFeedback)
        {
            gameStateManager.encounterState = EncounterState.Response;
            gameStateManager.emailDisplayed = false;
        }

        else if (gameStateManager.encounterState == EncounterState.RFeedback)
        {
            gameStateManager.dialogueStage = DialogueStage.Beginning;
            gameStateManager.encounterState = EncounterState.Unknown;
            gameStateManager.encounterActive = false;
            gameStateManager.highlightersReady = false;
            gameStateManager.highlightersTransparent = false;
            gameStateManager.EncounterNum = gameStateManager.EncounterNum + 1;
            selectionData.indicatorSelection = new IndicatorSelection();
            selectionData.responseSelection = new ResponseSelection();
            encounterResults.indicatorResults = new IndicatorResults();
            encounterResults.responseResults = new ResponseResults();
            Debug.Log("Game State was Updated to the beginning of the next encounter & dialogue was reactivated");
        }
    }
}

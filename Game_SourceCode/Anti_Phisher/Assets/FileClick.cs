using UnityEngine;
using UnityEngine.EventSystems;

public class FileClick : MonoBehaviour, IPointerClickHandler //Tracks when the file is clicked on and changes the indicator or response selection value accordingly
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks if the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (selectionData.indicatorSelection.fileSelected == false) //Checks if the file indicator is not already selected
                    {
                        selectionData.indicatorSelection.fileSelected = true; //Marks the file indicator as selected
                        Debug.Log("File indicator was selected");
                    }
                    else
                    {
                        selectionData.indicatorSelection.fileSelected = false; //Marks the file indicator as unselected
                        Debug.Log("File indicator was deselected");
                    }

                }
            }

            else if (gameStateManager.encounterState == EncounterState.Response) //Checks if the encounter is in the response state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    selectionData.responseSelection.fileDownloaded = true;
                    Debug.Log("Response - File was downloaded");
                    gameStateManager.encounterState = EncounterState.Feedback;
                }
            }
        }

    }
}

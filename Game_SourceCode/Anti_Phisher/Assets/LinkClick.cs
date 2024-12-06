using UnityEngine;
using UnityEngine.EventSystems;

public class LinkClick : MonoBehaviour, IPointerClickHandler //Tracks when the link is clicked on and changes the indicator or response selection value accordingly
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
                    if (selectionData.indicatorSelection.linkSelected == false) //Checks if the link indicator is not already selected
                    {
                        selectionData.indicatorSelection.linkSelected = true; //Marks the link indicator as selected
                        Debug.Log("Link indicator was selected");
                    }
                    else
                    {
                        selectionData.indicatorSelection.linkSelected = false; //Marks the link indicator as unselected
                        Debug.Log("Link indicator was deselected");
                    }

                }
            }

            else if (gameStateManager.encounterState == EncounterState.Response) //Checks if the encounter is in the response state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    selectionData.responseSelection.linkOpened = true;
                    Debug.Log("Response - Link was opened");
                    gameStateManager.encounterState = EncounterState.Feedback;
                }
            }
        }

    }
}

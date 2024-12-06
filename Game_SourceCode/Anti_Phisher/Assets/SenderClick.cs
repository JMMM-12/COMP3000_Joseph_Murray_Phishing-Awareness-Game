using UnityEngine;
using UnityEngine.EventSystems;

public class SenderClick : MonoBehaviour, IPointerClickHandler //Tracks when the sender indicator is clicked on and changes the selection value accordingly
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (selectionData.indicatorSelection.senderSelected == false) //Checks that the sender value is not already selected
                    {
                        selectionData.indicatorSelection.senderSelected = true; //Marks the sender as selected
                        Debug.Log("Subject text was selected");
                    }
                    else
                    {
                        selectionData.indicatorSelection.senderSelected = false; //Marks the sender as unselected
                        Debug.Log("Subject text was deselected");
                    }

                }
            }
        }

    }
}

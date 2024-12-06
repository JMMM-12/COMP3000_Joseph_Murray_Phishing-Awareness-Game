using UnityEngine;
using UnityEngine.EventSystems;

public class IntroductionClick : MonoBehaviour, IPointerClickHandler //Tracks when the introduction indicator is clicked on and changes the selection value accordingly
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
                    if (selectionData.indicatorSelection.introductionSelected == false) //Checks that the introduction value is not already selected
                    {
                        selectionData.indicatorSelection.introductionSelected = true; //Marks the introduction as selected
                        Debug.Log("Introduction text was selected");
                    }
                    else
                    {
                        selectionData.indicatorSelection.introductionSelected = false; //Marks the introduction as unselected
                        Debug.Log("Introduction text was deselected");
                    }

                }
            }
        }

    }
}

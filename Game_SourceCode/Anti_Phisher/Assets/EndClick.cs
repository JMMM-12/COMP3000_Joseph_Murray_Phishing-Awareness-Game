using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndClick : MonoBehaviour, IPointerClickHandler //Tracks when the end indicator is clicked on and changes the selection value accordingly
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public GameData gameData;

    public Image endImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true && gameStateManager.highlightersReady == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (selectionData.indicatorSelection.endSelected == false) //Checks if the end value is not already selected
                    {
                        endImage.color = gameData.selectedColor;
                        selectionData.indicatorSelection.endSelected = true; //Marks the end indicator as selected
                        Debug.Log("End text was selected");
                    }
                    else
                    {
                        endImage.color = gameData.transparentColor;
                        selectionData.indicatorSelection.endSelected = false; //Marks the end indicator as unselected
                        Debug.Log("End text was deselected");
                    }

                }
            }
        }

    }
}

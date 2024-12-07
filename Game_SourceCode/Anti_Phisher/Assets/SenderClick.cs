using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SenderClick : MonoBehaviour, IPointerClickHandler //Tracks when the sender indicator is clicked on and changes the selection value accordingly
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public GameData gameData;

    public Image senderImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true && gameStateManager.highlightersReady == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (selectionData.indicatorSelection.senderSelected == false) //Checks that the sender value is not already selected
                    {
                        senderImage.color = gameData.selectedColor;
                        selectionData.indicatorSelection.senderSelected = true; //Marks the sender as selected
                        Debug.Log("Sender text was selected");
                    }
                    else
                    {
                        senderImage.color = gameData.transparentColor;
                        selectionData.indicatorSelection.senderSelected = false; //Marks the sender as unselected
                        Debug.Log("Sender text was deselected");
                    }

                }
            }
        }

    }
}

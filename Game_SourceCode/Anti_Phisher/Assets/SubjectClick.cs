using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubjectClick : MonoBehaviour, IPointerClickHandler //Tracks when the subject indicator is clicked on and changes the selection value accordingly
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public GameData gameData;

    public Image subjectImage;

    public void OnPointerClick(PointerEventData eventData)
    { 
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true && gameStateManager.highlightersReady == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (selectionData.indicatorSelection.subjectSelected == false) //Checks that the subject value is not already selected
                    {
                        subjectImage.color = gameData.selectedColor;
                        selectionData.indicatorSelection.subjectSelected = true; //Marks the subject as selected
                        Debug.Log("Subject text was selected");
                    }
                    else
                    {
                        subjectImage.color = gameData.transparentColor;
                        selectionData.indicatorSelection.subjectSelected = false; //Marks the subject as unselected
                        Debug.Log("Subject text was deselected");
                    }
                    
                }
            }
        }
        
    }
}

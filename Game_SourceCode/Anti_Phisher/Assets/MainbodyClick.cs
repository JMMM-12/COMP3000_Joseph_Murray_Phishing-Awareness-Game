using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainbodyClick : MonoBehaviour, IPointerClickHandler //Tracks when the main body indicator is clicked on and changes the selection value accordingly
{
    GameStateManager gameStateManager;
    SelectionData selectionData;
    GameData gameData;

    public Image mainbodyImage;

    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;
        selectionData = GameManager.Instance.selectionData;
        gameData = GameManager.Instance.gameData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true && gameStateManager.highlightersReady == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (selectionData.indicatorSelection.mainbodySelected == false) //Checks that the main body value is not already selected
                    {
                        mainbodyImage.color = gameData.selectedColor;
                        selectionData.indicatorSelection.mainbodySelected = true; //Marks the main body as selected
                        Debug.Log("Main body text was selected");
                    }
                    else
                    {
                        mainbodyImage.color = gameData.transparentColor;
                        selectionData.indicatorSelection.mainbodySelected = false; //Marks the main body as unselected
                        Debug.Log("Main body text was deselected");
                    }

                }
            }
        }

    }
}

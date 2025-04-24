using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IntroductionClick : MonoBehaviour, IPointerClickHandler //Tracks when the introduction indicator is clicked on and changes the selection value accordingly
{
    GameStateManager gameStateManager;
    SelectionData selectionData;
    GameData gameData;

    public Image introductionImage;

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
                    if (selectionData.indicatorSelection.introductionSelected == false) //Checks that the introduction value is not already selected
                    {
                        introductionImage.color = gameData.selectedColor;
                        selectionData.indicatorSelection.introductionSelected = true; //Marks the introduction as selected
                        Debug.Log("Introduction text was selected");
                    }
                    else
                    {
                        introductionImage.color = gameData.transparentColor;
                        selectionData.indicatorSelection.introductionSelected = false; //Marks the introduction as unselected
                        Debug.Log("Introduction text was deselected");
                    }

                }
            }
        }

    }
}

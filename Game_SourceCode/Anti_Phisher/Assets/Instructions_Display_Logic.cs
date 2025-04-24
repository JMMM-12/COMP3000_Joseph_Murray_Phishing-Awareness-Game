using UnityEngine;

public class Instructions_Display_Logic : MonoBehaviour //Controls the activation and deactivation of the instructions box and the help button
{
    GameStateManager gameStateManager;

    public GameObject instructionsbox;
    public GameObject helpButton;


    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;
        helpButton.SetActive(false); //Both are deactivated upon game start
        instructionsbox.SetActive(false);
    }




    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks if the encounters are active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators)
            {
                if (gameStateManager.helpButtonActive == false)
                {
                    helpButton.SetActive(true);
                    gameStateManager.helpButtonActive = true;
                }
            }
            else if (gameStateManager.encounterState == EncounterState.Response)
            {
                if (gameStateManager.helpButtonActive == false)
                {
                    helpButton.SetActive(true);
                    gameStateManager.helpButtonActive = true;
                }
            }
            else
            {
                if (gameStateManager.helpButtonActive == true)
                {
                    helpButton.SetActive(false);
                    gameStateManager.helpButtonActive = false;
                }

                if(gameStateManager.instructionsDisplayed == true)
                {
                    instructionsbox.SetActive(false);
                    gameStateManager.instructionsDisplayed = false;
                }
            }
        }
    }
}

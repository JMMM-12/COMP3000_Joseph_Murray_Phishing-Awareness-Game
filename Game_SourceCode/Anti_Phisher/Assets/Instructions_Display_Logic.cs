using UnityEngine;

public class Instructions_Display_Logic : MonoBehaviour //Controls the activation and deactivation of the instructions box and the help button
{
    public GameStateManager gameStateManager;

    public GameObject instructionsbox;
    public GameObject helpButton;

    private bool instructionsAlreadyDeactivated;


    void Start()
    {
        instructionsAlreadyDeactivated = false;
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
                if (gameStateManager.instructionsDisplayed == true && instructionsAlreadyDeactivated == false)
                {
                    instructionsbox.SetActive(false);
                    gameStateManager.instructionsDisplayed = false;
                    instructionsAlreadyDeactivated = true;
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
                    instructionsAlreadyDeactivated = false;
                }
            }
        }
    }
}

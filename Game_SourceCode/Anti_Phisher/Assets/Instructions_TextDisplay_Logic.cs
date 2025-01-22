using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Instructions_TextDisplay_Logic : MonoBehaviour //Displays game instructions when required, based upon the encounter state
{
    private string fileName = "Instructions";

    private TextAsset ReadInstructions;
    private AllInstructions allInstructions;
    private Instruction instruction;

    public GameStateManager gameStateManager;

    public Text instructionsText;


    void Start()
    {
        ReadInstructions = new TextAsset();

        try
        {
            ReadInstructions = Resources.Load<TextAsset>(fileName); //Reads and stores the contents of the instructions JSON file
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message + "\nThe Instructions JSON file was not found");
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message + "\nThe Instructions JSON file could not be read");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\nAn error occured when the Instructions JSON file attempted to be read");
        }
        Debug.Log("Instruction contents were sucessfully read");


        allInstructions = JsonUtility.FromJson<AllInstructions>(ReadInstructions.text); //Deserializes the read JSON contents into a usable C# object
    }




    void Update()
    {
        if (gameStateManager.encounterActive == true && gameStateManager.instructionsTextRequired == true)
        {
            DisplayInstructionsText();
        }
    }


    private void DisplayInstructionsText()
    {
        if (gameStateManager.encounterState == EncounterState.Indicators)
        {
            instructionsText.text = allInstructions.Indicator.Text;
            gameStateManager.instructionsTextRequired = false;
        }
        else if (gameStateManager.encounterState == EncounterState.Response)
        {
            instructionsText.text = allInstructions.Response.Text;
            gameStateManager.instructionsTextRequired = false;
        }
    }





    [System.Serializable]
    public class AllInstructions
    {
        public Instruction Indicator;
        public Instruction Response;
    }


    [System.Serializable]
    public class Instruction
    {
        public string Text;
    }

}

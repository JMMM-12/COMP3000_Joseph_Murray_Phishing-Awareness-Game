using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class Dialogue_Logic : MonoBehaviour //Script to handle dynamic dialogue, text box, and Chip model changes upon player input
{
    public Text dialogueText; //Holds the reference to GameObject text field
    private string fileName = "Dialogues"; //Defines the filename for the stored dialogue
    private TextAsset Readdialogues;
    private string[] dialogues;
    private string[] chipModelIndicators;
    private string dialogue;
    private int DialoguesCount;
    private int currentDialogueIndex = 0;
    public float textCrawlDelay = 0.5f;
    private Coroutine TextCrawl; //Holds the textcrawl couritine to reference when stopping execution
    private bool firstDialogueDisplayed;
    private bool dialogueUIReady;

    public GameObject TextBox; //Stores a reference to the Text Box GameObject (Sprite Renderer)

    //References to Chip's components and sprites
    public GameObject Chip; //Stored a reference to Chip's Model (GameObject)
    public Sprite ChipSmiling;
    public Sprite ChipExcited;
    public Sprite ChipThinking;
    private SpriteRenderer ChipSpriteRenderer;
    private Transform ChipTransform;

    //Class objects for deserializing and handling JSON dialogue data
    private AllDialogues allDialoguesObj;
    private EncounterDialogues currentEncounterDialogues;

    public GameStateManager gameStateManager;
    public EncounterResults encounterResults;

    private bool dialoguesLoaded;


    void Start()
    {
        gameStateManager.gameState = GameState.Start;
        gameStateManager.dialogueStage = DialogueStage.Beginning;
        Readdialogues = new TextAsset();
        allDialoguesObj = new AllDialogues();
        currentEncounterDialogues = new EncounterDialogues();
        dialoguesLoaded = false;
        firstDialogueDisplayed = false;
        dialogueUIReady = true;

        dialogueText = GetComponent<Text>(); //Retrieves the Text object reference for this text (legacy) GameObject

        ChipSpriteRenderer = Chip.GetComponent<SpriteRenderer>(); //Retrieves a reference to Chip's Sprite Renderer component, for changing Chip's sprite
        
        ChipTransform = Chip.GetComponent<Transform>(); //Retrieves a reference to Chip's Transform component, for changing Chip's in-game position

        try
        {
            Readdialogues = Resources.Load<TextAsset>(fileName); //Reads and stores the contents of the dialogue JSON file - does not need to be read again
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message + "\nThe Dialogues JSON file was not found");
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message + "\nThe Dialogues JSON file could not be read");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\nAn error occured when the Dialogues JSON file attempted to be read");
        }
        Debug.Log("Dialogues were sucessfully read");


        allDialoguesObj = JsonUtility.FromJson<AllDialogues>(Readdialogues.text); //Deserializes and assigns the dialogues JSON object into the C# class object for use
        Debug.Log("Dialogues were sucessfully deserialized");

        currentEncounterDialogues = LoadEncounterDialogues(gameStateManager.EncounterNum); //Loads the dialogue for the current encounter

        DialoguesCount = CountDialogues(currentEncounterDialogues); //Counts the number of dialouges in the current encounter's specific part (start, middle, or feedback).

        //Initializes the 2D arrays to contain the size for the required number of dialogues
        chipModelIndicators = new string[DialoguesCount];
        dialogues = new string[DialoguesCount];

        AssignDialogue(); //Assigns the next dialogues to display, and the Chip Model Indicators to use

        dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex); //Retrieves the next dialogue
        Debug.Log("Dialogue after retrieval was: " + dialogue);
        if (dialogue != null) //Checks if the dialogue is null
        {
            TextCrawl = StartCoroutine(DisplayDialogue(dialogue, dialogueText, textCrawlDelay)); //Changes the on-screen text to the retrieved dialogue, appearing as a text crawl (runs in a coroutine to prevent main thread blocking)
            ChangeChipsModel(chipModelIndicators, currentDialogueIndex, ChipSpriteRenderer);
            currentDialogueIndex++;
            Debug.Log("Next dialogue text crawl has begun");
            firstDialogueDisplayed = true;
        }

        else
        {
            Debug.LogWarning("Dialogue Was Null");
        }

        gameStateManager.dialogueActive = true;
    }




    void Update()
    {
        if (gameStateManager.dialogueActive == true) //Checks if the game's dialogue is active
        {
            if (dialogueUIReady == false) //Checks if the dialogue UI (Text Box and Chip) are not displayed/formatted
            {
                TextBox.SetActive(true); //Ensures the Text Box returns during dialogue
                RescaleChipsModel(ChipSpriteRenderer, ChipTransform); //Ensures Chip's returns to its original size during dialogue
                dialogueUIReady = true;
            }

            
            if (firstDialogueDisplayed == true) //Checks if the first dilaogue entry for this encounter has already been displayed
            {
                if (Input.touchCount == 1 || Input.anyKeyDown == true) //Checks if player touch is detected (anyKeyDown input for testing purposes)
                {
                    if (currentDialogueIndex <= DialoguesCount - 1) //Checks if the current dialogue index is lower than the total number of dialogues 
                    {
                        if (dialogueText.text == dialogue) //Checks if all the dialogue has been displayed i.e. the text crawl has ended - then the next dialogue text crawl can start
                        {
                            dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex); //Retrieves the next dialogue
                            Debug.Log("Dialogue after retrieval was: " + dialogue);
                            if (dialogue != null) //Checks if the dialogue is null
                            {
                                TextCrawl = StartCoroutine(DisplayDialogue(dialogue, dialogueText, textCrawlDelay)); //Changes the on-screen text to the retrieved dialogue
                                ChangeChipsModel(chipModelIndicators, currentDialogueIndex, ChipSpriteRenderer);
                                currentDialogueIndex++;
                                Debug.Log("Next dialogue text crawl has begun");
                            }

                            else
                            {
                                Debug.LogWarning("Dialogue Was Null");
                            }

                        }

                        else if (dialogueText.text != dialogue) //Checks if all the dialogue has not been displayed i.e. the text crawl is still running
                        {
                            StopCoroutine(TextCrawl); //Stops the text crawl coroutine
                            dialogueText.text = dialogue; //displays the full dialogue to the player
                            Debug.Log("Text Crawl was stopped and full dialogue was displayed");
                        }
                    }

                    else if (dialogueText.text != dialogue) //If the dialogue has finished but there is still a text crawl in progress
                    {
                        StopCoroutine(TextCrawl); //Stops the text crawl coroutine
                        dialogueText.text = dialogue; //displays the full dialogue to the player
                        Debug.Log("Text Crawl was stopped and full dialogue was displayed");
                    }

                    else //If the dialogue has finished
                    {
                        gameStateManager.dialogueActive = false; //Deactivates the dialogue until future requirement
                        StopCoroutine(TextCrawl);
                        dialogueText.text = ""; //Clears the on screen text
                        TextBox.SetActive(false); //Deactivates the Text Box, removing its display from the screen
                        RescaleChipsModel(ChipSpriteRenderer, ChipTransform); //Minimizes Chip's model
                        dialogueUIReady = false;
                        currentDialogueIndex = 0;
                        DialoguesCount = 0;
                        Debug.Log("Dialogue has Ended");
                        gameStateManager.encounterActive = true; //Actives the encounter gameplay
                        UpdateGameState(); //Updates the state of the game, and loads new encounter dialogue once the encounter state has looped
                        dialoguesLoaded = false;
                        firstDialogueDisplayed = false;
                    }
                }
            }

            else //If the first dialogue for this encounter has not yet been displayed, then display it immediately
            {
                dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex); //Retrieves the next dialogue
                Debug.Log("Dialogue after retrieval was: " + dialogue);
                if (dialogue != null) //Checks if the dialogue is null
                {
                    TextCrawl = StartCoroutine(DisplayDialogue(dialogue, dialogueText, textCrawlDelay)); //Changes the on-screen text to the retrieved dialogue, appearing as a text crawl
                    ChangeChipsModel(chipModelIndicators, currentDialogueIndex, ChipSpriteRenderer);
                    currentDialogueIndex++;
                    Debug.Log("Next dialogue text crawl has begun");
                    firstDialogueDisplayed = true;
                }

                else
                {
                    Debug.LogWarning("Dialogue Was Null");
                }
            }
        }



        else
        {
            if (dialoguesLoaded == false)
            {
                if (gameStateManager.encounterState == EncounterState.Unknown && gameStateManager.dialogueStage == DialogueStage.Beginning)
                {
                    DialoguesCount = CountDialogues(currentEncounterDialogues); //Counts the number of dialouges in the current encounter's specific part (start, middle, or feedback).
                    chipModelIndicators = new string[DialoguesCount]; //Initializes the 2D arrays to contain the size for the required number of dialogues
                    dialogues = new string[DialoguesCount];
                    AssignDialogue(); //Assigns the next dialogues to display, and the Chip Model Indicators to use
                    dialoguesLoaded = true;
                    gameStateManager.dialogueActive = true;
                }
            }
            
        }
    }




    string retrieveNextDialogue(string[] dialogues, int dialogueIndex) //Method for retrieving the dialogue from the array based on the provided index
    {
        string newDialogue = null;
        try
        {
            newDialogue = dialogues[dialogueIndex]; //Retrieves and stores the next dialogue
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e.Message + "\nDialogue retrieval went outside of the index range");
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message + "\nDialogue retrieval encountered an Error");
        }

        return newDialogue;


    }




    void ChangeChipsModel(string[] indicators, int dialogueIndex, SpriteRenderer chipSpriteRenderer) //Changes Chip's SpriteRenderer Sprite value to match the required one specified for the dialogue
    {
        try
        {
            if (indicators[dialogueIndex] == "S") //Checks the desired sprite for the current dialogue
            {
                chipSpriteRenderer.sprite = ChipSmiling; //Changes Chip's sprite
            }

            else if (indicators[dialogueIndex] == "E")
            {
                chipSpriteRenderer.sprite = ChipExcited;
            }

            else if (indicators[dialogueIndex] == "T")
            {
                chipSpriteRenderer.sprite = ChipThinking;
            }

            else
            {
                Debug.LogWarning("Chip's model indicator was of an unknown type. Chip's model was set to default (Smiling)");
            }
        }

        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e.Message + "\n Chip's model indicator check went outside the index range");
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message + "\n Chip's model change encountered an error");
        }
    }




    void RescaleChipsModel (SpriteRenderer chipSpriteRenderer, Transform chipTransform) //Rescales Chip's model based upon whether the dialogue is active
    {
        if (gameStateManager.dialogueActive == true) //If dialogue is currently active, Chip's model should scale to its normal size
        {
            chipTransform.localScale = new Vector3(1.3541f, 1.3541f, 1.3541f); //Re-scales Chip to its default size
            chipTransform.position = new Vector3(-10.62f, -2.48f, 0f); //Moves Chip to its original position
        }

        else //If dialogue is currently inactive, Chip's model should scale down to the corner of the screen
        {
            chipSpriteRenderer.sprite = ChipSmiling; //Sets Chip's Sprite to Smiling
            chipTransform.localScale = new Vector3(0.45f, 0.45f, 0.45f); //De-scales Chip to a smaller size
            chipTransform.position = new Vector3(-18f, -8.5f, 0f); //Moves Chip to the corner of the screen
        }
        
    }




    IEnumerator DisplayDialogue (string dialogueToDisplay, Text dialogueDisplay, float delay) //Displays the dialogue character-by-character on screen to generate a text crawl effect
    {
        string displayDialogue;

        for (int i = 0; i < dialogueToDisplay.Length + 1; i++) //Lopps through every character in the dialogue to display
        {
            displayDialogue = dialogueToDisplay.Substring(0, i); //Creates a substring of the dialogue to display that crawls (increases) by 1 character every loop
            dialogueDisplay.text = displayDialogue; //Displays the dialogue at its current stage in the text crawl
            yield return new WaitForSeconds(delay); //Pauses the coroutine for {delay} seconds, ensuring the text crawl is of an appropriate and consistent speed
        }
    }




    EncounterDialogues LoadEncounterDialogues (int encounterNumber) //Loads an Encounter's dialogue based upon the game's current encounter state
    {
        EncounterDialogues encounterDialogues = new EncounterDialogues();
        if (encounterNumber == 0)
        {
            encounterDialogues = allDialoguesObj.Tutorial;
        }

        else if (encounterNumber == 1)
        {
            encounterDialogues = allDialoguesObj.Encounter1;
        }

        else if (encounterNumber == 2)
        {
            encounterDialogues = allDialoguesObj.Encounter2;
        }

        else if (encounterNumber == 3)
        {
            encounterDialogues = allDialoguesObj.Encounter3;
        }

        else if (encounterNumber == 4)
        {
            encounterDialogues = allDialoguesObj.Encounter4;
        }

        else if (encounterNumber == 5)
        {
            encounterDialogues = allDialoguesObj.Encounter5;
        }

        else if (encounterNumber == 6)
        {
            encounterDialogues = allDialoguesObj.Encounter6;
        }

        else if (encounterNumber == 7)
        {
            encounterDialogues = allDialoguesObj.Encounter7;
        }

        else if (encounterNumber == 8)
        {
            encounterDialogues = allDialoguesObj.Encounter8;
        }

        else if (encounterNumber == 9)
        {
            encounterDialogues = allDialoguesObj.Encounter9;
        }

        else if (encounterNumber == 10)
        {
            encounterDialogues = allDialoguesObj.Encounter10;
        }

        return encounterDialogues;
    }




    void UpdateGameState() //Updates the state of the game's encounter for the next dialogue display, based upon the current encounter state
    {
        if (gameStateManager.dialogueStage == DialogueStage.Beginning)
        {
            currentEncounterDialogues = LoadEncounterDialogues(gameStateManager.EncounterNum + 1); //Pre-loads the next set of encounter dialogues
            gameStateManager.encounterState = EncounterState.Indicators;
            gameStateManager.dialogueStage = DialogueStage.Inactive;
        }
    }




    int CountDialogues(EncounterDialogues dialoguesToCount) //Find the number of dialogues that must be displayed, based upon the encounter state 
    {
        int count = 0;
        if (gameStateManager.dialogueStage == DialogueStage.Beginning)
        {
            foreach (var d in dialoguesToCount.Start) //Loop to find the number of dialogues in the Start section
            {
                count++;
                Debug.Log("Dialogue was counted, current count: " + count.ToString());
            }
        }

        else if (gameStateManager.dialogueStage == DialogueStage.Unknown)
        {
            Debug.LogError("GameState was Unknown");
        }

        return count;
    }



    void AssignDialogue() //Assigns the Dialogues and Chip Model Indicators arrays with the values from the encounter dialogues, based upon the game state
    {
        int z = 0;
        if (gameStateManager.dialogueStage == DialogueStage.Beginning)
        {
            foreach (var d in currentEncounterDialogues.Start) //Loop to assign the Start dialogues and chip model indicators to their respective arrays
            {
                dialogues[z] = d.DialogueText.ToString();
                chipModelIndicators[z] = d.ChipModel.ToString();
                z++;
            }
        }

        else if (gameStateManager.dialogueStage == DialogueStage.Unknown)
        {
            Debug.LogError("GameState was Unknown");
        }
    }
}




[System.Serializable]
public class Dialogue //Contains a single dialogue (C# equivalent to a JSON object)
{
    public int id;
    public string ChipModel;
    public string DialogueText;

    public Dialogue(string newModel, string newText)
    {
        ChipModel = newModel;
        DialogueText = newText;
    }
}




[System.Serializable]
public class EncounterDialogues //Contains lists of dialogues for each part of an encounter - making up the dialogue for an entire encounter
{
    public List<Dialogue> Start;
    public List<Dialogue> Feedback;
}




[System.Serializable]
public class AllDialogues //Contains the entire game's dialogue values
{
    public EncounterDialogues Tutorial;
    public EncounterDialogues Encounter1;
    public EncounterDialogues Encounter2;
    public EncounterDialogues Encounter3;
    public EncounterDialogues Encounter4;
    public EncounterDialogues Encounter5;
    public EncounterDialogues Encounter6;
    public EncounterDialogues Encounter7;
    public EncounterDialogues Encounter8;
    public EncounterDialogues Encounter9;
    public EncounterDialogues Encounter10;
}
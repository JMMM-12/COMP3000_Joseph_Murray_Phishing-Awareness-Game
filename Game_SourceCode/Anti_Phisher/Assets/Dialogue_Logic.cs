using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;

public class Dialogue_Logic : MonoBehaviour //Script to handle dialogue, text box, and Chip model changes upon player input
{
    public Text dialogueText; //Holds the reference to GameObject text field
    private string fileName = "Dialogues"; //Defines the filename for the stored dialogue
    private TextAsset Readdialogues;
    private string[] dialogues;
    private string[] chipModelIndicators;
    private string dialogue;
    private int DialoguesCount;
    private int currentDialogueIndex = 0;
    public bool dialogueEnded = false;
    public float textCrawlDelay = 0.5f;
    private Coroutine TextCrawl; //Holds the textcrawl couritine to reference when stopping execution

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
    private EncounterDialogues encounterDialoguesObj;
    private Dialogue dialogueObj;


    void Start()
    { 
        Readdialogues = new TextAsset();
        allDialoguesObj = new AllDialogues();
        encounterDialoguesObj = new EncounterDialogues();
        dialogueObj = new Dialogue();

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

        if (allDialoguesObj == null)
        {
            Debug.LogError("allDialoguesObj was null");
        }
        else if (encounterDialoguesObj == null)
        {
            Debug.LogError("encounterDialoguesObj was null");
        }
        else if (dialogueObj == null)
        {
            Debug.LogError("dialogueObj was null");
        }


        foreach (var d in allDialoguesObj.Tutorial.Start) //Loop to find the number of dialogues in the Tutorial Start sections
        {
            DialoguesCount++;
        }

        //Initializes the 2D arrays to contain the current required number of dialogues
        chipModelIndicators = new string[DialoguesCount];
        dialogues = new string[DialoguesCount];

        int z = 0;
        foreach (var d in allDialoguesObj.Tutorial.Start) //Loop to assign the Tutorial Start dialogues and chip model indicators to their respective arrays
        {
            dialogues[z] = d.DialogueText.ToString();
            chipModelIndicators[z] = d.ChipModel.ToString();
            z++;
        }

        dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex); //Retrieves the first dialogue
        TextCrawl = StartCoroutine(DisplayDialogue(dialogue, dialogueText, textCrawlDelay)); //Changes the display text to Chip's first dialogue, appearing as a text crawl (runs in a coroutine to prevent main thread blocking)
        ChangeChipsModel(chipModelIndicators, currentDialogueIndex, ChipSpriteRenderer);
        currentDialogueIndex++; //Increases the dialogue index, ensuring the next dialgoue retrieval sucessfuly retrieves the next dialogue
    }




    void Update()
    {
        if (Input.touchCount == 1 || Input.anyKeyDown == true) //Checks if player touch is detected (anyKeyDown input for testing purposes)
        {
            Debug.Log("Input Detected");

            if (dialogueText.text == dialogue) //Checks if all the dialogue has been displayed i.e. the text crawl has ended - then the next dialogue text crawl can start
            {
                if (currentDialogueIndex <= DialoguesCount - 1) //Checks if the current dialogue index is lower than the total number of dialogues
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

                else
                {
                    dialogueEnded = true;
                    dialogueText.text = ""; //Clears the text once the dialogues have finished
                    TextBox.SetActive(false); //Deactivates the Text Box once dialogue has finished, removing its display from the screen
                    MinimizeChipsModel(ChipSpriteRenderer, ChipTransform); //Minimizes Chip's model
                    Debug.Log("Dialogue has Ended");
                }
            }

            else if (dialogueText.text != dialogue) //Checks if all the dialogue has not been displayed i.e. the text crawl is still running - then the text crawl should stop and the full dialogue should be displayed
            {
                StopCoroutine(TextCrawl); //Stops the text crawl coroutine
                dialogueText.text = dialogue; //displays the full dialogue to the player
                Debug.Log("Text Crawl was stopped and full dialogue was displayed ");
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




    void MinimizeChipsModel (SpriteRenderer chipSpriteRenderer, Transform chipTransform) //Minimizes Chip's model to the corner of the screen
    {
        chipSpriteRenderer.sprite = ChipSmiling; //Sets Chip's Sprite to Smiling
        chipTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f); //De-scales Chip to a smaller size
        chipTransform.position = new Vector3(-17f, -7.5f, 0f); //Moves Chip to the corner of the screen
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


}




[System.Serializable]
public class Dialogue //Contains a single dialogue (C# equivalent to a JSON object)
{
    public int id;
    public string ChipModel;
    public string Text;
    public string DialogueText;
    public string FeedbackType;
}




[System.Serializable]
public class EncounterDialogues //Contains lists of dialogues for each part of an encounter - making up the dialogue for an entire encounter
{
    public List<Dialogue> Start;
    public List<Dialogue> MidGame;
    public List<Dialogue> End;
}




[System.Serializable]
public class AllDialogues //Contains the entire game's dialogue values
{
    public EncounterDialogues Tutorial;
}





public enum EncounterState //Enums for tracking the state of the game's current encounter
{
    Beginning,
    Middle,
    Feedback,
    Unknown
}
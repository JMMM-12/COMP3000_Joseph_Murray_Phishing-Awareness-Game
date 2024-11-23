using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Collections;

public class Dialogue_Logic : MonoBehaviour //Script to handle dialogue, text box, and Chip model changes upon player input
{
    public Text dialogueText; //Holds the reference to GameObject text field
    private string filePath = "Assets/Dialogue_Texts/Level1.txt"; //Defines the filepath for the stored dialogue texts
    private string[] Readdialogues;
    private string[] dialogues;
    private string[] chipModelIndicators;
    private string dialogue;
    private int dialoguesLength;
    private int currentDialogueIndex;
    public bool dialogueEnded = false;
    public float textCrawlDelay = 0.5f;
    private Coroutine TextCrawl; //Holds the textcrawl couritine to reference when stopping execution

    public GameObject TextBox; //Stores a reference to the Text Box GameObject (Sprite Renderer)

    public GameObject Chip; //Stored a reference to Chip's Model (GameObject)
    public Sprite ChipSmiling;
    public Sprite ChipExcited;
    public Sprite ChipThinking;
    private SpriteRenderer ChipSpriteRenderer;
    private Transform ChipTransform;


    void Start()
    { 
        dialogueText = GetComponent<Text>(); //Retrieves the Text object reference for this text (legacy) GameObject

        ChipSpriteRenderer = Chip.GetComponent<SpriteRenderer>(); //Retrieves a reference to Chip's Sprite Renderer component, for changing Chip's sprite
        
        ChipTransform = Chip.GetComponent<Transform>(); //Retrieves a reference to Chip's Transform component, for changing Chip's in-game position

        try
        {
            Readdialogues = File.ReadAllLines(filePath); //Reads and stores the contents of the dialogue file (line-by-line)
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message + "\nThe Level 1 Dialogue Texts file was not found");
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message + "\nThe Level 1 Dialogue Texts file could not be read");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\nAn error occured when the Level 1 dialogue Text attempted to be read");
        }

        Debug.Log("Dialogues were sucessfully read. Second value in array: " + Readdialogues[1]);

        dialoguesLength = Readdialogues.Length; //Finds the number of values in the dialogues array
        Debug.Log("Dialogues Length is: " + dialoguesLength.ToString());

        chipModelIndicators = new string[dialoguesLength]; //Initializes the 2D arrays
        dialogues = new string[dialoguesLength];

        chipModelIndicators = ExtractChipModelIndicator(Readdialogues); //Extracts Chip's model indicators for changing Chip's model in aligment with the dialogue
        Debug.Log("Chip Model indicators were extracted, first one is: " + chipModelIndicators[0]);

        dialogues = ExtractDialogue(Readdialogues); //Extracts the dialogue text to display on screen, overwriting the dialogues array
        Debug.Log("Dialogue was Extracted, first dialogue is: " + dialogues[0]);

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
                if (currentDialogueIndex <= dialoguesLength - 1) //Checks if the current dialogue index is lower than the total number of dialogues
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
            newDialogue = dialogues[dialogueIndex]; //Retrieves and stored the next dialogue
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




    string[] ExtractDialogue(string[] dialoguesToExtract) //Extracts the displayable dialogue from the file read contents
    {
        string[] newDialogue = new string[dialoguesToExtract.Length];

        for (int i = 0; i < dialoguesToExtract.Length; i++) //Loops through each index in the file read contents
        {
            string extractedDialogue = dialoguesToExtract[i];
            extractedDialogue = extractedDialogue.Substring(1);
            newDialogue[i] = extractedDialogue; //extracts the displayable dialogue content into a new array
        }

        return newDialogue;
    }




    string[] ExtractChipModelIndicator(string[] dialoguesToExtract) //Extracts Chip's model indicators from the file read contents
    {
        string[] indicators = new string[dialoguesToExtract.Length];

        for (int i = 0;i < dialoguesToExtract.Length; i++)
        {
            string extractedDialogue = dialoguesToExtract[i]; //Stores the current read text file line in a new string
            indicators[i] = extractedDialogue[0].ToString(); //Adds the first character of this new string (the indicator) to the indicators array
        }

        return indicators;
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
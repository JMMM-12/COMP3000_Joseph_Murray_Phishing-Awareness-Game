using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Dialogue_Logic : MonoBehaviour
{
    public Text dialogueText; //Holds the reference to GameObject text field
    private string filePath = "Assets/Dialogue_Texts/Level1.txt"; //Defines the filepath for the stored dialogue texts
    private string[] dialogues;
    private string dialogue;
    private int dialoguesLength;
    private int currentDialogueIndex;
    public bool dialogueEnded = false;
    void Start()
    { 
        dialogueText = GetComponent<Text>(); //Retrieves the Text object reference for this text (legacy) GameObject
        
        try
        {
            dialogues = File.ReadAllLines(filePath); //Reads and stores the contents of the dialogue file (line-by-line)
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

        Debug.Log("Dialogues were sucessfully read. Second value in array: " + dialogues[1]);

        dialoguesLength = dialogues.Length; //Find the number of values in the dialogues array

        dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex); //Retrieves the first dialogue
        currentDialogueIndex++; //Increases the dialogue index, ensuring the next dialgoue retrieval sucessfuly retrieves the next dialogue

        dialogueText.text = dialogue; //Changes the text to Chip's first dialogue
    }




    void Update()
    {
        if (Input.touchCount == 1 || Input.anyKey == true) //Checks if player touch is detected (anyKey input for testing purposes)
        {
            Debug.Log("Input Detected");
            if (currentDialogueIndex <= dialoguesLength - 1) //Checks if the current dialogue index exceeds the total number of dialogues
            {
                dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex); //Retrieves the next dialogue
                Debug.Log("Dialogue after retrieval was: " + dialogue);
                if (dialogue != null) //Checks if the dialogue is null
                {
                    Debug.Log("Dialogue Was not Null");
                    currentDialogueIndex++;
                    dialogueText.text = dialogue; //Changes the on-screen text to the retrieved dialogue
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
                Debug.Log("Dialogue has Ended");
            }
        }
    }




    string retrieveNextDialogue(string[] dialogues, int dialogueIndex) //Method for retrieving the dialogue from the array based on the provided index
    {
        string newDialogue = null;
        try
        {
            newDialogue = dialogues[dialogueIndex];
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
}

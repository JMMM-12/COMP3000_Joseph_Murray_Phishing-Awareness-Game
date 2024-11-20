using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Dialogue_Logic : MonoBehaviour
{
    public Text dialogueText; //Holds the reference to GameObject text field
    private string filePath = "Assets/Dialogue_Texts/Level1"; //Defines the filepath for the stored dialogue texts
    private string[] dialogues;
    private string dialogue;
    private int dialoguesLength;
    private int currentDialogueIndex;
    private bool dialogueEnded = false;
    void Start()
    {
        //SET A COUNTER FOR NUMBER OF INPUTS DETECTED - So that when it reaches a certain number (or zero) the gameobject can dissapear/change?
        
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

        dialoguesLength = dialogues.Length; //Find the number of values in the dialogues array

        dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex); //Retrieves the first dialogue
        currentDialogueIndex++; //Increases the dialogue index, ensuring the next dialgoue retrieval sucessfuly retrieves the next dialogue

        dialogueText.text = dialogue; //Changes the text to Chip's first dialogue
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 || Input.anyKey == true) //Checks if player touch is detected (anyKey input for testing purposes)
        {
            if (currentDialogueIndex >= dialoguesLength - 1) //Checks if the current dialogue index exceeds the total number of dialogues
            {
                dialogue = retrieveNextDialogue(dialogues, currentDialogueIndex);
                currentDialogueIndex++;
                dialogueText.text = dialogue;
            } 
        }
    }

    string retrieveNextDialogue(string[] dialogues, int dialogueIndex)
    {
        string newDialogue = null;
        try
        {
            newDialogue = dialogues[dialogueIndex];
        }
        catch (IndexOutOfRangeException e)
        {

        }
        catch(Exception e)
        {

        }

        return newDialogue;


    }
}

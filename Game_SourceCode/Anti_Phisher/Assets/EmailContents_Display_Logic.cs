using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EmailContents_Display_Logic : MonoBehaviour //Handles the dynamic loading & display of the current encounter's email contents
{
    private string fileName = "Emails"; //Declares the file name for the stored email contents

    //For tracking the game states
    GameStateManager gameStateManager;

    //For reading from and storing the email contents
    private TextAsset ReadEmails;
    private AllEmails allEmails;
    private Email email;

    //For storing references to the text components to modify
    public Text Subject;
    public Text Sender;
    public Text Introduction;
    public Text MainBody;
    public Text Link;
    public Text End;
    public Text File;

    public GameObject LinkObj;

    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;

        ReadEmails = new TextAsset();

        try
        {
            ReadEmails = Resources.Load<TextAsset>(fileName); //Reads and stores the contents of the emails JSON file
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message + "\nThe Emails JSON file was not found");
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message + "\nThe Emails JSON file could not be read");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\nAn error occured when the Email JSON file attempted to be read");
        }
        Debug.Log("Email contents were sucessfully read");


        allEmails = JsonUtility.FromJson<AllEmails>(ReadEmails.text); //Deserializes the read JSON contents into a usable C# object

        email = LoadNextEmail(); //Loads the next encounter's email contents
    }




    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks if the encounter gameplay is active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators)
            {
                if (gameStateManager.emailContentsDisplayed == false && gameStateManager.emailDisplayed == true) //Checks if the contents for the current email have not already been shown
                {
                    email = LoadNextEmail();
                    DisplayEmailContents();
                    gameStateManager.emailContentsDisplayed = true;
                }
            }
            
        }

        else if (gameStateManager.encounterActive == false)
        {
            gameStateManager.emailContentsDisplayed = false;
        }


        else
        {
            Debug.LogWarning("Game encounter active state was unknown");
        }
    }




    Email LoadNextEmail() //Loads the next encounter's email contents
    {
        Email nextEmail = new Email();

        switch (gameStateManager.EncounterNum)
        {
            case 0:
                nextEmail = allEmails.Tutorial;
                break;
            case 1:
                nextEmail = allEmails.Encounter1;
                break;
            case 2:
                nextEmail = allEmails.Encounter2;
                break;
            case 3:
                nextEmail = allEmails.Encounter3;
                break;
            case 4:
                nextEmail = allEmails.Encounter4;
                break;
            case 5:
                nextEmail = allEmails.Encounter5;
                break;
            case 6:
                nextEmail = allEmails.Encounter6;
                break;
            case 7:
                nextEmail = allEmails.Encounter7;
                break;
            case 8:
                nextEmail = allEmails.Encounter8;
                break;
            case 9:
                nextEmail = allEmails.Encounter9;
                break;
            case 10:
                nextEmail = allEmails.Encounter10;
                break;
            default:
                Debug.LogError("Next Email could not be loaded, since it was unknown");
                break;
        }

        return nextEmail;
    }


    void DisplayEmailContents()
    {
        try
        {
            Subject.text = email.Subject;
            Sender.text = email.Sender;
            Introduction.text = email.Introduction;
            MainBody.text = email.MainBody;
            if (email.Link.ToString() != "None")
            {
                Link.text = email.Link;
            }
            else
            {
                LinkObj.SetActive(false);
            }
            End.text = email.End;
            File.text = email.File;
        }
        catch (Exception e)
        {
            Debug.LogError("Email Contents could not be displayed to the screen. Likley becuase the text GameObjects are inactive: " + e);
        }
        
    }
}





[System.Serializable]
public class AllEmails //For storing the entire JSON file contents
{
    public Email Tutorial;
    public Email Encounter1;
    public Email Encounter2;
    public Email Encounter3;
    public Email Encounter4;
    public Email Encounter5;
    public Email Encounter6;
    public Email Encounter7;
    public Email Encounter8;
    public Email Encounter9;
    public Email Encounter10;
}


[System.Serializable]
public class Email //For storing a single email's contents
{
    public int id;
    public string Subject;
    public string Sender;
    public string Introduction;
    public string MainBody;
    public string Link;
    public string End;
    public string File;
}

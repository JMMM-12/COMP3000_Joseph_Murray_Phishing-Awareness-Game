using System.IO;
using System;
using UnityEngine;

public class Answers_Checking : MonoBehaviour
{
    private string fileName = "Answers";
    private TextAsset ReadAnswers;
    private AllAnswers allAnswers;
    private EncounterAnswers encounterAnswers;
    private IndicatorAnswers indicatorAnswers;
    private ResponseAnswers responseAnswers;


    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public EncounterResults encounterResults;

    void Start()
    {
        ReadAnswers = new TextAsset();

        try
        {
            ReadAnswers = Resources.Load<TextAsset>(fileName); //Reads and stores the contents of the answers JSON file
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message + "\nThe Answers JSON file was not found");
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message + "\nThe Answers JSON file could not be read");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\nAn error occured when the Answers JSON file attempted to be read");
        }
        Debug.Log("Answer contents were sucessfully read");


        allAnswers = JsonUtility.FromJson<AllAnswers>(ReadAnswers.text); //Deserializes the read JSON contents into a usable C# object
    }





    void Update()
    {
        if (gameStateManager.encounterState == EncounterState.Feedback && gameStateManager.feedbackState == FeedbackState.AnswersCheck) //Checks that the game is in the answers checking stage of the feedback phase 
        {
            LoadNextAnswers(); //Loads the answers for the current scenario
            IndicatorsCheck(); //Checks all the selected indicators against the correct indicator answers, determining the indicator results
            ResponsesCheck(); //Checks all the selected responses against the correct response answers, determining the response results
            IndicatorsGrading();
        }
    }





    public void LoadNextAnswers() //Loads the next encounter's answers & assigns the indicator and response answers seperately
    {
        switch (gameStateManager.EncounterNum)
        {
            case 0:
                encounterAnswers = allAnswers.Tutorial;
                break;
            case 1:
                encounterAnswers = allAnswers.Encounter1;
                break;
            case 2:
                encounterAnswers = allAnswers.Encounter2;
                break;
            default:
                Debug.LogError("Next Email could not be loaded, since it was unknown");
                break;
        }

        indicatorAnswers = encounterAnswers.Indicators;
        responseAnswers = encounterAnswers.Response;

    }





    public void IndicatorsCheck() //Checks the results of every indicator individually & calculates the results
    {
        if (selectionData.indicatorSelection.subjectSelected != indicatorAnswers.Subject) //Checks if the value for subject selected and subject answer are not equal (the player has made a mistake on the subject indicator)
        {
            if (selectionData.indicatorSelection.subjectSelected == true) //Checks if the subject selected value is true, therefore the subject answer must be false (the player incorrectly identified an indicator)
            {
                encounterResults.indicatorResults.subjectResult = Result.FalsePositive; //Sets the subject result as a false positive
                encounterResults.indicatorResults.incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators + 1; //Increases the incorrectly identified indicators count by 1
                encounterResults.indicatorResults.indicatorsIncorrect = true; //Sets the indicator incorrectly identified value to true
            }
            else if (selectionData.indicatorSelection.subjectSelected == false) //Checks if the subject selected value is false, therefore the subject answer must be true (the player missed an indicator)
            {
                encounterResults.indicatorResults.subjectResult = Result.FalseNegative; //Sets the subject result as a false negative
                encounterResults.indicatorResults.missedIndicators = encounterResults.indicatorResults.missedIndicators + 1; //Increases the missed indicators count by 1
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1; //Increases the total indicators count by 1
            }
        }

        else if (selectionData.indicatorSelection.subjectSelected == indicatorAnswers.Subject) //Checks if the value for subject selected and subject answer are equal (the player has made a correct selection on the subject indicator)
        {
            if (selectionData.indicatorSelection.subjectSelected == true) //Checks if the subject selected value is true, therefore the subject answer must also be true (the player correctly identified an indicator)
            {
                encounterResults.indicatorResults.subjectResult = Result.TruePositive; //Sets the subject result as a true positive
                encounterResults.indicatorResults.correctIndicators = encounterResults.indicatorResults.correctIndicators + 1; //Increases the correct indicators count by 1
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1; //Increases the total indicators count by 1
            }
            else if (selectionData.indicatorSelection.subjectSelected == false) //Checks if the subject selected value is false, therefore the subject answer must also be false (the player correctly avoided selecting a non-existent indicator)
            {
                encounterResults.indicatorResults.subjectResult = Result.TrueNegative; //Sets the subject result as a true negative
            }
        }


        //Above answer checking & results calculating process for subject indicator is repeated for all remaining indicators
        if (selectionData.indicatorSelection.senderSelected != indicatorAnswers.Sender)
        {
            if (selectionData.indicatorSelection.senderSelected == true)
            {
                encounterResults.indicatorResults.senderResult = Result.FalsePositive;
                encounterResults.indicatorResults.incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators + 1;
                encounterResults.indicatorResults.indicatorsIncorrect = true;
            }
            else if (selectionData.indicatorSelection.senderSelected == false)
            {
                encounterResults.indicatorResults.senderResult = Result.FalseNegative;
                encounterResults.indicatorResults.missedIndicators = encounterResults.indicatorResults.missedIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
        }

        else if (selectionData.indicatorSelection.senderSelected == indicatorAnswers.Sender)
        {
            if (selectionData.indicatorSelection.senderSelected == true)
            {
                encounterResults.indicatorResults.senderResult = Result.TruePositive;
                encounterResults.indicatorResults.correctIndicators = encounterResults.indicatorResults.correctIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
            else if (selectionData.indicatorSelection.senderSelected == false)
            {
                encounterResults.indicatorResults.senderResult = Result.TrueNegative;
            }
        }



        if (selectionData.indicatorSelection.introductionSelected != indicatorAnswers.Introduction)
        {
            if (selectionData.indicatorSelection.introductionSelected == true)
            {
                encounterResults.indicatorResults.introductionResult = Result.FalsePositive;
                encounterResults.indicatorResults.incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators + 1;
                encounterResults.indicatorResults.indicatorsIncorrect = true;
            }
            else if (selectionData.indicatorSelection.introductionSelected == false)
            {
                encounterResults.indicatorResults.introductionResult = Result.FalseNegative;
                encounterResults.indicatorResults.missedIndicators = encounterResults.indicatorResults.missedIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
        }

        else if (selectionData.indicatorSelection.introductionSelected == indicatorAnswers.Introduction)
        {
            if (selectionData.indicatorSelection.introductionSelected == true)
            {
                encounterResults.indicatorResults.introductionResult = Result.TruePositive;
                encounterResults.indicatorResults.correctIndicators = encounterResults.indicatorResults.correctIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
            else if (selectionData.indicatorSelection.introductionSelected == false)
            {
                encounterResults.indicatorResults.introductionResult = Result.TrueNegative;
            }
        }



        if (selectionData.indicatorSelection.mainbodySelected != indicatorAnswers.MainBody)
        {
            if (selectionData.indicatorSelection.mainbodySelected == true)
            {
                encounterResults.indicatorResults.mainbodyResult = Result.FalsePositive;
                encounterResults.indicatorResults.incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators + 1;
                encounterResults.indicatorResults.indicatorsIncorrect = true;
            }
            else if (selectionData.indicatorSelection.mainbodySelected == false)
            {
                encounterResults.indicatorResults.mainbodyResult = Result.FalseNegative;
                encounterResults.indicatorResults.missedIndicators = encounterResults.indicatorResults.missedIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
        }

        else if (selectionData.indicatorSelection.mainbodySelected == indicatorAnswers.MainBody)
        {
            if (selectionData.indicatorSelection.mainbodySelected == true)
            {
                encounterResults.indicatorResults.mainbodyResult = Result.TruePositive;
                encounterResults.indicatorResults.correctIndicators = encounterResults.indicatorResults.correctIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
            else if (selectionData.indicatorSelection.mainbodySelected == false)
            {
                encounterResults.indicatorResults.mainbodyResult = Result.TrueNegative;
            }
        }



        if (selectionData.indicatorSelection.linkSelected != indicatorAnswers.Link)
        {
            if (selectionData.indicatorSelection.linkSelected == true)
            {
                encounterResults.indicatorResults.linkResult = Result.FalsePositive;
                encounterResults.indicatorResults.incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators + 1;
                encounterResults.indicatorResults.indicatorsIncorrect = true;
            }
            else if (selectionData.indicatorSelection.linkSelected == false)
            {
                encounterResults.indicatorResults.linkResult = Result.FalseNegative;
                encounterResults.indicatorResults.missedIndicators = encounterResults.indicatorResults.missedIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
        }

        else if (selectionData.indicatorSelection.linkSelected == indicatorAnswers.Link)
        {
            if (selectionData.indicatorSelection.linkSelected == true)
            {
                encounterResults.indicatorResults.linkResult = Result.TruePositive;
                encounterResults.indicatorResults.correctIndicators = encounterResults.indicatorResults.correctIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
            else if (selectionData.indicatorSelection.linkSelected == false)
            {
                encounterResults.indicatorResults.linkResult = Result.TrueNegative;
            }
        }



        if (selectionData.indicatorSelection.endSelected != indicatorAnswers.End)
        {
            if (selectionData.indicatorSelection.endSelected == true)
            {
                encounterResults.indicatorResults.endResult = Result.FalsePositive;
                encounterResults.indicatorResults.incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators + 1;
                encounterResults.indicatorResults.indicatorsIncorrect = true;
            }
            else if (selectionData.indicatorSelection.endSelected == false)
            {
                encounterResults.indicatorResults.endResult = Result.FalseNegative;
                encounterResults.indicatorResults.missedIndicators = encounterResults.indicatorResults.missedIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
        }

        else if (selectionData.indicatorSelection.endSelected == indicatorAnswers.End)
        {
            if (selectionData.indicatorSelection.endSelected == true)
            {
                encounterResults.indicatorResults.endResult = Result.TruePositive;
                encounterResults.indicatorResults.correctIndicators = encounterResults.indicatorResults.correctIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
            else if (selectionData.indicatorSelection.endSelected == false)
            {
                encounterResults.indicatorResults.endResult = Result.TrueNegative;
            }
        }



        if (selectionData.indicatorSelection.fileSelected != indicatorAnswers.File)
        {
            if (selectionData.indicatorSelection.fileSelected == true)
            {
                encounterResults.indicatorResults.fileResult = Result.FalsePositive;
                encounterResults.indicatorResults.incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators + 1;
                encounterResults.indicatorResults.indicatorsIncorrect = true;
            }
            else if (selectionData.indicatorSelection.fileSelected == false)
            {
                encounterResults.indicatorResults.fileResult = Result.FalseNegative;
                encounterResults.indicatorResults.missedIndicators = encounterResults.indicatorResults.missedIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
        }

        else if (selectionData.indicatorSelection.fileSelected == indicatorAnswers.File)
        {
            if (selectionData.indicatorSelection.fileSelected == true)
            {
                encounterResults.indicatorResults.fileResult = Result.TruePositive;
                encounterResults.indicatorResults.correctIndicators = encounterResults.indicatorResults.correctIndicators + 1;
                encounterResults.indicatorResults.totalIndicators = encounterResults.indicatorResults.totalIndicators + 1;
            }
            else if (selectionData.indicatorSelection.fileSelected == false)
            {
                encounterResults.indicatorResults.fileResult = Result.TrueNegative;
            }
        }
    }





    public void ResponsesCheck() //Checks the results of every response individually & calculates the results
    {
        if (selectionData.responseSelection.emailReply == responseAnswers.Reply) //Checks if the value for email reply and Reply answer are equal (the player has made a correct email reply response selection)
        {
            if (selectionData.responseSelection.emailReply == true) //Checks if the email reply value is true, therefore the Reply answer must also be true (the player chose a correct response)
            {
                encounterResults.responseResults.replyResult = Result.TruePositive;
                encounterResults.responseResults.responseCorrect = true;
            }
            else if (selectionData.responseSelection.emailReply == false) //Checks if the email reply value is false, therefore the Reply answer must also be false (the player correctly avoided an incorrect response)
            {
                encounterResults.responseResults.replyResult = Result.TrueNegative;
            }
        }

        else if (selectionData.responseSelection.emailReply != responseAnswers.Reply) //Checks if the value for email reply and Reply answer are not equal (the player has made an incorrect email reply response selection, or has not selected another acceptable reply)
        {
            if (selectionData.responseSelection.emailReply == true) //Checks if the email reply value is true, therefore the Reply answer must be false (the player chose an incorrect response)
            {
                encounterResults.responseResults.replyResult = Result.FalsePositive;
                encounterResults.responseResults.responseCorrect = false;
            }
            else if (selectionData.responseSelection.emailReply == false) //Checks if the email reply value is false, therefore the Reply answer must be true (the player missed an accepted response, whether or not their selected response was correct or not)
            {
                encounterResults.responseResults.replyResult = Result.FalseNegative;
            }
        }


        //Above answer checking & results calculating process for email reply response is repeated for all remaining response types
    }





    public void IndicatorsGrading()
    {
        if (encounterResults.indicatorResults.correctIndicators == encounterResults.indicatorResults.totalIndicators)
        {
            if (encounterResults.indicatorResults.correctIndicators == 0)
            {
                encounterResults.indicatorResults.indicatorGrade = Grade.Unrequired;
            }
            else
            {
                encounterResults.indicatorResults.indicatorGrade = Grade.All;
            }
        }
        else if (encounterResults.indicatorResults.correctIndicators < encounterResults.indicatorResults.totalIndicators && encounterResults.indicatorResults.correctIndicators != 0)
        {
            encounterResults.indicatorResults.indicatorGrade = Grade.Some;
        }
        else
        {
            encounterResults.indicatorResults.indicatorGrade = Grade.None;
        }
    }
}






[System.Serializable]
public class AllAnswers //Stores all the answers data from the JSON Answers file
{
    public EncounterAnswers Tutorial;
    public EncounterAnswers Encounter1;
    public EncounterAnswers Encounter2;
}



[System.Serializable]
public class EncounterAnswers //Stores all the answers data for a particular encounter
{
    public IndicatorAnswers Indicators;
    public ResponseAnswers Response;
}



[System.Serializable]
public class IndicatorAnswers //Stores the answers data for the indicators
{
    public bool Subject;
    public bool Sender;
    public bool Introduction;
    public bool MainBody;
    public bool Link;
    public bool End;
    public bool File;
}



[System.Serializable]
public class ResponseAnswers //Stores the answers data for the responses
{
    public bool OpenLink;
    public bool OpenFile;
    public bool Reply;
    public bool Delete;
    public bool Report;
}

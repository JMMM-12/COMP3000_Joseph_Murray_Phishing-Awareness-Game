using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Feedback_Determining : MonoBehaviour
{
    private string fileName = "Feedback";
    private TextAsset readFeedback;
    public AllFeedback allFeedback;
    private IndicatorsGeneral indicatorsGeneral;
    private IndicatorsSpecific indicatorsSpecific;
    private ResponsesGeneral responsesGeneral;
    private ResponsesSpecific responsesSpecific;
    private Feedback feedback;

    public List<FeedbackDialogue> feedbackDialogues;

    public GameStateManager gameStateManager;
    public EncounterResults encounterResults;

    void Start()
    {
        readFeedback = new TextAsset();

        try
        {
            readFeedback = Resources.Load<TextAsset>(fileName); //Reads and stores the contents of the feedback JSON file
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message + "\nThe Feedback JSON file was not found");
        }
        catch (IOException e)
        {
            Debug.LogError(e.Message + "\nThe Feedback JSON file could not be read");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\nAn error occured when the Feedback JSON file attempted to be read");
        }
        Debug.Log("Feedback contents were sucessfully read");


        allFeedback = JsonUtility.FromJson<AllFeedback>(readFeedback.text); //Deserializes the read JSON contents into a usable C# object
    }





    void Update()
    {
        if (gameStateManager.encounterState == EncounterState.Feedback && gameStateManager.feedbackState == FeedbackState.FeedbackDetermine) //Checks that the game is in the feedback determining stage of the feedback phase 
        {
            DetermineGeneralIndicatorFeedback();
            DetermineSpecificIndicatorFeedback();
        }
    }





    public void DetermineGeneralIndicatorFeedback() //Determines all the general feedback dialogue to display and adds it to the Feedback Dialogue List
    {
        feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.Intro.ChipModel, allFeedback.indicatorsGeneral.Intro.FeedbackText)); //Adds the intro indicators feedback dialogue


        if (encounterResults.indicatorResults.indicatorGrade == Grade.All) //Checks if the indicators grade is All (the player identified all the indicators)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) //Checks if no indicators were incorrectly identified
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.AllIndicatorsCorrect.ChipModel, allFeedback.indicatorsGeneral.AllIndicatorsCorrect.FeedbackText)); //Adds the corresponding feedback dialogue to the Feedback List
                //Within here - searches through the indicator results and adds the correct indicator's dialogue when a correct indicator result is found (loops until the incorrect dialogue count reaches zero - this will increment)
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //Checks if one or more indicators were incorrectly identified 
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.AllIndicatorsCorrectI.ChipModel, allFeedback.indicatorsGeneral.AllIndicatorsCorrectI.FeedbackText));
                //Within here - searches through the indicator results and adds the incorrect indicator's dialogue when an incorrect indicator result is found (loops until the incorrect dialogue count reaches zero - this will increment)
            }
        }


        else if (encounterResults.indicatorResults.indicatorGrade == Grade.Some) //Checks if the indicators grade is Some (the player identified some indicators and missed some indicators)
        {
            //Process continues for all grades
        }
    }





    public void DetermineSpecificIndicatorFeedback() //Determines all the specific feedback dialogue to display and adds it to the Feedback Dialogue List
    {

    }
}






[System.Serializable]
public class AllFeedback //Stores all the feedback data from the JSON Feedback file
{
    public IndicatorsGeneral indicatorsGeneral;
    public IndicatorsSpecific indicatorsSpecific;
    public ResponsesGeneral responsesGeneral;
    public ResponsesSpecific responsesSpecific;
}



[System.Serializable]
public class IndicatorsGeneral //Stores all the feedback data for general indicator feedback
{
    public Feedback Intro;
    public Feedback AllIndicatorsCorrect;
    public Feedback AllIndicatorsCorrectI;
    public Feedback IndicatorsCorrectlyAvoided;
    public Feedback IndicatorsIncorrectlyAvoided;
    public Feedback SomeIndicatorsCorrect;
    public Feedback SomeIndicatorsCorrectI;
    public Feedback NoIndicatorsCorrect;
    public Feedback NoIndicatorsCorrectI;
    public Feedback Score;
}



[System.Serializable]
public class IndicatorsSpecific //Stores all the feedback data for specific indicator feedback
{
    public Feedback SubjectCorrect;
    public Feedback SubjectIncorrect;
    public Feedback SubjectMissed;
    public Feedback SenderCorrect;
    public Feedback SenderIncorrect;
    public Feedback SenderMissed;
    public Feedback IntroductionCorrect;
    public Feedback IntroductionIncorrect;
    public Feedback IntroductionMissed;
    public Feedback MainBodyCorrect;
    public Feedback MainBodyIncorrect;
    public Feedback MainBodyMissed;
    public Feedback LinkCorrect;
    public Feedback LinkIncorrect;
    public Feedback LinkMissed;
    public Feedback EndCorrect;
    public Feedback EndIncorrect;
    public Feedback EndMissed;
    public Feedback FileCorrect;
    public Feedback FileIncorrect;
    public Feedback FileMissed;
}



[System.Serializable]
public class ResponsesGeneral //Stores all the feedback data for general response feedback
{
    public Feedback Intro;
    public Feedback ResponseCorrect;
    public Feedback ResponseIncorrect;
}



[System.Serializable]
public class ResponsesSpecific //Stores all the feedback data for specific response feedback
{
    public Feedback ReplyCorrect;
    public Feedback ReplyIncorrect;
    public Feedback ReplySuggest;
    public Feedback DeleteCorrect;
    public Feedback DeleteIncorrect;
    public Feedback DeleteSuggest;
    public Feedback ReportCorrect;
    public Feedback ReportIncorrect;
    public Feedback ReportSuggest;
    public Feedback OpenLinkCorrect;
    public Feedback OpenLinkIncorrect;
    public Feedback DownloadFileCorrect;
    public Feedback DownloadFileIncorrect;

}



[System.Serializable]
public class Feedback //Stores a single feedback dialogue
{
    public string ChipModel;
    public string FeedbackText;
}



public class FeedbackDialogue //Stores the feedback dialogue to be displayed
{
    public string ChipModel;
    public string FeedbackText;

    public FeedbackDialogue(string Model, string Text)
    {
        ChipModel = Model;
        FeedbackText = Text;
    }
}

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
    private List<FDialogues> fDialogues;

    public GameStateManager gameStateManager;
    public EncounterResults encounterResults;
    public FeedbackDialogues feedbackDialoguesAsset;

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
            DetermineIndicatorFeedback(); //Determines and assigns all the required feedback from the indicators results
            DetermineResponseFeedback(); //Determines and assigns all the required feedback from the response results
            foreach (var d in feedbackDialogues)
            {
                fDialogues.Add(new FDialogues(d.ChipModel, d.FeedbackText));
            }
            gameStateManager.feedbackState = FeedbackState.FeedbackDisplay;
        }
    }





    public void DetermineIndicatorFeedback() //Determines the general and specific indicator feedback to display, adding them to the Feedback Dialogue List
    {
        feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.Intro.ChipModel, allFeedback.indicatorsGeneral.Intro.FeedbackText)); //Adds the intro indicators feedback dialogue


        if (encounterResults.indicatorResults.indicatorGrade == Grade.All) //Checks if the indicators grade is All (the player identified all the indicators)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) //Checks if no indicators were incorrectly identified (the player identified all the indicators without mistake)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.AllIndicatorsCorrect.ChipModel, allFeedback.indicatorsGeneral.AllIndicatorsCorrect.FeedbackText)); //Adds the corresponding feedback dialogue to the Feedback List
                DetermineCorrectIndicatorFeedback(); //Determines and assigns the feedback to display for each correct indicator result
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //Checks if one or more indicators were incorrectly identified (the player identified all the indicators, but made mistakes)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.AllIndicatorsCorrectI.ChipModel, allFeedback.indicatorsGeneral.AllIndicatorsCorrectI.FeedbackText));
                DetermineCorrectIndicatorFeedback();
                DetermineIncorrectIndicatorFeedback();
            }
        }


        else if (encounterResults.indicatorResults.indicatorGrade == Grade.Some) //Checks if the indicators grade is Some (the player identified some indicators and missed some indicators)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) //Checks if no indicators were incorrectly identified (the player identified some and missed some indicators, but did not incorrectly identify any indicators)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.SomeIndicatorsCorrect.ChipModel, allFeedback.indicatorsGeneral.SomeIndicatorsCorrect.FeedbackText));
                DetermineCorrectIndicatorFeedback();
                DetermineMissedIndicatorFeedback();
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //Checks if one or more indicators were incorrectly identified (the player identifed some, missed some, and incorrecetly identified some indicators)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.SomeIndicatorsCorrectI.ChipModel, allFeedback.indicatorsGeneral.SomeIndicatorsCorrectI.FeedbackText));
                DetermineCorrectIndicatorFeedback();
                DetermineMissedIndicatorFeedback();
                DetermineIncorrectIndicatorFeedback();
            }
        }


        else if (encounterResults.indicatorResults.indicatorGrade == Grade.None) //Checks if the indicators grade is None (the player missed all the indicators)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) // (the player missed all the indicators, but did not incorrectly identify any)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.NoIndicatorsCorrect.ChipModel, allFeedback.indicatorsGeneral.NoIndicatorsCorrect.FeedbackText));
                DetermineMissedIndicatorFeedback();
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //(the player missed all the indicators and incorrectly identified some)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.NoIndicatorsCorrectI.ChipModel, allFeedback.indicatorsGeneral.NoIndicatorsCorrectI.FeedbackText));
                DetermineMissedIndicatorFeedback();
                DetermineIncorrectIndicatorFeedback();
            }
        }


        else if (encounterResults.indicatorResults.indicatorGrade == Grade.Unrequired) //Checks if the indicators grade is Unrequired (therer were no indicators for the player to identify)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) //(the player correctly avoided selecting any indicators when there weren't any)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.IndicatorsCorrectlyAvoided.ChipModel, allFeedback.indicatorsGeneral.IndicatorsCorrectlyAvoided.FeedbackText));
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //(the player incorrectly selected indicators when there weren't any)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsGeneral.IndicatorsIncorrectlyAvoided.ChipModel, allFeedback.indicatorsGeneral.IndicatorsIncorrectlyAvoided.FeedbackText));
                DetermineIncorrectIndicatorFeedback();
            }
        }
    }





    public void DetermineResponseFeedback() //Determines the general and specific response feedback to display, adding them to the Feedback Dialogue List
    {
        feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesGeneral.Intro.ChipModel, allFeedback.responsesGeneral.Intro.FeedbackText));


        if (encounterResults.responseResults.responseCorrect == true) //Checks if the player provided a correct response
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesGeneral.ResponseCorrect.ChipModel, allFeedback.responsesGeneral.ResponseCorrect.FeedbackText));
            DetermineCorrectResponseFeedback();
        }

        else if (encounterResults.responseResults.responseCorrect == false) //Checks if the player provided an incorrect response
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesGeneral.ResponseIncorrect.ChipModel, allFeedback.responsesGeneral.ResponseIncorrect.FeedbackText));
            DetermineIncorrectResponseFeedback();
            SuggestResponseFeedback();
        }
    }





    public void DetermineCorrectIndicatorFeedback() //Determines and assigns the feedback to display for correct indicators
    {
        if (encounterResults.indicatorResults.subjectResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.SubjectCorrect.ChipModel, allFeedback.indicatorsSpecific.SubjectCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.senderResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.SenderCorrect.ChipModel, allFeedback.indicatorsSpecific.SenderCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.introductionResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.IntroductionCorrect.ChipModel, allFeedback.indicatorsSpecific.IntroductionCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.mainbodyResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.MainBodyCorrect.ChipModel, allFeedback.indicatorsSpecific.MainBodyCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.linkResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.LinkCorrect.ChipModel, allFeedback.indicatorsSpecific.LinkCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.endResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.EndCorrect.ChipModel, allFeedback.indicatorsSpecific.EndCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.fileResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.FileCorrect.ChipModel, allFeedback.indicatorsSpecific.FileCorrect.FeedbackText));
        }
    }





    public void DetermineIncorrectIndicatorFeedback() //Determines and assigns the feedback to display for incorrect indicators
    {
        if (encounterResults.indicatorResults.subjectResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.SubjectIncorrect.ChipModel, allFeedback.indicatorsSpecific.SubjectIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.senderResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.SenderIncorrect.ChipModel, allFeedback.indicatorsSpecific.SenderIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.introductionResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.IntroductionIncorrect.ChipModel, allFeedback.indicatorsSpecific.IntroductionIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.mainbodyResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.MainBodyIncorrect.ChipModel, allFeedback.indicatorsSpecific.MainBodyIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.linkResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.LinkIncorrect.ChipModel, allFeedback.indicatorsSpecific.LinkIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.endResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.EndIncorrect.ChipModel, allFeedback.indicatorsSpecific.EndIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.fileResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.FileIncorrect.ChipModel, allFeedback.indicatorsSpecific.FileIncorrect.FeedbackText));
        }
    }





    public void DetermineMissedIndicatorFeedback() //Determines and assigns the feedback to display for missed indicators
    {
        if (encounterResults.indicatorResults.subjectResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.SubjectMissed.ChipModel, allFeedback.indicatorsSpecific.SubjectMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.senderResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.SenderMissed.ChipModel, allFeedback.indicatorsSpecific.SenderMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.introductionResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.IntroductionMissed.ChipModel, allFeedback.indicatorsSpecific.IntroductionMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.mainbodyResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.MainBodyMissed.ChipModel, allFeedback.indicatorsSpecific.MainBodyMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.linkResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.LinkMissed.ChipModel, allFeedback.indicatorsSpecific.LinkMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.endResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.EndMissed.ChipModel, allFeedback.indicatorsSpecific.EndMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.fileResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.indicatorsSpecific.FileMissed.ChipModel, allFeedback.indicatorsSpecific.FileMissed.FeedbackText));
        }
    }





    public void DetermineCorrectResponseFeedback()
    {
        if (encounterResults.responseResults.linkResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.OpenLinkCorrect.ChipModel, allFeedback.responsesSpecific.OpenLinkCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.fileResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.DownloadFileCorrect.ChipModel, allFeedback.responsesSpecific.DownloadFileCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.replyResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.ReplyCorrect.ChipModel, allFeedback.responsesSpecific.ReplyCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.deleteResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.DeleteCorrect.ChipModel, allFeedback.responsesSpecific.DeleteCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.reportResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.ReportCorrect.ChipModel, allFeedback.responsesSpecific.ReportCorrect.FeedbackText));
        }
    }





    public void DetermineIncorrectResponseFeedback()
    {
        if (encounterResults.responseResults.linkResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.OpenLinkIncorrect.ChipModel, allFeedback.responsesSpecific.OpenLinkIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.fileResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.DownloadFileIncorrect.ChipModel, allFeedback.responsesSpecific.DownloadFileIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.replyResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.ReplyIncorrect.ChipModel, allFeedback.responsesSpecific.ReplyIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.deleteResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.DeleteIncorrect.ChipModel, allFeedback.responsesSpecific.DeleteIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.reportResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.ReportIncorrect.ChipModel, allFeedback.responsesSpecific.ReportIncorrect.FeedbackText));
        }
    }





    public void SuggestResponseFeedback()
    {
        if (encounterResults.responseResults.replyResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.ReplySuggest.ChipModel, allFeedback.responsesSpecific.ReplyIncorrect.FeedbackText));
        }

        else if (encounterResults.responseResults.deleteResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.DeleteIncorrect.ChipModel, allFeedback.responsesSpecific.DeleteIncorrect.FeedbackText));
        }

        else if (encounterResults.responseResults.reportResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.responsesSpecific.ReportIncorrect.ChipModel, allFeedback.responsesSpecific.ReportIncorrect.FeedbackText));
        }
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
public class IndicatorsGeneral //Stores the feedback data for general indicator feedback
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
public class IndicatorsSpecific //Stores the feedback data for specific indicator feedback
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
public class ResponsesGeneral //Stores the feedback data for general response feedback
{
    public Feedback Intro;
    public Feedback ResponseCorrect;
    public Feedback ResponseIncorrect;
}



[System.Serializable]
public class ResponsesSpecific //Stores the feedback data for specific response feedback
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
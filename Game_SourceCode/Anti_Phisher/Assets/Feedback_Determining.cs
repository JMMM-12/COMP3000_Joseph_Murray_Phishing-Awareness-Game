using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Feedback_Determining : MonoBehaviour
{
    private string fileName = "Feedback";
    private TextAsset readFeedback;
    public AllFeedback allFeedback;
    private IndicatorsGeneral IndicatorsGeneral;
    private IndicatorsSpecific IndicatorsSpecific;
    private ResponsesGeneral ResponsesGeneral;
    private ResponsesSpecific ResponsesSpecific;
    private Feedback feedback;

    public List<FeedbackDialogue> feedbackDialogues;
    private List<FDialogues> fDialogues;

    public GameStateManager gameStateManager;
    public EncounterResults encounterResults;
    public FeedbackDialogues feedbackDialoguesAsset;

    void Start()
    {
        readFeedback = new TextAsset();
        allFeedback = new AllFeedback();

        feedbackDialogues = new List<FeedbackDialogue>();
        fDialogues = new List<FDialogues>();

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
        Debug.Log("Feedback contents were sucessfully deserialized");
    }





    void Update()
    {
        if (gameStateManager.encounterState == EncounterState.Feedback && gameStateManager.feedbackState == FeedbackState.FeedbackDetermine) //Checks that the game is in the feedback determining stage of the feedback phase 
        {
            if (allFeedback == null)
            {
                Debug.LogError("allFeedback is null");
            }
            else if (allFeedback.IndicatorsGeneral == null)
            {
                Debug.LogError("allFeedback > IndicatorsGeneral is null");
            }
            else if (allFeedback.IndicatorsGeneral.Intro == null)
            {
                Debug.LogError("allFeedback > IndicatorsGeneral > Intro is null");
            }
            else if (allFeedback.IndicatorsGeneral.Intro.ChipModel == null)
            {
                Debug.LogError("allFeedback > IndicatorsGeneral > Intro > ChipModel is null");
            }
            else if (allFeedback.IndicatorsGeneral.Intro.FeedbackText == null)
            {
                Debug.LogError("allFeedback > IndicatorsGeneral > Intro > FeedbackText is null");
            }
            else if(feedbackDialogues == null)
            {
                Debug.LogError("feedbackdialogues is null");
            }

            DetermineIndicatorFeedback(); //Determines and assigns all the required feedback from the indicators results
            DetermineResponseFeedback(); //Determines and assigns all the required feedback from the response results

            Debug.Log("Feedback Intro contents from read & deserialized file: " + allFeedback.IndicatorsGeneral.Intro.ChipModel + " And: " + allFeedback.IndicatorsGeneral.Intro.FeedbackText);
            Debug.Log("Feedback Intro contents from feedback dialogues variable: " + allFeedback.IndicatorsGeneral.Intro.ChipModel + " And: " + allFeedback.IndicatorsGeneral.Intro.FeedbackText);

            foreach (var d in feedbackDialogues) //Assign the feedback dialogues to a new object type for display
            {
                Debug.Log("Feedback to be displayed: " + d.ChipModel + " And: " + d.FeedbackText);
                fDialogues.Add(new FDialogues(d.ChipModel, d.FeedbackText));
            }

            feedbackDialoguesAsset.feedbackDialogues = fDialogues; //Assigns the feedback dialogues to the global asset for display

            gameStateManager.feedbackState = FeedbackState.FeedbackDisplay;
            Debug.Log("Feedback Determining moved to Feedback Display");
        }
    }





    public void DetermineIndicatorFeedback() //Determines the general and specific indicator feedback to display, adding them to the Feedback Dialogue List
    {
        feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.Intro.ChipModel, allFeedback.IndicatorsGeneral.Intro.FeedbackText)); //Adds the intro indicators feedback dialogue


        if (encounterResults.indicatorResults.indicatorGrade == Grade.All) //Checks if the indicators grade is All (the player identified all the indicators)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) //Checks if no indicators were incorrectly identified (the player identified all the indicators without mistake)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.AllIndicatorsCorrect.ChipModel, allFeedback.IndicatorsGeneral.AllIndicatorsCorrect.FeedbackText)); //Adds the corresponding feedback dialogue to the Feedback List
                DetermineCorrectIndicatorFeedback(); //Determines and assigns the feedback to display for each correct indicator result
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //Checks if one or more indicators were incorrectly identified (the player identified all the indicators, but made mistakes)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.AllIndicatorsCorrectI.ChipModel, allFeedback.IndicatorsGeneral.AllIndicatorsCorrectI.FeedbackText));
                DetermineCorrectIndicatorFeedback();
                DetermineIncorrectIndicatorFeedback();
            }
        }


        else if (encounterResults.indicatorResults.indicatorGrade == Grade.Some) //Checks if the indicators grade is Some (the player identified some indicators and missed some indicators)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) //Checks if no indicators were incorrectly identified (the player identified some and missed some indicators, but did not incorrectly identify any indicators)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.SomeIndicatorsCorrect.ChipModel, allFeedback.IndicatorsGeneral.SomeIndicatorsCorrect.FeedbackText));
                DetermineCorrectIndicatorFeedback();
                DetermineMissedIndicatorFeedback();
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //Checks if one or more indicators were incorrectly identified (the player identifed some, missed some, and incorrecetly identified some indicators)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.SomeIndicatorsCorrectI.ChipModel, allFeedback.IndicatorsGeneral.SomeIndicatorsCorrectI.FeedbackText));
                DetermineCorrectIndicatorFeedback();
                DetermineMissedIndicatorFeedback();
                DetermineIncorrectIndicatorFeedback();
            }
        }


        else if (encounterResults.indicatorResults.indicatorGrade == Grade.None) //Checks if the indicators grade is None (the player missed all the indicators)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) // (the player missed all the indicators, but did not incorrectly identify any)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.NoIndicatorsCorrect.ChipModel, allFeedback.IndicatorsGeneral.NoIndicatorsCorrect.FeedbackText));
                DetermineMissedIndicatorFeedback();
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //(the player missed all the indicators and incorrectly identified some)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.NoIndicatorsCorrectI.ChipModel, allFeedback.IndicatorsGeneral.NoIndicatorsCorrectI.FeedbackText));
                DetermineMissedIndicatorFeedback();
                DetermineIncorrectIndicatorFeedback();
            }
        }


        else if (encounterResults.indicatorResults.indicatorGrade == Grade.Unrequired) //Checks if the indicators grade is Unrequired (therer were no indicators for the player to identify)
        {
            if (encounterResults.indicatorResults.indicatorsIncorrect == false) //(the player correctly avoided selecting any indicators when there weren't any)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.IndicatorsCorrectlyAvoided.ChipModel, allFeedback.IndicatorsGeneral.IndicatorsCorrectlyAvoided.FeedbackText));
            }

            else if (encounterResults.indicatorResults.indicatorsIncorrect == true) //(the player incorrectly selected indicators when there weren't any)
            {
                feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsGeneral.IndicatorsIncorrectlyAvoided.ChipModel, allFeedback.IndicatorsGeneral.IndicatorsIncorrectlyAvoided.FeedbackText));
                DetermineIncorrectIndicatorFeedback();
            }
        }
    }





    public void DetermineResponseFeedback() //Determines the general and specific response feedback to display, adding them to the Feedback Dialogue List
    {
        feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesGeneral.Intro.ChipModel, allFeedback.ResponsesGeneral.Intro.FeedbackText));


        if (encounterResults.responseResults.responseCorrect == true) //Checks if the player provided a correct response
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesGeneral.ResponseCorrect.ChipModel, allFeedback.ResponsesGeneral.ResponseCorrect.FeedbackText));
            DetermineCorrectResponseFeedback();
        }

        else if (encounterResults.responseResults.responseCorrect == false) //Checks if the player provided an incorrect response
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesGeneral.ResponseIncorrect.ChipModel, allFeedback.ResponsesGeneral.ResponseIncorrect.FeedbackText));
            DetermineIncorrectResponseFeedback();
            SuggestResponseFeedback();
        }
    }





    public void DetermineCorrectIndicatorFeedback() //Determines and assigns the feedback to display for correct indicators
    {
        if (encounterResults.indicatorResults.subjectResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.SubjectCorrect.ChipModel, allFeedback.IndicatorsSpecific.SubjectCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.senderResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.SenderCorrect.ChipModel, allFeedback.IndicatorsSpecific.SenderCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.introductionResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.IntroductionCorrect.ChipModel, allFeedback.IndicatorsSpecific.IntroductionCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.mainbodyResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.MainBodyCorrect.ChipModel, allFeedback.IndicatorsSpecific.MainBodyCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.linkResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.LinkCorrect.ChipModel, allFeedback.IndicatorsSpecific.LinkCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.endResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.EndCorrect.ChipModel, allFeedback.IndicatorsSpecific.EndCorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.fileResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.FileCorrect.ChipModel, allFeedback.IndicatorsSpecific.FileCorrect.FeedbackText));
        }
    }





    public void DetermineIncorrectIndicatorFeedback() //Determines and assigns the feedback to display for incorrect indicators
    {
        if (encounterResults.indicatorResults.subjectResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.SubjectIncorrect.ChipModel, allFeedback.IndicatorsSpecific.SubjectIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.senderResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.SenderIncorrect.ChipModel, allFeedback.IndicatorsSpecific.SenderIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.introductionResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.IntroductionIncorrect.ChipModel, allFeedback.IndicatorsSpecific.IntroductionIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.mainbodyResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.MainBodyIncorrect.ChipModel, allFeedback.IndicatorsSpecific.MainBodyIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.linkResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.LinkIncorrect.ChipModel, allFeedback.IndicatorsSpecific.LinkIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.endResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.EndIncorrect.ChipModel, allFeedback.IndicatorsSpecific.EndIncorrect.FeedbackText));
        }

        if (encounterResults.indicatorResults.fileResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.FileIncorrect.ChipModel, allFeedback.IndicatorsSpecific.FileIncorrect.FeedbackText));
        }
    }





    public void DetermineMissedIndicatorFeedback() //Determines and assigns the feedback to display for missed indicators
    {
        if (encounterResults.indicatorResults.subjectResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.SubjectMissed.ChipModel, allFeedback.IndicatorsSpecific.SubjectMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.senderResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.SenderMissed.ChipModel, allFeedback.IndicatorsSpecific.SenderMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.introductionResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.IntroductionMissed.ChipModel, allFeedback.IndicatorsSpecific.IntroductionMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.mainbodyResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.MainBodyMissed.ChipModel, allFeedback.IndicatorsSpecific.MainBodyMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.linkResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.LinkMissed.ChipModel, allFeedback.IndicatorsSpecific.LinkMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.endResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.EndMissed.ChipModel, allFeedback.IndicatorsSpecific.EndMissed.FeedbackText));
        }

        if (encounterResults.indicatorResults.fileResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.IndicatorsSpecific.FileMissed.ChipModel, allFeedback.IndicatorsSpecific.FileMissed.FeedbackText));
        }
    }





    public void DetermineCorrectResponseFeedback()
    {
        if (encounterResults.responseResults.linkResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.OpenLinkCorrect.ChipModel, allFeedback.ResponsesSpecific.OpenLinkCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.fileResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.DownloadFileCorrect.ChipModel, allFeedback.ResponsesSpecific.DownloadFileCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.replyResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.ReplyCorrect.ChipModel, allFeedback.ResponsesSpecific.ReplyCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.deleteResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.DeleteCorrect.ChipModel, allFeedback.ResponsesSpecific.DeleteCorrect.FeedbackText));
        }

        if (encounterResults.responseResults.reportResult == Result.TruePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.ReportCorrect.ChipModel, allFeedback.ResponsesSpecific.ReportCorrect.FeedbackText));
        }
    }





    public void DetermineIncorrectResponseFeedback()
    {
        if (encounterResults.responseResults.linkResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.OpenLinkIncorrect.ChipModel, allFeedback.ResponsesSpecific.OpenLinkIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.fileResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.DownloadFileIncorrect.ChipModel, allFeedback.ResponsesSpecific.DownloadFileIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.replyResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.ReplyIncorrect.ChipModel, allFeedback.ResponsesSpecific.ReplyIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.deleteResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.DeleteIncorrect.ChipModel, allFeedback.ResponsesSpecific.DeleteIncorrect.FeedbackText));
        }

        if (encounterResults.responseResults.reportResult == Result.FalsePositive)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.ReportIncorrect.ChipModel, allFeedback.ResponsesSpecific.ReportIncorrect.FeedbackText));
        }
    }





    public void SuggestResponseFeedback()
    {
        if (encounterResults.responseResults.replyResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.ReplySuggest.ChipModel, allFeedback.ResponsesSpecific.ReplySuggest.FeedbackText));
        }

        else if (encounterResults.responseResults.deleteResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.DeleteSuggest.ChipModel, allFeedback.ResponsesSpecific.DeleteSuggest.FeedbackText));
        }

        else if (encounterResults.responseResults.reportResult == Result.FalseNegative)
        {
            feedbackDialogues.Add(new FeedbackDialogue(allFeedback.ResponsesSpecific.ReportSuggest.ChipModel, allFeedback.ResponsesSpecific.ReportSuggest.FeedbackText));
        }
    }
}






[System.Serializable]
public class AllFeedback //Stores all the feedback data from the JSON Feedback file
{
    public IndicatorsGeneral IndicatorsGeneral;
    public IndicatorsSpecific IndicatorsSpecific;
    public ResponsesGeneral ResponsesGeneral;
    public ResponsesSpecific ResponsesSpecific;
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
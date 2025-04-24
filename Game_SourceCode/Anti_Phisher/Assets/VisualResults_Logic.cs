using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisualResults_Logic : MonoBehaviour //Highlights the part of the email where the player got answers correct, incorrect, and missed correct answers (using colors)
{

    GameStateManager gameStateManager;
    EncounterResults encounterResults;
    GameData gameData;

    public Image subjectImg;
    public Image senderImg;
    public Image introductionImg;
    public Image mainbodyImg;
    public Image linkImg;
    public Image endImg;
    public Image fileImg;
    public Image SubjectFeedbackIcon;
    public Image SenderFeedbackIcon;
    public Image IntroductionFeedbackIcon;
    public Image MainBodyFeedbackIcon;
    public Image LinkFeedbackIcon;
    public Image EndFeedbackIcon;
    public Image FileFeedbackIcon;
    public Text SubjectFeedbackTxt;
    public Text SenderFeedbackTxt;
    public Text IntroductionFeedbackTxt;
    public Text MainBodyFeedbackTxt;
    public Text LinkFeedbackTxt;
    public Text EndFeedbackTxt;
    public Text FileFeedbackTxt;
    

    private IndicatorResults indicatorResults;
    private ResponseResults responseResults;

    private Color correctColor;
    private Color incorrectColor;
    private Color missedColor;
    private Color transparent;

    public Sprite Cross;
    public Sprite Tick;
    public Sprite Exclamation;

    private string correctFeedback;
    private string incorrectFeedback;
    private string missedFeedback;

    private bool feedbackDone;


    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;
        encounterResults = GameManager.Instance.encounterResults;
        gameData = GameManager.Instance.gameData;

        //Defines the colors
        correctColor = Color.green;
        incorrectColor = Color.red;
        missedColor = Color.yellow;
        transparent = gameData.transparentColor;

        correctFeedback = "Correct!";
        incorrectFeedback = "Incorrect";
        missedFeedback = "Missed Indicator";

        indicatorResults = encounterResults.indicatorResults;
        responseResults = encounterResults.responseResults;

        feedbackDone = false;
    }


    void Update()
    {
        if (gameStateManager.encounterState == EncounterState.IFeedback && gameStateManager.answerCheckRequired == false && gameStateManager.emailDisplayed == true && gameStateManager.highlightersTransparent == true)
        {
            if(feedbackDone == false)
            {
                LoadNewEncounterResults();

                //Determines the colours for all email contents
                subjectImg.color = DetermineColor(indicatorResults.subjectResult);
                senderImg.color = DetermineColor(indicatorResults.senderResult);
                introductionImg.color = DetermineColor(indicatorResults.introductionResult);
                mainbodyImg.color = DetermineColor(indicatorResults.mainbodyResult);
                linkImg.color = DetermineColor(indicatorResults.linkResult);
                endImg.color = DetermineColor(indicatorResults.endResult);
                fileImg.color = DetermineColor(indicatorResults.fileResult);

                //Determine the feedback text to display for each email content
                SubjectFeedbackTxt.text = DetermineText(indicatorResults.subjectResult);
                SenderFeedbackTxt.text = DetermineText(indicatorResults.senderResult);
                IntroductionFeedbackTxt.text = DetermineText(indicatorResults.introductionResult);
                MainBodyFeedbackTxt.text = DetermineText(indicatorResults.mainbodyResult);
                LinkFeedbackTxt.text = DetermineText(indicatorResults.linkResult);
                EndFeedbackTxt.text = DetermineText(indicatorResults.endResult);
                FileFeedbackTxt.text = DetermineText(indicatorResults.fileResult);


                //Determine the feedback icon to display for each email content
                DetermineIcon(SubjectFeedbackIcon, indicatorResults.subjectResult);
                DetermineIcon(SenderFeedbackIcon, indicatorResults.senderResult);
                DetermineIcon(IntroductionFeedbackIcon, indicatorResults.introductionResult);
                DetermineIcon(MainBodyFeedbackIcon, indicatorResults.mainbodyResult);
                DetermineIcon(LinkFeedbackIcon, indicatorResults.linkResult);
                DetermineIcon(EndFeedbackIcon, indicatorResults.endResult);
                DetermineIcon(FileFeedbackIcon, indicatorResults.fileResult);



                feedbackDone = true;
            }
        }

        else
        {
            feedbackDone = false;
        }
    }

    public Color DetermineColor(Result result) //Determines the color to use for each particular highlighter image, based upon the indicator results
    {
        if (result == Result.TruePositive)
        {
            return correctColor;
        }

        else if (result == Result.FalsePositive)
        {
            return incorrectColor;
        }

        else if (result == Result.FalseNegative)
        {
            return missedColor;
        }

        else
        {
            return transparent;
        }
    }

    public string DetermineText (Result result)
    {
        if (result == Result.TruePositive)
        {
            return correctFeedback;
        }

        else if (result == Result.FalsePositive)
        {
            return incorrectFeedback;
        }

        else if (result == Result.FalseNegative)
        {
            return missedFeedback;
        }

        else
        {
            return " ";
        }
    }

    public void DetermineIcon(Image img, Result result)
    {
        if (result == Result.TruePositive)
        {
            img.enabled = true;
            img.sprite = Tick;
        }

        else if (result == Result.FalsePositive)
        {
            img.enabled = true;
            img.sprite = Cross;
        }

        else if (result == Result.FalseNegative)
        {
            img.enabled = true;
            img.sprite = Exclamation;
        }

        else
        {
            img.enabled = false;
        }
    }

    public void LoadNewEncounterResults()
    {
        indicatorResults = encounterResults.indicatorResults;
        responseResults = encounterResults.responseResults;
    }
}

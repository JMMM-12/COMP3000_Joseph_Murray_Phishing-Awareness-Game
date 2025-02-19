using UnityEngine;
using UnityEngine.UI;

public class VisualResults_Logic : MonoBehaviour //Highlights the part of the email where the player got answers correct, incorrect, and missed correct answers (using colors)
{

    public GameStateManager gameStateManager;
    public EncounterResults encounterResults;
    public GameData gameData;

    public Image subjectImg;
    public Image senderImg;
    public Image introductionImg;
    public Image mainbodyImg;
    public Image linkImg;
    public Image endImg;
    public Image fileImg;

    private IndicatorResults indicatorResults;
    private ResponseResults responseResults;

    private Color correctColor;
    private Color incorrectColor;
    private Color missedColor;
    private Color transparent;

    private bool highlightingDone;


    void Start()
    {
        //Defines the colors
        correctColor = Color.green;
        incorrectColor = Color.red;
        missedColor = Color.yellow;
        transparent = gameData.transparentColor;

        indicatorResults = encounterResults.indicatorResults;
        responseResults = encounterResults.responseResults;

        highlightingDone = false;
    }


    void Update()
    {
        if (gameStateManager.encounterState == EncounterState.IFeedback && gameStateManager.answerCheckRequired == false && gameStateManager.emailDisplayed == true && gameStateManager.highlightersTransparent == true)
        {
            if(highlightingDone == false)
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
                highlightingDone = true;
            }
        }

        else
        {
            highlightingDone = false;
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

    public void LoadNewEncounterResults()
    {
        indicatorResults = encounterResults.indicatorResults;
        responseResults = encounterResults.responseResults;
    }
}

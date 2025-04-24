using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay_Logic : MonoBehaviour
{
    GameStateManager gameStateManager;
    EncounterResults encounterResults;
    public Text scoreTxt;

    private int correctindicators;
    private int incorrectIndicators;
    private int missedIndicators;
    private int totalIndicators;
    private bool responseCorrect;

    private int indicatorsScore;
    private string scoreTextToDisplay;

    bool scoreDisplayDone;


    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;
        encounterResults = GameManager.Instance.encounterResults;

        scoreTextToDisplay = string.Empty;
        LoadNewEncounterResults();
    }



    void Update()
    {
        if (gameStateManager.encounterState == EncounterState.IFeedback && gameStateManager.answerCheckRequired == false && gameStateManager.emailDisplayed == true)
        {
            if (scoreDisplayDone == false)
            {
                LoadNewEncounterResults();
                indicatorsScore = DetermineScore();
                scoreTextToDisplay = DetermineDisplay();
                scoreTxt.text = scoreTextToDisplay;
                scoreDisplayDone = true;
            }
        }

        else if (gameStateManager.encounterState == EncounterState.RFeedback && gameStateManager.answerCheckRequired == false && gameStateManager.emailDisplayed == true)
        {
            if (scoreDisplayDone == false)
            {
                LoadNewEncounterResults();
                if (responseCorrect == false)
                {
                    scoreTextToDisplay = "Your response was incorrect. Good try though.";
                    scoreTxt.text = scoreTextToDisplay;
                }

                else if (responseCorrect == true)
                {
                    scoreTextToDisplay = "Fantastic judgement! your response was correct!";
                    scoreTxt.text = scoreTextToDisplay;
                }

                scoreDisplayDone = true;
            }
        }

        else
        {
            scoreDisplayDone = false;
        }
    }



    public void LoadNewEncounterResults()
    {
        correctindicators = encounterResults.indicatorResults.correctIndicators;
        incorrectIndicators = encounterResults.indicatorResults.incorrectIndicators;
        missedIndicators = encounterResults.indicatorResults.missedIndicators;
        totalIndicators = encounterResults.indicatorResults.totalIndicators;
        responseCorrect = encounterResults.responseResults.responseCorrect;
    }



    public int DetermineScore()
    {
        int newscore = correctindicators - missedIndicators - incorrectIndicators;
        if (newscore < 0)
        {
            newscore = 0;
        }

        return newscore;
    }



    public string DetermineDisplay()
    {
        string newText;
        if (indicatorsScore == totalIndicators)
        {
            newText = $"Fantastic work! You got: {indicatorsScore}/{totalIndicators}";
            return newText;
        }

        else if (indicatorsScore < totalIndicators && indicatorsScore != 0)
        {
            newText = $"Nice one! You got: {indicatorsScore}/{totalIndicators}";
            return newText;
        }

        else if (indicatorsScore == 0)
        {
            newText = $"Good try, but your score was {indicatorsScore}/{totalIndicators}";
            return newText;
        }

        else
        {
            return "Sorry I'm not sure what you score got.";
        }
    }
}

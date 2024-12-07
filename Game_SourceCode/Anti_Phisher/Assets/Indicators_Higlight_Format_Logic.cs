using UnityEngine;
using UnityEngine.UI;

public class Indicators_Higlight_Format_Logic : MonoBehaviour //Matches the size and position of the indicator image objects (for highlighting indicators) to that of the related indicator text object
{
    public GameStateManager gameStateManager;
    bool highlightersMatched;

    //Declares all the involved text and image GameObjects
    public GameObject subjectTxt;
    public Image subjectImg;

    //Used for retrieving and changing the size and poisition values
    private RectTransform TxtTransform;
    private RectTransform ImgTransform;

    private void Start()
    {
        highlightersMatched = false;
    }



    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (highlightersMatched == false) //Checks that the indicators highlight images have not already been activated
                    {
                        MatchImageToText();
                        highlightersMatched = true;
                    }
                }
            }
        }

        else
        {
            highlightersMatched = false;
        }
    }




    public void MatchImageToText()
    {
        TxtTransform = subjectTxt.GetComponent<RectTransform>();
        ImgTransform = subjectImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;
    }

    public void DeactivateImages()
    {

    }
}

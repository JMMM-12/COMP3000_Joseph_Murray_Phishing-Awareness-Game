using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class Indicators_Higlight_Format_Logic : MonoBehaviour //Matches the size and position of the indicator image objects (for highlighting indicators) to that of the related indicator text object
{
    public GameStateManager gameStateManager;
    public GameData gameData;

    //Declares all the involved text and image indicator GameObjects
    public GameObject subjectTxt;
    public GameObject senderTxt;
    public GameObject introductionTxt;
    public GameObject mainbodyTxt;
    public GameObject linkTxt;
    public GameObject endTxt;
    public GameObject fileTxt;
    public Image subjectImg;
    public Image senderImg;
    public Image introductionImg;
    public Image mainbodyImg;
    public Image linkImg;
    public Image endImg;
    public Image fileImg;

    //Used for retrieving and changing the size and poisition values
    private RectTransform TxtTransform;
    private RectTransform ImgTransform;

    private Color transparentImage;


    private void Start()
    {
        gameStateManager.highlightersReady = false;
        gameStateManager.highlightersTransparent = false;
        transparentImage = gameData.transparentColor;
    }



    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (gameStateManager.highlightersReady == false) //Checks that the indicators highlight images have not already been matched with the text
                    {
                        MatchImageToText();
                        SetImagesAsTransparent();
                        gameStateManager.highlightersReady = true;
                    }
                }
            }

            else if (gameStateManager.encounterState == EncounterState.IFeedback)
            {
                if (gameStateManager.highlightersTransparent == false)
                {
                    SetImagesAsTransparent();
                    gameStateManager.highlightersTransparent = true;
                }
            }

            else
            {
                gameStateManager.highlightersReady = false;
                gameStateManager.highlightersTransparent = false;
            }
        }

        else
        {
            gameStateManager.highlightersReady = false;
            gameStateManager.highlightersTransparent = false;
        }
    }




    public void MatchImageToText() //Matches the image objects to the text objects
    {
        //Subject matching
        TxtTransform = subjectTxt.GetComponent<RectTransform>();
        ImgTransform = subjectImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;

        //Sender matching
        TxtTransform = senderTxt.GetComponent<RectTransform>();
        ImgTransform = senderImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;

        //Introduction matching
        TxtTransform = introductionTxt.GetComponent<RectTransform>();
        ImgTransform = introductionImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;

        //Main Body matching
        TxtTransform = mainbodyTxt.GetComponent<RectTransform>();
        ImgTransform = mainbodyImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;

        //Link matching
        TxtTransform = linkTxt.GetComponent<RectTransform>();
        ImgTransform = linkImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;

        //End matching
        TxtTransform = endTxt.GetComponent<RectTransform>();
        ImgTransform = endImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;

        //File matching
        TxtTransform = fileTxt.GetComponent<RectTransform>();
        ImgTransform = fileImg.GetComponent<RectTransform>();
        ImgTransform.sizeDelta = TxtTransform.sizeDelta;
        ImgTransform.position = TxtTransform.position;
    }

    public void SetImagesAsTransparent() //Sets the image objects to transparent to start (since they will start unselected)
    {
        subjectImg.color = transparentImage;
        senderImg.color = transparentImage;
        introductionImg.color = transparentImage;
        mainbodyImg.color = transparentImage;
        linkImg.color = transparentImage;
        endImg.color = transparentImage;
        fileImg.color = transparentImage;
    }
}

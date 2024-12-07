using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class Indicators_Higlight_Format_Logic : MonoBehaviour //Matches the size and position of the indicator image objects (for highlighting indicators) to that of the related indicator text object
{
    public GameStateManager gameStateManager;
    bool highlightersReady; //for tracking if the indicator highlighting images are matched with the text's size and position

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

    private Color transparentImage = new Color(0f, 0f, 0f, 0f);


    private void Start()
    {
        highlightersReady = false;
    }



    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (highlightersReady == false) //Checks that the indicators highlight images have not already been matched with the text
                    {
                        MatchImageToText();
                        SetImagesAsTransparent();
                        highlightersReady = true;
                    }
                }
            }
        }

        else
        {
            highlightersReady = false;
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

using UnityEngine;
using UnityEngine.UI;

public class Email_Display_Logic : MonoBehaviour //Handles the activation/deactivation of the email's container, text, and buttons
{
    public GameStateManager gameStateManager;

    //Holds the two email container sprites for display
    public Sprite indicatorContainer;
    public Sprite responseContainer;

    private SpriteRenderer emailContainerRenderer;

    //Holds references to the emails text, button, and image GameObjects
    public GameObject SubjectTxt;
    public GameObject SenderTxt;
    public GameObject IntroductionTxt;
    public GameObject MainBodyTxt;
    public GameObject LinkTxt;
    public GameObject EndTxt;
    public GameObject FileTxt;
    public GameObject ReplyButton;
    public GameObject DeleteButton;
    public GameObject ReportButton;
    public GameObject IndicatorsConfirmButton;
    public GameObject ContinueButton;
    public GameObject SubjectImg;
    public GameObject SenderImg;
    public GameObject IntroductionImg;
    public GameObject MainBodyImg;
    public GameObject LinkImg;
    public GameObject EndImg;
    public GameObject FileImg;
    public GameObject ScoreDisplay;

    private int textOffset = 70;
    private RectTransform textRectTransform;


    void Start()
    {
        Debug.Log("Email Display Logic has started");

        emailContainerRenderer = GetComponent<SpriteRenderer>(); //Retrieves the Sprite Renderer for this GameObject
        emailContainerRenderer.sprite = null; //Prevents email container display upon game start
        
        //Deactivates all Text Elements upon game start
        SubjectTxt.SetActive(false);
        SenderTxt.SetActive(false);
        IntroductionTxt.SetActive(false);
        MainBodyTxt.SetActive(false);
        LinkTxt.SetActive(false);
        EndTxt.SetActive(false);
        FileTxt.SetActive(false);
        ScoreDisplay.SetActive(false);

        //Deactivates all the Buttons upon game start
        ReplyButton.SetActive(false);
        DeleteButton.SetActive(false);
        ReportButton.SetActive(false);
        IndicatorsConfirmButton.SetActive(false);
        gameStateManager.emailDisplayed = false;
        ContinueButton.SetActive(false);

        //Deactivates all the images upon game start
        SubjectImg.SetActive(false);
        SenderImg.SetActive(false);
        IntroductionImg.SetActive(false);
        MainBodyImg.SetActive(false);
        LinkImg.SetActive(false);
        EndImg.SetActive(false);
        FileImg.SetActive (false);
    }




    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks if the encounter gameplay is active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks the current encounter state
            {
                if (gameStateManager.emailDisplayed == false)
                {
                    emailContainerRenderer.sprite = indicatorContainer; //Performs different email content activations based upon the encounter state
                    SubjectTxt.SetActive(true);
                    SenderTxt.SetActive(true);
                    IntroductionTxt.SetActive(true);
                    MainBodyTxt.SetActive(true);
                    LinkTxt.SetActive(true);
                    EndTxt.SetActive(true);
                    FileTxt.SetActive(true);
                    if (gameStateManager.EncounterNum != 0)
                    {
                        MoveText();
                    }
                    IndicatorsConfirmButton.SetActive(true);
                    ReplyButton.SetActive(false);
                    DeleteButton.SetActive(false);
                    ReportButton.SetActive(false);
                    SubjectImg.SetActive(true);
                    SenderImg.SetActive(true);
                    IntroductionImg.SetActive(true);
                    MainBodyImg.SetActive(true);
                    LinkImg.SetActive(true);
                    EndImg.SetActive(true);
                    FileImg.SetActive(true);
                    ContinueButton.SetActive(false);
                    ScoreDisplay.SetActive(false);
                    gameStateManager.emailDisplayed = true;
                    Debug.Log("Email Contents for the Indicator state were activated");
                }
                
            }

            else if (gameStateManager.encounterState == EncounterState.IFeedback)
            {
                if (gameStateManager.emailDisplayed == false)
                {
                    IndicatorsConfirmButton.SetActive(false);
                    ContinueButton.SetActive(true);
                    ScoreDisplay.SetActive(true);
                    gameStateManager.emailDisplayed = true;
                }
            }

            else if (gameStateManager.encounterState == EncounterState.Response)
            {
                if (gameStateManager.emailDisplayed == false)
                {
                    emailContainerRenderer.sprite = responseContainer;
                    MoveText();
                    IndicatorsConfirmButton.SetActive(false);
                    SubjectImg.SetActive(false);
                    SenderImg.SetActive(false);
                    IntroductionImg.SetActive(false);
                    MainBodyImg.SetActive(false);
                    LinkImg.SetActive(false);
                    EndImg.SetActive(false);
                    FileImg.SetActive(false);
                    ReplyButton.SetActive(true);
                    DeleteButton.SetActive(true);
                    ReportButton.SetActive(true);
                    ContinueButton.SetActive(false);
                    ScoreDisplay.SetActive(false);
                    gameStateManager.emailDisplayed = true;
                    Debug.Log("Email Contents for the Response state were activated");
                }
                
            }

            else if (gameStateManager.encounterState == EncounterState.RFeedback)
            {
                if (gameStateManager.emailDisplayed == false)
                {
                    ContinueButton.SetActive(true);
                    ScoreDisplay.SetActive(true);
                    gameStateManager.emailDisplayed = true;
                }
            }

            else if (gameStateManager.encounterState == EncounterState.Unknown)
            {
                if (gameStateManager.emailDisplayed == true)
                {
                    emailContainerRenderer.sprite = null;
                    emailContainerRenderer.sprite = null;
                    SubjectTxt.SetActive(false);
                    SenderTxt.SetActive(false);
                    IntroductionTxt.SetActive(false);
                    MainBodyTxt.SetActive(false);
                    LinkTxt.SetActive(false);
                    EndTxt.SetActive(false);
                    FileTxt.SetActive(false);
                    SubjectImg.SetActive(false);
                    SenderImg.SetActive(false);
                    IntroductionImg.SetActive(false);
                    MainBodyImg.SetActive(false);
                    LinkImg.SetActive(false);
                    EndImg.SetActive(false);
                    FileImg.SetActive(false);
                    ReplyButton.SetActive(false);
                    DeleteButton.SetActive(false);
                    ReportButton.SetActive(false);
                    IndicatorsConfirmButton.SetActive(false);
                    ContinueButton.SetActive(false);
                    ScoreDisplay.SetActive(false);
                    gameStateManager.emailDisplayed = false;
                    gameStateManager.emailContentsDisplayed = false;
                    Debug.LogWarning("Encounter State was Unknown. Email Contents were deactivated");
                }
                
            }
        }

        else if (gameStateManager.encounterActive == false) //Checks if the encounter gameplay is inactive
        {
            if (gameStateManager.emailDisplayed == true)
            {
                emailContainerRenderer.sprite = null;
                SubjectTxt.SetActive(false);
                SenderTxt.SetActive(false);
                IntroductionTxt.SetActive(false);
                MainBodyTxt.SetActive(false);
                LinkTxt.SetActive(false);
                EndTxt.SetActive(false);
                FileTxt.SetActive(false);
                SubjectImg.SetActive(false);
                SenderImg.SetActive(false);
                IntroductionImg.SetActive(false);
                MainBodyImg.SetActive(false);
                LinkImg.SetActive(false);
                EndImg.SetActive(false);
                FileImg.SetActive(false);
                ReplyButton.SetActive(false);
                DeleteButton.SetActive(false);
                ReportButton.SetActive(false);
                IndicatorsConfirmButton.SetActive(false);
                ContinueButton.SetActive(false);
                ScoreDisplay.SetActive(false);
                gameStateManager.emailDisplayed = false;
                gameStateManager.emailContentsDisplayed = false;
            }
            
        }
    }




    void MoveText() //Moves the text to fit with the email container
    {
        if (gameStateManager.encounterState == EncounterState.Indicators)
        {
            textRectTransform = SubjectTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y - textOffset, textRectTransform.position.z);
            textRectTransform = SenderTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y - textOffset, textRectTransform.position.z);
            textRectTransform = IntroductionTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y - textOffset, textRectTransform.position.z);
            textRectTransform = MainBodyTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y - textOffset, textRectTransform.position.z);
            textRectTransform = LinkTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y - textOffset, textRectTransform.position.z);
            textRectTransform = EndTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y - textOffset, textRectTransform.position.z);
            textRectTransform = FileTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y - textOffset, textRectTransform.position.z);
            Debug.Log("Email Text Contents were moved");
        }

        else if (gameStateManager.encounterState == EncounterState.Response)
        {
            textRectTransform = SubjectTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y + textOffset, textRectTransform.position.z);
            textRectTransform = SenderTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y + textOffset, textRectTransform.position.z);
            textRectTransform = IntroductionTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y + textOffset, textRectTransform.position.z);
            textRectTransform = MainBodyTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y + textOffset, textRectTransform.position.z);
            textRectTransform = LinkTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y + textOffset, textRectTransform.position.z);
            textRectTransform = EndTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y + textOffset, textRectTransform.position.z);
            textRectTransform = FileTxt.GetComponent<RectTransform>();
            textRectTransform.position = new Vector3(textRectTransform.position.x, textRectTransform.position.y + textOffset, textRectTransform.position.z);
            Debug.Log("Email Text Contents were moved");
        }
    }
}

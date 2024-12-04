using UnityEngine;
using UnityEngine.UI;

public class Email_Display_Logic : MonoBehaviour //Handles the activation/deactivation of the email's container, text, and buttons
{
    public GameStateManager gameStateManager;

    //Holds the two email container sprites for display
    public Sprite indicatorContainer;
    public Sprite responseContainer;

    private SpriteRenderer emailContainerRenderer;

    //Holds references to the emails text & button GameObjects
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

    bool emailDisplayed;


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

        //Deactivates all the Buttons upon game start
        ReplyButton.SetActive(false);
        DeleteButton.SetActive(false);
        ReportButton.SetActive(false);

        emailDisplayed = false;
    }




    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks if the encounter gameplay is active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks the current encounter state
            {
                if (emailDisplayed == false)
                {
                    emailContainerRenderer.sprite = indicatorContainer; //Performs different email content activations based upon the encounter state
                    SubjectTxt.SetActive(true);
                    SenderTxt.SetActive(true);
                    IntroductionTxt.SetActive(true);
                    MainBodyTxt.SetActive(true);
                    LinkTxt.SetActive(true);
                    EndTxt.SetActive(true);
                    FileTxt.SetActive(true);
                    emailDisplayed = true;
                    Debug.Log("Email Contents for the Indicator state were activated");
                }
                
            }

            else if (gameStateManager.encounterState == EncounterState.Response)
            {
                if (emailDisplayed == false) //Note - code will not be reached as long as emailDisplayed is still set as true from the last encounter state
                {
                    emailContainerRenderer.sprite = responseContainer;
                    SubjectTxt.SetActive(true); //Note- the text SetActive code may be redundant as the text may remain active since the last encounter state
                    SenderTxt.SetActive(true);
                    IntroductionTxt.SetActive(true);
                    MainBodyTxt.SetActive(true);
                    LinkTxt.SetActive(true);
                    EndTxt.SetActive(true);
                    FileTxt.SetActive(true);
                    ReplyButton.SetActive(true);
                    DeleteButton.SetActive(true);
                    ReportButton.SetActive(true);
                    emailDisplayed = true;
                    Debug.Log("Email Contents for the Response state were activated");
                }
                
            }

            else if (gameStateManager.encounterState == EncounterState.Feedback)
            {
                if (emailDisplayed == true)
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
                    ReplyButton.SetActive(false);
                    DeleteButton.SetActive(false);
                    ReportButton.SetActive(false);
                    emailDisplayed = false;
                    Debug.Log("Email Contents were deactivated as part of the Feedback state");
                }
                
            }

            else if (gameStateManager.encounterState == EncounterState.Unknown)
            {
                if (emailDisplayed == true)
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
                    ReplyButton.SetActive(false);
                    DeleteButton.SetActive(false);
                    ReportButton.SetActive(false);
                    emailDisplayed = false;
                    Debug.LogWarning("Encounter State was Unknown. Email Contents were deactivated");
                }
                
            }
        }

        else if (gameStateManager.encounterActive == false) //Checks if the encounter gameplay is inactive
        {
            emailContainerRenderer.sprite = null;
            SubjectTxt.SetActive(false);
            SenderTxt.SetActive(false);
            IntroductionTxt.SetActive(false);
            MainBodyTxt.SetActive(false);
            LinkTxt.SetActive(false);
            EndTxt.SetActive(false);
            FileTxt.SetActive(false);
            ReplyButton.SetActive(false);
            DeleteButton.SetActive(false);
            ReportButton.SetActive(false);
            emailDisplayed = false;
        }
    }
}

using UnityEngine;

public class IndicatorLogic : MonoBehaviour
{

    public GameStateManager gameStateManager;
    public SelectionData selectionData;

    //Stores references to the text GameObjects involved in the selection logic
    public GameObject subject;
    public GameObject sender;
    public GameObject introduction;
    public GameObject mainbody;
    public GameObject link;
    public GameObject end;
    public GameObject file;

    void Start()
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks that the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    //Logic here for indicator selection
                }
            }
        }
    }


    void Update()
    {
        
    }
}

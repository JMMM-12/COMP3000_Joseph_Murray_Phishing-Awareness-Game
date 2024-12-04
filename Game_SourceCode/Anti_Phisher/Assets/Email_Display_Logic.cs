using UnityEngine;

public class Email_Display_Logic : MonoBehaviour
{
    public GameStateManager gameStateManager;

    //Holds the two email container sprites for display
    public Sprite indicatorContainer;
    public Sprite responseContainer;

    private SpriteRenderer emailContainerRenderer;


    void Start()
    {
        Debug.Log("Email Display Logic has started");
        emailContainerRenderer = GetComponent<SpriteRenderer>(); //Retrieves the Sprite Renderer for this GameObject
        emailContainerRenderer.sprite = null; //Prevents email container display upon game start
    }




    void Update()
    {
        if (gameStateManager.encounterActive == true) //Checks if the encounter gameplay is active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators)
            {
                emailContainerRenderer.sprite = indicatorContainer;
            }

            else if (gameStateManager.encounterState == EncounterState.Response)
            {
                emailContainerRenderer.sprite = responseContainer;
            }

            else if (gameStateManager.encounterState == EncounterState.Feedback)
            {
                emailContainerRenderer.sprite = responseContainer;
            }

            else if (gameStateManager.encounterState == EncounterState.Unknown)
            {
                emailContainerRenderer.sprite = null;
                Debug.LogWarning("Encounter State was Unknown");
            }
        }

        else if (gameStateManager.encounterActive == false) //Checks if the encounter gameplay is inactive
        {
            emailContainerRenderer.sprite = null;
        }
    }
}

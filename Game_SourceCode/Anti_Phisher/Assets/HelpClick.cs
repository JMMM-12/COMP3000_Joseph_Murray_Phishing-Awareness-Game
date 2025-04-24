using UnityEngine;
using UnityEngine.UI;

public class HelpClick : MonoBehaviour //Triggers the instructions to display once the help button is clicked
{
    GameStateManager gameStateManager;

    public GameObject instrcutionsbox;

    public Button helpButton;


    void Start()
    {
        gameStateManager = GameManager.Instance.gameStateManager;

        helpButton = GetComponent<Button>();

        if (helpButton != null)
        {
            helpButton.onClick.AddListener(OnButtonClick);
            Debug.Log("Help Button OnButtonClick Listener was successfully added");
        }

        else
        {
            Debug.LogWarning("Help Button component was null when trying to add a listener to it");
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("Help Button was clicked");
        instrcutionsbox.SetActive(true);
        gameStateManager.instructionsDisplayed = true;
        gameStateManager.instructionsTextRequired = true;
    }


}

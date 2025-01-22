using UnityEngine;
using UnityEngine.UI;

public class HelpClick : MonoBehaviour //Triggers the instructions to display once the help button is clicked
{
    public GameStateManager gameStateManager;

    public GameObject instrcutionsbox;

    public Button helpButton;


    void Start()
    {
        helpButton = GetComponent<Button>();

        if (helpButton != null)
        {
            helpButton.onClick.AddListener(OnButtonClick);
        }

        else
        {
            Debug.LogWarning("Help Button component was null when trying to add a listener to it");
        }
    }

    public void OnButtonClick()
    {
        instrcutionsbox.SetActive(true);
        gameStateManager.instructionsDisplayed = true;
        gameStateManager.instructionsTextRequired = true;
    }


}
